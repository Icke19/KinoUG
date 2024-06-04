using KinoUG.Server.Models;

namespace KinoUG.Server.Repository.Interfaces
{
    public interface ITokenService
    {
        Task <string> GenerateJwtToken(User user, TimeSpan expiration);
    }
}
