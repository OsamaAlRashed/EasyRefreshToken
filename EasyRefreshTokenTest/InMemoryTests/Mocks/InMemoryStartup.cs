using EasyRefreshToken.InMemory;
using EasyRefreshTokenTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshTokenTest.InMemoryTests.Mocks;

public class InMemoryStartup
{
    public static ServiceCollection ConfigureService(Action<InMemoryTokenOptions<User, Guid>> options = default)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(op => op.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddInMemoryRefreshToken(options);

        return services;
    }
}
