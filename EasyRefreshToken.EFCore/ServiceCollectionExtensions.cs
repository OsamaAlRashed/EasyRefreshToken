using EasyRefreshToken.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using EasyRefreshToken.Providers;

namespace EasyRefreshToken.EFCore
{
    /// <summary>
    /// Extension methods to add and configure the token service.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the EF Core refresh token service.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
        /// <typeparam name="TRefreshToken">The type representing the refresh token entity.</typeparam>
        /// <typeparam name="TUser">The type representing a user.</typeparam>
        /// <typeparam name="TKey">The type representing the key of the user entity.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The configuration options for the EF Core token service.</param>
        /// <param name="lifetime">The lifetime of the registered services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddEFCoreRefreshToken<TDbContext, TRefreshToken, TUser, TKey>(
            this IServiceCollection services,
            Action<EFTokenOptions> options = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TRefreshToken : RefreshToken<TUser, TKey>, new()
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            options ??= (option) => { };

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenRepository<TUser, TKey>), typeof(EFTokenRepository<TDbContext, TRefreshToken, TUser, TKey>), lifetime));

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenService<TKey>), typeof(EFTokenService<TUser, TKey>), lifetime));

            return services;
        }

        /// <summary>
        /// Adds and configures the EF Core refresh token service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The configuration options for the EF Core token service.</param>
        /// <param name="lifetime">The lifetime of the registered services.</param>
        /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
        /// <typeparam name="TUser">The type representing a user.</typeparam>
        /// <typeparam name="TKey">The type representing the key of the user entity.</typeparam>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddEFCoreRefreshToken<TDbContext, TUser, TKey>(
            this IServiceCollection services,
            Action<EFTokenOptions> options = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
            => services.AddEFCoreRefreshToken<TDbContext, RefreshToken<TUser, TKey>, TUser, TKey>(options, lifetime);

        /// <summary>
        /// Adds and configures the refresh token service with a custom refresh token type.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
        /// <typeparam name="TRefreshToken">The type representing the custom refresh token entity.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The configuration options for the EF Core token service.</param>
        /// <param name="lifetime">The lifetime of the registered services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddRefreshToken<TDbContext, TRefreshToken>(this IServiceCollection services,
            Action<EFTokenOptions> options = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            where TRefreshToken : RefreshToken<IUser<string>, string>, new()
            => services.AddEFCoreRefreshToken<TDbContext, TRefreshToken, IUser<string>, string>(options, lifetime);

        /// <summary>
        /// Adds and configures the refresh token service with the default refresh token type.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the DbContext.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The configuration options for the EF Core token service.</param>
        /// <param name="lifetime">The lifetime of the registered services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddRefreshToken<TDbContext>(
            this IServiceCollection services,
            Action<EFTokenOptions> options = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
            => services.AddEFCoreRefreshToken<TDbContext, RefreshToken<IUser, string>, IUser, string>(options, lifetime);
    }
}
