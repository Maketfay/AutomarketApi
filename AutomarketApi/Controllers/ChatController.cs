using AutomarketApi.Filters.BL;
using AutomarketApi.Models.Car;
using AutomarketApi.Models.Discussions;
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

        [Authorize]
        [HttpGet("getMessages")]
        public async Task<IActionResult> GetMessages(Guid chatId)
        {
            var chat = await _chatService.ReadAsync(chatId);
            if (chat == null)
                return BadRequest();
            var viewModel = new List<MessageViewModel>();
            foreach (var message in chat.Messages.OrderBy(message => message.SentDate))
            {
                viewModel.Add(new MessageViewModel
                {
                    Username = message.Sender.Username,
                    Message = message.MessageText,
                    SentDate = message.SentDate.ToString("f")
                });
            }
            return Ok(viewModel);
        }
    }
}
