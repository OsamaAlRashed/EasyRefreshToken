using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using System;

namespace EasyRefreshToken
{
    internal class CustomTokenService<TUser, TKey> : BaseTokenService<TUser, TKey>
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public CustomTokenService(
            ITokenRepository<TUser, TKey> repository,
            RefreshTokenOptions options) : base(repository, options) { }
    }
}
