using AutoMapper;
using AutomarketApi.Configuration.MapperConfiguration;

namespace AutomarketApi.Configuration
{
    public class AppMappingProfile: Profile
    {
        public AppMappingProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            UserMapConfig.CreateMap(this);
            ChatMapConfig.CreateMap(this);
        }
    }
}
