using KinoUG.Server.Data;
using KinoUG.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Controllers
{

    public class CartController : BaseApiController
    {
        private readonly DataContext _context;
        public CartController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task <IActionResult> getCart(string userId)
        {
           try
            {
                var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Ticket)
                .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("{userId}")]
        [Authorize]
        public async Task<IActionResult> AddToCart(string userId, CartItem cartItem) 
        { 
         try
            {
                var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        Items = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                }
                else
                {
                    cart.Items.Add(cartItem);
                }

                cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                await _context.SaveChangesAsync();


                return Ok(cart);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItemFromCart (string userId, int itemId)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    return NotFound();
                }

                var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
                if (item == null)
                {
                    return NotFound();
                }
                cart.Items.Remove(item);
                cart.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);

                await _context.SaveChangesAsync();
                return Ok(cart);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    
    
    }

}

