using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace EasyRefreshToken.EFCore
{
    internal class EFTokenService<TUser, TKey> : BaseTokenService<TUser, TKey>
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {

        public EFTokenService(
            ITokenRepository<TUser, TKey> repository,
            IOptions<EFTokenOptions> options)
            : base(repository, MapOptions(options)) { }

        private static RefreshTokenOptions MapOptions(IOptions<EFTokenOptions> efOptions)
        {
            var options = efOptions?.Value ?? new EFTokenOptions();

            return options;
        }
    }
}
