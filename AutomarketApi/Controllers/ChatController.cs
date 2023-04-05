using AutomarketApi.Filters.BL;
using AutomarketApi.Models.Car;
using AutomarketApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AutomarketApi.Controllers
{
    [ApiController]
    [Route("/chats")]
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatController(IUnitOfWork uinOfWork, IChatService chatService, IMessageService messageService) : base(uinOfWork)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        [Authorize]
        [HttpGet("getChatList")]
        public async Task<IActionResult> GetChatList(int page)
        {
            var userId = Guid.Parse(User.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sid).Value);

            var filter = new ChatFilterBL
            {
                CurrentPage = page,
                PageSize = 5,
                UserId = userId,
            };
            var chatsResult = await _chatService.GetPagedList(filter);

            return Ok();
           // return Ok(new { PageSize = chatsResult.PageSize, TotalItemsCount = chatsResult.TotalItemsCount, Items = viewModel });
        }
    }
}
