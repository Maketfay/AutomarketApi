using AutomarketApi.Models.Identity;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Repositories.Interfaces
{
    public interface IUserRepository:IBaseRepository<User>
    {
        public Task<User?> Read(string username);
    }
}
