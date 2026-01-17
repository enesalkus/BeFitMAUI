using BeFitMAUI.Models;
using BeFitMAUI.Data;
using Microsoft.EntityFrameworkCore;

namespace BeFitMAUI.Services
{
    public class UserService
    {
        private readonly BeFitDbContext _context;

        public UserService(BeFitDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetOrCreateUserAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = userId,
                    UserName = "User",
                    Email = "user@example.com"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
    }
}
