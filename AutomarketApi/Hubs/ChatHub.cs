using AutomarketApi.Services.Interfaces;

namespace AutomarketApi.Hubs
{
    public class ChatHub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
    }
}
