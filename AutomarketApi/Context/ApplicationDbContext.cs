using AutomarketApi.Context;
using AutomarketApi.Models.Car;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AutomarketApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public DbSet<Car> Car { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshTokenDto> RefreshTokens { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRefreshTokenDto>().HasIndex(urf => urf.RefreshToken).IsUnique();

            modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r=>r.Users);

            //modelBuilder.Entity<Chat>().HasOne(c => c.UserToJoin).WithMany().OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Message>().HasOne(c => c.Sender).WithMany().OnDelete(DeleteBehavior.NoAction);


            //modelBuilder.Entity<Role>().HasMany(u => u.Users).WithOne(r=>r.Role);

            //modelBuilder.Entity<Car>().HasData(
            //        new Car { Id = Guid.NewGuid(), Name = "BMW X5", Description = "dfskm", Model = "BMW", TypeCar = Models.Enum.TypeCar.HatchBack, DateCreate = DateTime.Now, Price = 150000, Speed = 120, Color = "Red" }
            //);

            //var role = new Role { Id = Guid.NewGuid(), Name = "Admin" };
            //modelBuilder.Entity<Role>().HasData(role);

            //var roleUser = new Role { Id = Guid.NewGuid(), Name = "User" };
            //modelBuilder.Entity<Role>().HasData(roleUser);

            //var user = new User { Id = Guid.NewGuid(), FirstName = "Ivan", LastName = "Ivanov", Patronymic = "Ivanovich", Email = "IvanovIvan@gmail.com", Username = "TesrUser123", Password = "12345678" };
            ////user.RoleId = role.Id;
            //user.Role = role;



            //modelBuilder.Entity<User>().HasData(user);
        }
    }
}

