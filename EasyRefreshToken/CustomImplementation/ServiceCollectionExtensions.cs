using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshToken
{
    /// <summary>
    /// Extension methods to add and configure the refresh token service.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the custom refresh token service.
        /// </summary>
        /// <typeparam name="TUser">The type representing a user.</typeparam>
        /// <typeparam name="TKey">The type representing the key of the user entity.</typeparam>
        /// <typeparam name="TRepository">The type representing the repository of the user entity.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The configuration options for the custom token service.</param>
        /// <param name="lifetime">The lifetime of the registered services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddCustomRefreshToken<TUser, TKey, TRepository>(
            this IServiceCollection services,
            Action<RefreshTokenOptions> options = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
            where TRepository : class, ITokenRepository<TUser, TKey>
        {
            options ??= (option) => { };

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenRepository<TUser, TKey>), typeof(TRepository), lifetime));

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenService<TKey>), typeof(CustomTokenService<TUser, TKey>), lifetime));

            return services;
        }
    }
}
