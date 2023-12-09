using EasyRefreshToken.Abstractions;
using EasyRefreshToken.Commons;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Enums;
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
        private readonly object _lock = new object();
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
        public virtual async Task<bool> ClearAsync() => await _tokenRepository.DeleteAsync();

        /// <inheritdoc/>
        public virtual async Task<bool> ClearAsync(TKey userId) => await _tokenRepository.DeleteAsync(userId);

        /// <inheritdoc/>
        public virtual async Task<bool> ClearExpiredAsync() => await _tokenRepository.DeleteExpiredAsync();

        /// <inheritdoc/>
        public virtual async Task<bool> ClearExpiredAsync(TKey userId) => await _tokenRepository.DeleteExpiredAsync(userId);

        /// <inheritdoc/>
        public virtual async Task<bool> OnLogoutAsync(string token) => await _tokenRepository.DeleteAsync(token);

        /// <inheritdoc/>
        public virtual async Task<TokenResult> OnAccessTokenExpiredAsync(TKey userId, string oldToken, bool renewTheToken)
        {
            var isValidToken = await _tokenRepository.IsValidTokenAsync(userId, oldToken);

            if (!isValidToken)
                return TokenResult.SetFailed($"{oldToken} not valid", 400);

            await _tokenRepository.DeleteAsync(oldToken);

            var token = await _tokenRepository.AddAsync(
                userId,
                renewTheToken ? oldToken : _options.TokenGenerationMethod(),
                GetExpiredDate());

            return TokenResult.SetSuccess(token);
        }

        /// <inheritdoc/>
        public virtual async Task<string> OnChangePasswordAsync(TKey userId)
        {
            await _tokenRepository.DeleteAsync(userId);

            if (_options.OnChangePasswordBehavior == OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken)
                return await _tokenRepository.AddAsync(userId, _options.TokenGenerationMethod(), GetExpiredDate());

            return null;
        }

        /// <inheritdoc/>
        public virtual async Task<TokenResult> OnLoginAsync(TKey userId)
        {
            lock (_lock)
            {
                if (_blackList.Contains(userId))
                    return TokenResult.SetFailed($"User with id {userId} is blocked.", 401);
            }

            TUser user = await _tokenRepository.GetByIdAsync(userId);

            if (user is null && _options.MaxNumberOfActiveDevices.Type != MaxNumberOfActiveDevicesType.GlobalLimit)
                return TokenResult.SetFailed($"User with id {userId} not found.", 404);

            int maxNumberOfActiveDevicesPerUser = _options.MaxNumberOfActiveDevices.GlobalLimit;

            if (_options.MaxNumberOfActiveDevices.Type != MaxNumberOfActiveDevicesType.GlobalLimit)
            {
                maxNumberOfActiveDevicesPerUser = GetMaxNumberOfActiveDevicesPerUser(user);
            }

            if (await _tokenRepository.GetNumberOfActiveTokensAsync(userId) >= maxNumberOfActiveDevicesPerUser)
            {
                var oldedToken = await _tokenRepository.GetOldestTokenAsync(userId);
                if (_options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices || oldedToken == null)
                    return TokenResult.SetFailed("Login not allowed because access to max number of active devices.", 401);

                await _tokenRepository.DeleteAsync(oldedToken);
            }

            var newToken = await _tokenRepository.AddAsync(
                userId,
                _options.TokenGenerationMethod(),
                GetExpiredDate());

            return TokenResult.SetSuccess(newToken);
        }

        /// <inheritdoc/>
        public async Task<bool> BlockAsync(TKey userId)
        {
            lock (_lock)
            {
                if (_blackList.Contains(userId))
                    return false;

                _blackList.Add(userId);
                return true;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UnblockAsync(TKey userId)
        {
            lock (_lock)
            {
                if (!_blackList.Contains(userId))
                    return false;

                _blackList.Remove(userId);
                return true;
            }
        }

        #endregion

        #region Private
        private int GetMaxNumberOfActiveDevicesPerUser(TUser user)
        {
            if(_options.MaxNumberOfActiveDevices.Type == MaxNumberOfActiveDevicesType.LimitPerType &&
                _options.MaxNumberOfActiveDevices.LimitPerType.ContainsKey(user.GetType()))
            {
                return _options.MaxNumberOfActiveDevices.LimitPerType[user.GetType()];
            }

            if(_options.MaxNumberOfActiveDevices.Type == MaxNumberOfActiveDevicesType.LimitPerProperty)
            {
                var propertyName = _options.MaxNumberOfActiveDevices.LimitPerProperty.PropertyName;
                var propValue = Helpers.GetPropertyValue(user, propertyName);
                if (propValue is not null && _options.MaxNumberOfActiveDevices.LimitPerProperty.ValuePerLimit.ContainsKey(propValue))
                    return _options.MaxNumberOfActiveDevices.LimitPerProperty.Item2[propValue];
            }

            return _options.MaxNumberOfActiveDevices.GlobalLimit;
        }

        private DateTime? GetExpiredDate()
            => _options.TokenExpiredDays.HasValue ? DateTime.UtcNow.AddDays(_options.TokenExpiredDays.Value) : null;
        #endregion
    }
}
