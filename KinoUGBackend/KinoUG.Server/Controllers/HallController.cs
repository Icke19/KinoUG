using KinoUG.Server.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Controllers
{
    public class HallController: BaseApiController
    {
        private readonly DataContext _context;
        public HallController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{hallId}")]

        public async Task<IActionResult> GetHall(int hallId)
        {
            var seats = await _context.Seats.Where(s => s.HallId == hallId).Select(s => new
            {
                SeatId = s.Id,
                Row = s.Row,
                Column = s.Column,
            }).ToListAsync();
            var hall = new
            {
                HallId = hallId,
                Seats = seats
            };  
            return Ok(hall);
        }
    }
}
    

