using EasyRefreshToken.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshToken.InMemoryCache
{
    /// <summary>
    /// Extension methods to add and configure the refresh token service.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the in-memory cache refresh token service.
        /// </summary>
        /// <typeparam name="TUser">The type representing a user.</typeparam>
        /// <typeparam name="TKey">The type representing the key of the user entity.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The configuration options for the in-memory token service.</param>
        /// <param name="lifetime">The lifetime of the registered services.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddInMemoryCacheRefreshToken<TUser, TKey>(
            this IServiceCollection services,
            Action<InMemoryTokenOptions<TUser, TKey>> options = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TUser : class, IUser<TKey>
            where TKey : IEquatable<TKey>
        {
            options ??= (option) => { };

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenRepository<TUser, TKey>), typeof(InMemoryTokenRepository<TUser, TKey>), lifetime));

            services.Configure(options)
                .Add(new ServiceDescriptor(typeof(ITokenService<TKey>), typeof(InMemoryTokenService<TUser, TKey>), lifetime));

            return services;
        }
    }
}
