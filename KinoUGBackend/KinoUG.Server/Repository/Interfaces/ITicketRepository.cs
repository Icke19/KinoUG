using KinoUG.Server.Models;

namespace KinoUG.Server.Repository.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(int id);
    }
}
