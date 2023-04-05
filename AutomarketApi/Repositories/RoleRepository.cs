using AutomarketApi.Context;
using AutomarketApi.Models.Identity;
using AutomarketApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AutomarketApi.Repositories
{
    public class RoleRepository:BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
