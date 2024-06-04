using KinoUG.Server.Data;
using KinoUG.Server.DTO;
using KinoUG.Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; 

namespace KinoUG.Server.Controllers
{
    public class TicketController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketController(DataContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("GetTickets")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
        public async Task<IActionResult> AddTicket(AddTicketDTO addTicketDTO)
        {
            try
            {
                string userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                // Check if the user exists
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    return BadRequest("Invalid User email: the specified user does not exist.");
                }

                var ticketFind = await _context.Tickets
                    .Where(t => t.SeatId.Equals(addTicketDTO.SeatId) && t.ScheduleId.Equals(addTicketDTO.ScheduleId))
                    .FirstOrDefaultAsync();

                if (ticketFind != null)
                {
                    return BadRequest("Ticket for this seat is already sold.");
                }

                var ticket = new Ticket
                {
                    UserId = user.Id,
                    SeatId = addTicketDTO.SeatId,
                    ScheduleId = addTicketDTO.ScheduleId,
                    Price = addTicketDTO.Price,
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                return Ok("Ticket successfully assigned to the seat.");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpGet("ticketDetails/{ticketId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult<Ticket>> GetTicketDetails(int ticketId)
        {
            try
            {
                string userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    return BadRequest("Invalid User email: the specified user does not exist.");
                }

                var ticket = await _context.Tickets
                    .Where(t => t.Id == ticketId && t.UserId == user.Id)
                    .Include(t => t.Seat)
                    .Include(t => t.Schedule)
                    .ThenInclude(s => s.Movie)
                    .Include(u => u.User)
                    .FirstOrDefaultAsync();

                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }

                return Ok(ticket);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("cancel/{ticketId}")]
        
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

                if (ticket == null)
                {
                    return NotFound("Ticket not found.");
                }

                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                return Ok("Ticket has been canceled and seat freed.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }

}
