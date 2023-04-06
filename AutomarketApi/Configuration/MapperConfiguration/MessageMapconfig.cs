using AutomarketApi.Models.Discussions;
using AutomarketApi.Models;

namespace AutomarketApi.Configuration.MapperConfiguration
{
    internal static class MessageMapconfig
    {
        public static void CreateMap(AppMappingProfile profile)
        {
            profile.CreateMap<Message, MessageDto>().ReverseMap();
            profile.CreateMap<MessageDto?, Message?>().ReverseMap();
        }
    }
}
