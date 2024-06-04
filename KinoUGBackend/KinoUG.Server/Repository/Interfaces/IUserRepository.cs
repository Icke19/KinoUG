using KinoUG.Server.Models;

namespace KinoUG.Server.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task <IEnumerable<User>> GetUsersAsync();
        Task <User> GetUserByIdAsync(int id);


    }
}
