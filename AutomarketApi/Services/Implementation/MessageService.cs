using AutoMapper;
using AutomarketApi.Filters;
using AutomarketApi.Filters.BL;
using AutomarketApi.Models;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Repositories.Interfaces;
using AutomarketApi.Services.Interfaces;

namespace AutomarketApi.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(MessageRequestModel message)
        {
            var chat = await _chatRepository.Read(message.ChatId);
            var user = await _userRepository.Read(message.SenderUsername);
            var newMessage = new Message
            {
                Chat = chat,
                MessageText = message.Message,
                SentDate = DateTime.UtcNow,
                Sender = user
            };

            await _messageRepository.Create(newMessage);
        }

        public async Task<PagedList<MessageDto>> GetPagedList(MessageFilterBL filter)
        {
            var list = await _messageRepository.GetPageListAsync(_mapper.Map<MessageFilter>(filter));
            return _mapper.Map<PagedList<MessageDto>>(list);
        }
    }
}
