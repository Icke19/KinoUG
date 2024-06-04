using KinoUG.Server.Data;
using KinoUG.Server.Models;
using KinoUG.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KinoUG.Server.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DataContext _context;
        public TicketRepository(DataContext context)
        {
            _context= context;
        }
        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }
    }
}
