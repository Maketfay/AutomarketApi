using AutoMapper;
using AutomarketApi.Filters;
using AutomarketApi.Filters.BL;
using AutomarketApi.Models;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Repositories.Interfaces;
using AutomarketApi.Services.Interfaces;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Services.Implementation
{
    public class ChatService:IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMapper mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateChat(ChatDto chat)
        {
            var result = await _chatRepository.Create(_mapper.Map<Chat>(chat));
            return result;
        }

        public async Task<PagedList<ChatDto>> GetPagedList(ChatFilterBL filter)
        {
            var list = await _chatRepository.GetPageListAsync(_mapper.Map<ChatFilter>(filter));
            return _mapper.Map<PagedList<ChatDto>>(list);
        }

        public async Task<ChatDto?> ReadAsync(Guid chatId)
        {
            var chat = await _chatRepository.Read(chatId);
            return _mapper.Map<ChatDto?>(chat);
        }

        public async Task<Result> SetIsDeletedState(Guid chatId, bool isDeletedState)
        {
            var chat = await _chatRepository.Read(chatId);
            if (chat == null)
                return Result.Failure($"Chat with id: {chatId} not found");
            chat.IsDeleted = isDeletedState;
            return Result.Success();
        }
    }
}
