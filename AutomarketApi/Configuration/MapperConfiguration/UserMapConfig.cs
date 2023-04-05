using AutomarketApi.Models.Identity;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Configuration.MapperConfiguration
{
    internal static class UserMapConfig
    {
        public static void CreateMap(AppMappingProfile profile)
        {
            profile.CreateMap<User, UserModel>().ReverseMap();
            profile.CreateMap<User?, UserModel?>();
        }
    }
}

