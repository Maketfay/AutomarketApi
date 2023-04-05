using AutomarketApi.Filters.BL;
using AutomarketApi.Models;
using AutomarketApi.Models.Discussions;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Services.Interfaces
{
    public interface IChatService
    {
        Task<Guid> CreateChat(ChatDto chat);
        Task<ChatDto?> ReadAsync(Guid chatId);
        Task<PagedList<ChatDto>> GetPagedList(ChatFilterBL filter);
        Task<Result> SetIsDeletedState(Guid chatId, bool isDeletedState);
    }
}
