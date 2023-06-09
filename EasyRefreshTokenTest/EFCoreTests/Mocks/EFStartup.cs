using EasyRefreshToken.EFCore;
using EasyRefreshTokenTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshTokenTest.EFCoreTests.Mocks;

public class EFStartup
{
    public static ServiceCollection ConfigureService(Action<EFTokenOptions> options = default)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(op => op.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddEFCoreRefreshToken<AppDbContext, MyRefreshToken, User, Guid>(options);

        return services;
    }
}
