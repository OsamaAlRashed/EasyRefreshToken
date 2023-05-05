using EasyRefreshToken.Abstractions;
using EasyRefreshToken.Models;
using EasyRefreshToken.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Extensions methods to add token service
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures refresh token service
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TRefreshToken"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="lifetime"></param>
        /// <returns>The same service collection</returns>
        public static IServiceCollection AddRefreshToken<TDbContext, TRefreshToken, TUser, TKey>(
            this IServiceCollection services, 
            Action<RefreshTokenOptions> options = default,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TRefreshToken : RefreshToken<TUser, TKey>, new()
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            options ??= (option) => { };

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenService<TKey>), typeof(TokenService<TDbContext, TRefreshToken, TUser, TKey>), lifetime));
               
            return services;
        }

        /// <summary>
        /// Adds and configures refresh token service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="lifetime"></param>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns>The same service collection</returns>
        public static IServiceCollection AddRefreshToken<TDbContext, TUser, TKey>(
            this IServiceCollection services,
            Action<RefreshTokenOptions> options = default,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
            => services.AddRefreshToken<TDbContext, RefreshToken<TUser, TKey>, TUser, TKey>(options, lifetime);

        /// <summary>
        /// Adds and configures refresh token service
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TRefreshToken"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="lifetime"></param>
        /// <returns>The same service collection</returns>
        public static IServiceCollection AddRefreshToken<TDbContext, TRefreshToken>(this IServiceCollection services,
            Action<RefreshTokenOptions> options = default,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TRefreshToken : RefreshToken<IUser<string>, string>, new()
            => services.AddRefreshToken<TDbContext, TRefreshToken, IUser<string>, string>(options, lifetime);

        /// <summary>
        /// Adds and configures refresh token service
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="lifetime"></param>
        /// <param name="options"></param>
        /// <returns>The same service collection</returns>
        public static IServiceCollection AddRefreshToken<TDbContext>(
            this IServiceCollection services,
            Action<RefreshTokenOptions> options = default,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            => services.AddRefreshToken<TDbContext, RefreshToken<IUser, string>, IUser, string>(options, lifetime);
    }
}