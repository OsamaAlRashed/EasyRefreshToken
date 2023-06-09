using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.DependencyInjection.Enums;
using EasyRefreshToken.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyRefreshToken
{
    /// <inheritdoc/>
    public abstract class BaseTokenService<TUser, TKey> : ITokenService<TKey>
        where TKey : IEquatable<TKey>
        where TUser : class, IUser<TKey>
    {
        private readonly RefreshTokenOptions _options;
        private readonly HashSet<TKey> _blackList = new();
        private readonly ITokenRepository<TUser, TKey> _tokenRepository;

        /// <inheritdoc/>
        public BaseTokenService(
            ITokenRepository<TUser, TKey> tokenRepository,
            RefreshTokenOptions options)
        {
            _options = options ?? new RefreshTokenOptions();
            _tokenRepository = tokenRepository;
        }

        #region Service

        /// <inheritdoc/>
        public virtual async Task<bool> ClearAsync() => await _tokenRepository.Delete();

        /// <inheritdoc/>
        public virtual async Task<bool> ClearAsync(TKey userId) => await _tokenRepository.Delete(userId);

        /// <inheritdoc/>
        public virtual async Task<bool> ClearExpiredAsync() => await _tokenRepository.DeleteExpired();

        /// <inheritdoc/>
        public virtual async Task<bool> ClearExpiredAsync(TKey userId) => await _tokenRepository.DeleteExpired(userId);

        /// <inheritdoc/>
        public virtual async Task<bool> OnLogoutAsync(string token) => await _tokenRepository.Delete(token);

        /// <inheritdoc/>
        public virtual async Task<TokenResult> OnAccessTokenExpiredAsync(TKey userId, string oldToken)
        {
            var isValidToken = await _tokenRepository.IsValidToken(userId, oldToken);

            if (isValidToken)
            {
                await _tokenRepository.Delete(oldToken);
                return TokenResult.SetSuccess(
                    await _tokenRepository.Add(userId,
                              _options.TokenGenerationMethod(),
                              GetExpiredDate()));
            }

            return TokenResult.SetFailed($"{oldToken} not found", 404);
        }

        /// <inheritdoc/>
        public virtual async Task<string> OnChangePasswordAsync(TKey userId)
        {
            await _tokenRepository.Delete(userId);

            if (_options.OnChangePasswordBehavior == OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken)
                return await _tokenRepository.Add(userId, _options.TokenGenerationMethod(), GetExpiredDate());

            return null;
        }

        /// <inheritdoc/>
        public virtual async Task<TokenResult> OnLoginAsync(TKey userId)
        {
            TUser user = await _tokenRepository.GetById(userId);
            if (user is null && _options.MaxNumberOfActiveDevices.Type != MaxNumberOfActiveDevicesType.GlobalLimit)
            {
                return TokenResult.SetFailed($"User with id {userId} not found.", 404);
            }

            if (await IsAccessToLimit(user, userId))
            {
                var oldedToken = await _tokenRepository.GetOldestToken(userId);
                if (_options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices || oldedToken == null)
                    return TokenResult.SetFailed("Login not allowed because access to max number of active devices.", 401);

                await _tokenRepository.Delete(oldedToken);
            }

            return TokenResult.SetSuccess(
                await _tokenRepository.Add(
                    userId,
                    _options.TokenGenerationMethod(),
                    GetExpiredDate()));
        }
        #endregion

        #region Private
        private async Task<bool> IsAccessToLimit(TUser user, TKey userId)
        {
            var limit = GetMaxNumberOfActiveDevicesPerUser(user);
            return await _tokenRepository.GetNumberActiveTokens(userId) >= limit;
        }

        private int GetMaxNumberOfActiveDevicesPerUser(TUser user)
        {
            if(user is null)
            {
                return _options.MaxNumberOfActiveDevices.GlobalLimit;
            }

            switch (_options.MaxNumberOfActiveDevices.Type)
            {
                case MaxNumberOfActiveDevicesType.GlobalLimit:
                    return _options.MaxNumberOfActiveDevices.GlobalLimit;

                case MaxNumberOfActiveDevicesType.LimitPerType:
                    if (_options.MaxNumberOfActiveDevices.LimitPerType.TryGetValue(user.GetType(), out int value))
                        return value;
                    break;

                case MaxNumberOfActiveDevicesType.LimitPerProperty:
                    var propName = _options.MaxNumberOfActiveDevices.LimitPerProperty.Item1;
                    var propValue = Helpers.GetPropertyValue(user, propName);
                    if (propValue == null || !_options.MaxNumberOfActiveDevices.LimitPerProperty.Item2.ContainsKey(propValue))
                        return _options.MaxNumberOfActiveDevices.GlobalLimit;

                    return _options.MaxNumberOfActiveDevices.LimitPerProperty.Item2[propValue];
            }

            return _options.MaxNumberOfActiveDevices.GlobalLimit;
        }

        private DateTime? GetExpiredDate()
            => _options.TokenExpiredDays.HasValue ? DateTime.UtcNow.AddDays(_options.TokenExpiredDays.Value) : null;

        #endregion
    }
}
