using Demo.Models;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>
                (options =>
                {
                    options.UseSqlServer("Data Source=.;Initial Catalog=EazyRefreshTokenTest;Integrated Security=True; MultipleActiveResultSets=True");
                });
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRefreshToken<AppDbContext, MyRefreshToken, User, Guid>(options =>
{
    options.TokenExpiredDays = 7;
    options.MaxNumberOfActiveDevices = 2;
    options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices = true;
    options.OnChangePasswordBehavior = OnChangePasswordBehavior.DeleteAllTokens;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
