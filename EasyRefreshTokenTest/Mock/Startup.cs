using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Utils;
using EasyRefreshTokenTest.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EasyRefreshTokenTest.Mock
{

    public class Startup
    {
        public static ServiceCollection ConfigureService(Action<RefreshTokenOptions> options = default)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(op => op.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            if(options != null)
                services.AddRefreshToken<AppDbContext, MyRefreshToken, User, Guid>(options);
            else 
                services.AddRefreshToken<AppDbContext, MyRefreshToken, User, Guid>();

            return services;
        }
    }
}
