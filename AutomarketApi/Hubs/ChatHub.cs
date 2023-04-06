using AutomarketApi.Connection;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AutomarketApi.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDictionary<string, ChatConnection> _connections;

        public ChatHub(IMessageService messageService, IUnitOfWork unitOfWork, IDictionary<string, ChatConnection> connection)
        {
            _messageService = messageService;
            _unitOfWork = unitOfWork;
            _connections = connection;
        }

        public async Task SendMessage(MessageRequestModel viewModel)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out ChatConnection connection))
            {
                //await _messageService.CreateAsync(viewModel);
                await Clients.Group(connection.ChatId.ToString()).SendAsync("ReceiveMessage", $"{connection.User.Username}", viewModel.Message);
                //await _unitOfWork.CommitAsync();
            }
        }

        public async Task JoinChat(ChatConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatId.ToString());
            _connections[Context.ConnectionId] = connection;
            await Clients.Group(connection.ChatId.ToString()).SendAsync("ReceiveMessage", "Bot",  "U join)");
        }

    }
}
