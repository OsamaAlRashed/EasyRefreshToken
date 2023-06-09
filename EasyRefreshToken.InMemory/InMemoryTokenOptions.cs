using EasyRefreshToken.DependencyInjection;
using System;

namespace EasyRefreshToken.InMemory
{
    /// <summary>
    /// Options to control the behavior of the in-memory token service.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TKey">The type representing the key of the user entity.</typeparam>
    public class InMemoryTokenOptions<TUser, TKey> : RefreshTokenOptions
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets a function to retrieve a user by their ID.
        /// </summary>
        public Func<IServiceProvider, TKey, TUser>? GetUserById { get; set; }
    }
}
