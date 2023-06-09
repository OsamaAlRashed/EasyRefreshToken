using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace EasyRefreshToken.InMemoryCache
{
    internal class InMemoryTokenService<TUser, TKey> : BaseTokenService<TUser, TKey>
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public InMemoryTokenService(
            ITokenRepository<TUser, TKey> repository,
            IOptions<InMemoryTokenOptions<TUser, TKey>> options) : base(repository, MapOptions(options)) { }

        private static RefreshTokenOptions MapOptions(IOptions<InMemoryTokenOptions<TUser, TKey>> efOptions)
        {
            var options = efOptions?.Value ?? new InMemoryTokenOptions<TUser, TKey>();

            return options;
        }
    }
}
