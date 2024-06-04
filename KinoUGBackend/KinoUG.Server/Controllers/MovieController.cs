using AutoMapper;
using KinoUG.Server.Data;
using KinoUG.Server.DTO;
using KinoUG.Server.Helper;
using KinoUG.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Controllers
{
    
    public class MovieController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _automapper;
        public MovieController(DataContext context, IMapper automapper)
        {
            _context = context;
            _automapper = automapper;
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
           var movies = await _context.Movies.ToListAsync();
            return _automapper.Map<List<MovieDTO>>(movies);
        }

        
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddMovie(AddMovieDTO film)
        {
            var movie = new Movie
            {
                Title = film.Title,
                Description = film.Description,
                Image = film.Image

            };
            
            _context.Movies.Add(movie);
            
            await _context.SaveChangesAsync();
   

            return Ok(film);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Schedules)
                                              .ThenInclude(s => s.Tickets)
                                              .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            foreach (var schedule in movie.Schedules)
            {
                _context.Tickets.RemoveRange(schedule.Tickets);
            }

            _context.Schedules.RemoveRange(movie.Schedules);

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateMovie(int id, EditMovieDto updatedMovie)
        {

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updatedMovie.Title;
            movie.Description = updatedMovie.Description;
            movie.Image = updatedMovie.Image;
           

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }


    }
}
