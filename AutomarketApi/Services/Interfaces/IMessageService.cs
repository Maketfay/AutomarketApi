using AutomarketApi.Filters.BL;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Models;

namespace AutomarketApi.Services.Interfaces
{
    public interface IMessageService
    {
        Task CreateAsync(MessageRequestModel message);
        Task<PagedList<MessageDto>> GetPagedList(MessageFilterBL filter);
    }
}
