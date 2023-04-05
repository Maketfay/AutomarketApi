using AutomarketApi.Helpers.Interfaces;
using AutomarketApi.Models.Identity;


namespace AutomarketApi.Helpers
{
    public class UserModelHelper:IUserModelHelper
    {
        public UserModel FromEntityToView(User entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Patronymic = entity.Patronymic,
                Password = entity.Password,
                Username = entity.Username
            };
        }

        public User FromViewToEntity(UserModel view)
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                FirstName = view.FirstName,
                LastName = view.LastName,
                Email = view.Email,
                Password = view.Password,
                Patronymic = view.Patronymic,
                Username = view.Username,
                Role = new Role()
            };
        }
    }
}
