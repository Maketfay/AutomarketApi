using AutomarketApi.Models.Identity;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<TokenViewModel> Authenticate(AuthenticateRequest model);

        Task<TokenViewModel> GenerateTokenAsync(User user);
        Task<Result<Guid>> Register(UserCreateViewModel userModel);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(Guid id);
        Task<Result> LogoutAsync(TokenViewModel tokenDto);
        Task<Result<TokenViewModel>> RefreshTokenAsync(TokenViewModel viewModel);
    }
}
