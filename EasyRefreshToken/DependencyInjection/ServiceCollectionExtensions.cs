using EasyRefreshToken.Models;
using EasyRefreshToken.TokenService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRefreshToken.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add refresh token service to your project
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns>The same service collection</returns>
        public static IServiceCollection AddRefreshToken<TDbContext, TUser, TKey>(this IServiceCollection services, Action<TokenOptions> options = default) 
            where TDbContext : DbContext, IDbSetRefreshToken<TUser, TKey>
            => services.Configure(options).AddScoped<ITokenService, TokenService<TDbContext, TUser, TKey>>();
    }
}
