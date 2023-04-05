using AutomarketApi.Models.Discussions;
using AutomarketApi.Models;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Configuration.MapperConfiguration
{
    internal static class ChatMapConfig
    {
        public static void CreateMap(AppMappingProfile profile) 
        {
            profile.CreateMap<Chat, ChatDto>().ReverseMap();
            profile.CreateMap<ChatDto?, Chat?>().ReverseMap();  
            profile.CreateMap<PagedList<ChatDto>, PagedList<Chat>>().ReverseMap();
        }
    }
}
