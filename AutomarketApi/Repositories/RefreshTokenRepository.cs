using AutomarketApi.Context;
using AutomarketApi.Models.Identity;
using AutomarketApi.Repositories.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AutomarketApi.Repositories
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UserRefreshTokenDto entity)
        {
            entity.Id = Guid.NewGuid();
            await _context.RefreshTokens.AddAsync(entity);
        }

        public async Task<UserRefreshTokenDto?> ReadAsync(string refreshToken)
        {
            var userRefreshToken = await _context.RefreshTokens.Include(i => i.User).FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);
            return userRefreshToken;
        }

        public void Delete(UserRefreshTokenDto refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
        }
    }
}
