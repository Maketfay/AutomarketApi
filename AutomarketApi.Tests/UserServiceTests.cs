using AutomarketApi.Controllers;
using AutomarketApi.Helpers.Interfaces;
using AutomarketApi.Models.Identity;
using AutomarketApi.Repositories.Interfaces;
using AutomarketApi.Services.Implementation;
using AutomarketApi.Services.Interfaces;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace AutomarketApi.Tests
{
    public class UsersServiceTests
    {

        [Fact]
        public async Task Authenticate_ReturnsTokenViewModel_WhenUserFound()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            mock.Setup(repo => repo.ReadAll()).Returns(GetTestUsers());

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(config => config["Jwt:SecurityKey"]).Returns("securityKeyCyber");
            mockConfiguration.SetupGet(config => config["Jwt:Issuer"]).Returns("MyAuthServer");
            mockConfiguration.SetupGet(config => config["Jwt:Audience"]).Returns("MyAuthClient");
            mockConfiguration.SetupGet(config => config["RefreshTokenSettings:ExpirationTimeMin"]).Returns("2880");
            mockConfiguration.SetupGet(config => config["AccessTokenSettings:ExpirationTimeMin"]).Returns("5");

            var mockUserModelHelper = new Mock<IUserModelHelper>();
            var mockRoleRepository = new Mock<IRoleRepository>();
            var mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();

            var service = new UserService(mock.Object, mockConfiguration.Object, mockUserModelHelper.Object, mockRoleRepository.Object, mockRefreshTokenRepository.Object);

            //Act
            var authenticateModel = new AuthenticateRequest { Username = "first", Password = "12345678" };
            var result = await service.Authenticate(authenticateModel);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<TokenViewModel>(result);
            Assert.True(result.RefreshToken != null && result.AccessToken != null);
        }
        private async Task<IQueryable<User>> GetTestUsers()
        {
            var users = new List<User>
            {
                new User{
                    Id = Guid.NewGuid(),
                    FirstName = "first",
                    LastName = "first",
                    Email = "first@gmail.com",
                    Password = "12345678",
                    Patronymic = "first",
                    Username = "first",
                    Role = new Role() {Id = Guid.NewGuid(), Name = "User" }
                },
                new User{
                    Id = Guid.NewGuid(),
                    FirstName = "second",
                    LastName = "second",
                    Email = "second@gmail.com",
                    Password = "12345678",
                    Patronymic = "second",
                    Username = "second",
                    Role = new Role() {Id = Guid.NewGuid(), Name = "User" }
                },
                new User{
                    Id = Guid.NewGuid(),
                    FirstName = "third",
                    LastName = "third",
                    Email = "third@gmail.com",
                    Password = "12345678",
                    Patronymic = "third",
                    Username = "third",
                    Role = new Role() {Id = Guid.NewGuid(), Name = "User" }
                },
                new User{
                    Id = Guid.NewGuid(),
                    FirstName = "fourth",
                    LastName = "fourtht",
                    Email = "fourth@gmail.com",
                    Password = "12345678",
                    Patronymic = "fourth",
                    Username = "fourth",
                    Role = new Role() {Id = Guid.NewGuid(), Name = "User" }
                }
            };
            return users.AsQueryable();
        }

        private async Task<User?> GetTestUserById(Guid id) 
        {
            var result = await GetTestUsers();
            var user = result.FirstOrDefault(usr => usr.Id == id);

            return user;
        }

        [Fact]
        public async Task Authenticate_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.ReadAll()).Returns(GetTestUsers);

            var userService = new UserService(
                mockUserRepository.Object,
                Mock.Of<IConfiguration>(),
                Mock.Of<IUserModelHelper>(),
                Mock.Of<IRoleRepository>(),
                Mock.Of<IRefreshTokenRepository>());

            // Act
            var result = await userService.Authenticate(new AuthenticateRequest { Username = "fifth", Password = "password" });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAll_Returns_ListOfUsers()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.ReadAll()).Returns(GetTestUsers());
            var userService = new UserService(
                mockUserRepository.Object,
                Mock.Of<IConfiguration>(),
                Mock.Of<IUserModelHelper>(),
                Mock.Of<IRoleRepository>(),
                Mock.Of<IRefreshTokenRepository>());

            //Act
            var result = await userService.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count() == (await GetTestUsers()).Count());
        }

        [Fact]
        public async Task GetById_Returns_User_WhenUserNotFound() 
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var testId = (await GetTestUsers()).FirstOrDefault(usr => usr.FirstName == "first").Id;
            mockUserRepository.Setup(repo => repo.Read(testId)).Returns(GetTestUserById(testId));
            var userService = new UserService(
               mockUserRepository.Object,
               Mock.Of<IConfiguration>(),
               Mock.Of<IUserModelHelper>(),
               Mock.Of<IRoleRepository>(),
               Mock.Of<IRefreshTokenRepository>());

            //Act
            var result = await userService.GetById(testId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Register_Returns_Id_And_IsSuccess()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "first",
                LastName = "first",
                Email = "first1@gmail.com",
                Password = "Qq123456",
                Patronymic = "first",
                Username = "first1",
                Role = new Role()
            };
            var userModel = new UserCreateViewModel
            {
                FirstName = "first",
                LastName = "first",
                Email = "first1@gmail.com",
                Password = "Qq123456",
                Patronymic = "first",
                Username = "first1",
                PasswordConfirm = "first"
            };
            mockUserRepository.Setup(repo => repo.Create(user)).ReturnsAsync(user.Id);
            mockUserRepository.Setup(repo => repo.ReadAll()).Returns(GetTestUsers());

            var mockUserModelHelper = new Mock<IUserModelHelper>();
            mockUserModelHelper.Setup(helper => helper.FromViewToEntity(It.IsAny<UserModel>()))
                .Returns(user);

            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(repo => repo.ReadAll())
                .Returns(GetTestRoles());

            var mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();

            var userService = new UserService(
               mockUserRepository.Object,
               Mock.Of<IConfiguration>(),
               mockUserModelHelper.Object,
               mockRoleRepository.Object,
               Mock.Of<IRefreshTokenRepository>());

            //Act
            var result = await userService.Register(userModel);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<Guid>(result);
        }

        private async Task<IQueryable<Role>> GetTestRoles() 
        {
            var roles = new List<Role>
                {
                new Role {Id = Guid.NewGuid(), Name = "User"},
                new Role {Id = Guid.NewGuid(), Name = "Admin"}
                };

            return roles.AsQueryable(); 
        }

        [Fact]
        public async Task Register_IsFailure_WhenUserExist()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "first",
                LastName = "first",
                Email = "first@gmail.com",
                Password = "12345678",
                Patronymic = "first",
                Username = "first",
                Role = new Role()
            };
            var userModel = new UserCreateViewModel
            {
                FirstName = "first",
                LastName = "first",
                Email = "first@gmail.com",
                Password = "Qq123456",
                Patronymic = "first",
                Username = "first",
                PasswordConfirm = "Qq123456"
            };
            mockUserRepository.Setup(repo => repo.Create(user)).ReturnsAsync(user.Id);
            mockUserRepository.Setup(repo => repo.ReadAll()).Returns(GetTestUsers());

            var mockUserModelHelper = new Mock<IUserModelHelper>();
            mockUserModelHelper.Setup(helper => helper.FromViewToEntity(It.IsAny<UserModel>()))
                .Returns(user);

            var mockRoleRepository = new Mock<IRoleRepository>();
            mockRoleRepository.Setup(repo => repo.ReadAll())
                .Returns(GetTestRoles());

            var mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();

            var userService = new UserService(
               mockUserRepository.Object,
               Mock.Of<IConfiguration>(),
               mockUserModelHelper.Object,
               mockRoleRepository.Object,
               Mock.Of<IRefreshTokenRepository>());

            //Act
            var result = await userService.Register(userModel);

            //Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task GenerateToken_Returns_TokenViewModel() 
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "first",
                LastName = "first",
                Email = "first@gmail.com",
                Password = "12345678",
                Patronymic = "first",
                Username = "first",
                Role = new Role() { Id  = Guid.NewGuid(), Name = "User"}
            };

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(config => config["Jwt:SecurityKey"]).Returns("securityKeyCyber");
            mockConfiguration.SetupGet(config => config["Jwt:Issuer"]).Returns("MyAuthServer");
            mockConfiguration.SetupGet(config => config["Jwt:Audience"]).Returns("MyAuthClient");
            mockConfiguration.SetupGet(config => config["RefreshTokenSettings:ExpirationTimeMin"]).Returns("2880");
            mockConfiguration.SetupGet(config => config["AccessTokenSettings:ExpirationTimeMin"]).Returns("5");

            var mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            mockRefreshTokenRepository.Setup(repo=> repo.CreateAsync(new UserRefreshTokenDto { Id = Guid.NewGuid(), Expiration = DateTime.UtcNow.AddMinutes(10), RefreshToken = string.Empty, User = user})).Returns(Task.CompletedTask);

            var userService = new UserService(
                   Mock.Of<IUserRepository>(),
                   mockConfiguration.Object,
                   Mock.Of<IUserModelHelper>(),
                   Mock.Of<IRoleRepository>(),
                   mockRefreshTokenRepository.Object);

            //Act
            var result = await userService.GenerateTokenAsync(user);

            //Assert
            Assert.NotNull(result);
            Assert.True(!result.AccessToken.IsNullOrEmpty() && !result.AccessToken.IsNullOrEmpty());
        }

    }
}