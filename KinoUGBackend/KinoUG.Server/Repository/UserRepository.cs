using KinoUG.Server.Data;
using KinoUG.Server.Models;
using KinoUG.Server.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;

namespace KinoUG.Server.Repository

{
    [Authorize(Roles = "Admin")]
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
            
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
           return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (email.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(email), "Email is required");
            }
            else
            {
                return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
            }
            
        }
    }
}
