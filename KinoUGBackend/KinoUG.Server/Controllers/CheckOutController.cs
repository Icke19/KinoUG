using KinoUG.Server.Data;
using KinoUG.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoUG.Server.Controllers
{
    
    public class CheckoutController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public CheckoutController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("create-checkout-session/{userId}")]
        [Authorize]
        public async Task<ActionResult> CheckoutOrder(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Ticket)
                .ThenInclude(t => t.Schedule)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                return BadRequest("Cart is empty or does not exist.");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.Items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Stripe expects the amount in cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Ticket for {item.Ticket.Schedule.Movie.Title}",
                            Description = $"Seat: {item.Ticket.Seat.SeatNumber}",
                        }
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{_configuration["AppSettings:ClientUrl"]}/checkout/success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_configuration["AppSettings:ClientUrl"]}/checkout/cancel"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            var response = new CheckOutOrderResponse
            {
                SessionId = session.Id,
                PubKey = _configuration["Stripe:PublishableKey"]
            };

            return Ok(response);
        }

        [HttpGet("success")]
        public ActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            // Here you can save order and customer details to your database.
            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;

            return Redirect($"{_configuration["AppSettings:ClientUrl"]}/success");
        }

        [HttpGet("cancel")]
        public ActionResult CheckoutCancel()
        {
            return Redirect($"{_configuration["AppSettings:ClientUrl"]}/cancel");
        }
    }

    
}
