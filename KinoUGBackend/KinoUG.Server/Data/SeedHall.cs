using KinoUG.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Data
{
    public class SeedHall
    {
        private readonly DataContext _context;

        public SeedHall(DataContext context)
        {
            _context = context;
        }

        public async Task InitializeRoomTemplate()
        {
            if (!await _context.Halls.AnyAsync())
            {
                var hall = new Hall
                {
                    Seats = new List<Seat>()
                };

                for (int row = 1; row <= 5; row++)
                {
                    for (int col = 1; col <= 5; col++)
                    {
                        hall.Seats.Add(new Seat
                        {
                            Row = row,
                            Column = col
                        });
                    }
                }

                _context.Halls.Add(hall);
                await _context.SaveChangesAsync();
            }
        }
    }
}
