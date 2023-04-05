using AutomarketApi.Models.Identity;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Repositories.Interfaces
{
    public interface IRefreshTokenRepository 
    {
        Task CreateAsync(UserRefreshTokenDto entity);
        Task<UserRefreshTokenDto?> ReadAsync(string refreshToken);
        void Delete(UserRefreshTokenDto refreshToken);
    }
}
