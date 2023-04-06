using AutomarketApi.Helpers;
using AutomarketApi.Helpers.Interfaces;
using AutomarketApi.Models.Identity;
using AutomarketApi.Repositories;
using AutomarketApi.Repositories.Interfaces;
using AutomarketApi.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AutomarketApi.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserModelHelper _userModelHelper;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IUserModelHelper userModelHelper, IRoleRepository roleRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userModelHelper = userModelHelper;
            _roleRepository = roleRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var users = await _userRepository.ReadAll();
            var user = users
               .FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null)
            {
                // todo: need to add logger
                return null;
            }

            var result = await GenerateTokenAsync(user);

            var response = new AuthenticateResponse
            {
                Token = result,
                UserId = user.Id
            };

            return response;
        }

        public async Task<TokenViewModel> GenerateTokenAsync(User user)
        {

            //todo: check that user has value
            var accessToken = GenerateAccessToken(user);

            var refreshToken = GenerateRefreshToken();

            int refreshTokenExpirationTimeInMinutes = int.Parse(_configuration["RefreshTokenSettings:ExpirationTimeMin"]);
            await _refreshTokenRepository.CreateAsync(new UserRefreshTokenDto { User = user, Expiration = DateTime.Now.AddMinutes(refreshTokenExpirationTimeInMinutes), RefreshToken = refreshToken });

            return new TokenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.ReadAll();
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _userRepository.Read(id);
        }

        public async Task<Result<Guid>> Register(UserCreateViewModel userModel)
        {
            //todo validate user
            var valRes = await ValidateUser(userModel);

            if (valRes.IsFailure) 
            {
                return Result.Failure<Guid>(valRes.ToString());
            }
            var user = new User
            {
                Email = userModel.Email,
                Username = userModel.Username,
                LastName = userModel.LastName,
                Patronymic = userModel.Patronymic,
                Password = userModel.Password,
                Role = new Role(),
                FirstName = userModel.FirstName,
                Chats = new(),
                Id = Guid.NewGuid(),
            };

            var res = await AddUserToRole(user, "User");

            //todo hash password
            var addedUserId  = await _userRepository.Create(user);

            return Result.Success(addedUserId);
        }

        public async Task<Result> ValidateUser(UserCreateViewModel model) 
        {
            var validator = new UserCreateViewModelValidator();
            var res = validator.Validate(model);

            if (!res.IsValid) { return Result.Failure("Validation error"); }

            var valid = (await _userRepository.ReadAll()).FirstOrDefault(usr => (usr.Username == model.Username) || (usr.Email == model.Email));

            if (valid != null) { return Result.Failure("User already exist"); }

            return Result.Success();
        }
        private string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role.Name)
            };

            //Add token settings to work with config.

            int accessTokenExpirationTimeInMinutes = int.Parse(_configuration["AccessTokenSettings:ExpirationTimeMin"]);

            var accessToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpirationTimeInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<Boolean> AddUserToRole(User user, string roleName)
        {
            var roles = await _roleRepository.ReadAll();

            var role = roles.FirstOrDefault(x => x.Name == roleName);

            if (role == null)
            {
                return false;
            }

            user.Role = role;
            
            return true;
        }

        public async Task<Result<TokenViewModel>> RefreshTokenAsync(TokenViewModel viewModel)
        {
            var refreshTokenResult = await ValidateRefreshToken(viewModel);

            if (refreshTokenResult.IsFailure)
                return Result.Failure<TokenViewModel>(refreshTokenResult.Error);

            var refreshedToken = await Refresh(refreshTokenResult.Value);

            return Result.Success(refreshedToken);
        }

        private async Task<Result<UserRefreshTokenDto>>? ValidateRefreshToken(TokenViewModel viewModel)
        {
            var refreshToken = await _refreshTokenRepository.ReadAsync(viewModel.RefreshToken);

            if (refreshToken == null)
                return Result.Failure<UserRefreshTokenDto>("Wrong refresh token");

            var userIdResult = GetIdClaim(viewModel.AccessToken, JwtRegisteredClaimNames.Sid);

            if (userIdResult.IsFailure)
                return Result.Failure<UserRefreshTokenDto>("User id in claim failure");

            if (userIdResult.Value != refreshToken.User.Id)
                return Result.Failure<UserRefreshTokenDto>("Your aren't owner of the refresh token");

            return Result.Success(refreshToken);
        }

        private Result<Guid> GetIdClaim(string jwtToken, string claimName)
        {
            var jwtSecurityToken = ParseJwt(jwtToken);

            var claim = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == claimName);
            if (claim == null)
            {
                return Result.Failure<Guid>($"{claimName} claim missed.");
            }

            if (!Guid.TryParse(claim.Value, out Guid userId))
            {
                return Result.Failure<Guid>($"Wrong userId value.");
            }
            return Result.Success(userId);
        }

        private JwtSecurityToken ParseJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken;

            jwtSecurityToken = handler.ReadJwtToken(jwtToken);

            return jwtSecurityToken;
        }

        private async Task<TokenViewModel> Refresh(UserRefreshTokenDto refreshToken)
        {
            var isRefreshTokenExpired = await RefreshTokenIsExpired(refreshToken);
            if (isRefreshTokenExpired)
            {
                var token = await GenerateTokenAsync(refreshToken.User);
                return token;
            }
            else
            {
                var accessToken = GenerateAccessToken(refreshToken.User);
                return new TokenViewModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.RefreshToken
                };
            }
        }

        private async Task<bool> RefreshTokenIsExpired(UserRefreshTokenDto refreshToken)
        {
            int beforeMinutes = int.Parse(_configuration["RefreshTokenSettings:TimeToCheckBeforeRefreshTokenExpiredMin"]);
            var isRefreshTokenExpired = DateTime.Now.AddMinutes(beforeMinutes) >= refreshToken.Expiration;
            return isRefreshTokenExpired;
        }

        public async Task<Result> LogoutAsync(TokenViewModel tokenDto)
        {
            var userRefreshToken = await _refreshTokenRepository.ReadAsync(tokenDto.RefreshToken);
            if (userRefreshToken == null)
                return Result.Failure("Refresh token isn't exist!");

            var userIdResult = GetIdClaim(tokenDto.AccessToken, JwtRegisteredClaimNames.Sid);
            if (userIdResult.IsFailure)
                return Result.Failure(userIdResult.Error);

            if (userIdResult.Value != userRefreshToken.User.Id)
                return Result.Failure("Your aren't owner of the refresh token");

            _refreshTokenRepository.Delete(userRefreshToken);
            return Result.Success();
        }
    }
}
