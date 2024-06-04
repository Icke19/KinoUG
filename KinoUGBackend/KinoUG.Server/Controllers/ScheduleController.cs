using KinoUG.Server.Data;
using KinoUG.Server.DTO;
using KinoUG.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Controllers
{
    public class ScheduleController:BaseApiController
    {
        private readonly DataContext _context;
        public ScheduleController(DataContext context)
        {
            _context = context;
        }




        [HttpGet("all")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<MinScheduleDTO>>> GetScheduleList()
        {
            try
            {
                var schedules = await _context.Schedules
                .Include(s => s.Movie)
                .OrderBy(s => s.Id)
                .Skip(3)
                .ToListAsync();
                return schedules.Select(s => new MinScheduleDTO
                {
                    Id = s.Id,
                    MovieTitle = s.Movie.Title,
                    Date = s.Date,
                    Image = s.Movie.Image,
                    Description = s.Movie.Description
                }).ToList();
            }
            catch (System.Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpGet("latest6")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<List<MinScheduleDTO>>> GetScheduleListOf6()
        {
           try
            {
                var schedules = await _context.Schedules
               .Include(s => s.Movie)
               .OrderByDescending(s => s.Date)
               .Take(6)
               .ToListAsync();

                return schedules.Select(s => new MinScheduleDTO
                {
                    Id = s.Id,
                    MovieTitle = s.Movie.Title,
                    Date = s.Date,
                    Image = s.Movie.Image
                }).ToList();
            }
           catch (System.Exception)
            {
                return BadRequest("Error");
            }
        }



        [HttpGet("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
           
             var tickets = await _context.Tickets
                .Include(t => t.Seat)
                .Where(t => t.ScheduleId == id)
                .ToListAsync();

            var schedule = await _context.Schedules
                .Include(s => s.Movie)
                .IgnoreAutoIncludes()
                .Include(s => s.Hall)
                .ThenInclude(h => h.Seats)
                .FirstOrDefaultAsync(s => s.Id == id);

            return new ScheduleDTO
            {
                Id = schedule.Id,
                Movie = new MovieDTO
                {
                    Id = schedule.Movie.Id,
                    Title = schedule.Movie.Title,
                    Description = schedule.Movie.Description,
                    Image = schedule.Movie.Image,
                    
                    
                },
                Seats = schedule.Hall.Seats.Select(s => new SeatDTO
                {
                    Id = s.Id,
                    Row = s.Row,
                    Column = s.Column,
                    IsReserved = tickets.Select(t => t.SeatId).Contains(s.Id)
                }).OrderBy(s=>s.Id).ToList(),
                Date = schedule.Date,
            };
            
            
        }
       
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ScheduleDTO>> AddSchedule(AddScheduleDTO addScheduleDTO)
        {
            var schedule = new Schedule
            {
                MovieId = addScheduleDTO.MovieId,
                HallId = addScheduleDTO.HallId,
                Date = addScheduleDTO.Date,
            };
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            var newSchedule = await _context.Schedules
                
                .Include(s => s.Movie)
                .IgnoreAutoIncludes()
                .Include(s => s.Hall)
                .ThenInclude(h => h.Seats)
                .FirstOrDefaultAsync(s => s.Id == schedule.Id);
            
            return new ScheduleDTO
            {
                Id = newSchedule.Id,
                Movie = new MovieDTO
                {
                    Id = newSchedule.Movie.Id,
                    Title = newSchedule.Movie.Title,
                    Description = newSchedule.Movie.Description
                },
                Date = newSchedule.Date,
                Seats = newSchedule.Hall.Seats.Select(s => new SeatDTO
                {
                    Id = s.Id,
                    Row = s.Row,
                    Column = s.Column,
                    IsReserved = false
                }).ToList()
            };
        }
        
    }
}
