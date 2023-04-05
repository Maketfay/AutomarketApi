using AutomarketApi.Context;
using AutomarketApi.Models.Identity;
using AutomarketApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutomarketApi.Repositories
{
    public class UserRepository:BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> Read(string username)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return entity;
        }
    }
}
