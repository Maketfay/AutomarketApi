using AutomarketApi.Helpers;
using AutomarketApi.Models.Identity;
using AutomarketApi.Services.Interfaces;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomarketApi.Controllers
{
    [ApiController]
    [Route("/Users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IUnitOfWork unitOfWork): base(unitOfWork) 
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return await ReturnSuccess(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateViewModel userModel)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new { message = "Invalid model" });
            }

            var userId = await _userService.Register(userModel);

            if (userId.IsFailure)
            {
                return BadRequest(new { message = "Didn't register!" });
            }

            return Ok(userId.Value);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenViewModel tokenDto)
        {
            var res = await _userService.RefreshTokenAsync(tokenDto);

            if (res.IsFailure)
                return BadRequest(res.Error);

            return await ReturnSuccess(res.Value);
        }

        
        [HttpGet("getUsers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = (await _userService.GetAll()).ToList();
            return Ok(users);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] TokenViewModel tokenViewModel)
        {
            var res = await _userService.LogoutAsync(tokenViewModel);

            if (res.IsFailure)
            {
                return BadRequest(res);
            }

            return await ReturnSuccess();
        }
    }
}
