using AutomarketApi;
using AutomarketApi.Configuration;
using AutomarketApi.Context;
using AutomarketApi.Helpers;
using AutomarketApi.Helpers.Interfaces;
using AutomarketApi.Repositories;
using AutomarketApi.Repositories.Interfaces;
using AutomarketApi.Services.Implementation;
using AutomarketApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var env = builder.Environment;
services.AddControllers();


services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddCors();


services.AddScoped<IUnitOfWork,UnitOfWork>();
services.AddScoped<ICarRepository, CarRepository>();
services.AddScoped<ICarViewHelper, CarViewHelper>();
services.AddScoped<ICarService, CarService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserModelHelper, UserModelHelper>();
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddScoped<IChatRepository, ChatRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IChatService, ChatService>();
//services.AddScoped<IMessageService, MessageService>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

services.AddAutoMapper(cfg =>
{
    cfg.AllowNullCollections = true;
    cfg.AllowNullDestinationValues = true;
}, typeof(AppMappingProfile));

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"]))
    };
});

services.AddAuthorization();


services.AddSwaggerGen();

services.AddEndpointsApiExplorer();

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();