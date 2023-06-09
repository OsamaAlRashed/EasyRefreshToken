using EasyRefreshToken.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRefreshToken.InMemory
{

    internal class InMemoryTokenRepository<TUser, TKey> : ITokenRepository<TUser, TKey>
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly InMemoryTokenOptions<TUser, TKey> _options;
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<TKey, List<InMemoryRefreshTokenModel>> _refreshTokens = new();

        public InMemoryTokenRepository(
            IServiceProvider serviceProvider,
            IOptions<InMemoryTokenOptions<TUser, TKey>> options)
        {
            _serviceProvider = serviceProvider;
            _options = options?.Value ?? new InMemoryTokenOptions<TUser, TKey>();
        }

        public async Task<string> Add(TKey userId, string token, DateTime? expiredDate)
        {
            if (userId is null)
                throw new ArgumentNullException(nameof(userId));

            if (!_refreshTokens.TryGetValue(userId, out List<InMemoryRefreshTokenModel> values) || values == null)
            {
                values = new List<InMemoryRefreshTokenModel>();
            }

            values.Add(new InMemoryRefreshTokenModel(token, expiredDate));

            _refreshTokens[userId] = values;

            return token;
        }

        public async Task<bool> Delete()
        {
            _refreshTokens.Clear();

            return true;
        }

        public async Task<bool> Delete(TKey userId) 
            => _refreshTokens.TryRemove(userId, out _);

        public async Task<bool> Delete(string token)
        {
            foreach (var user in _refreshTokens)
            {
                if(user.Value.Any(x => x.Token == token))
                {
                    _refreshTokens[user.Key] = user.Value.Where(x => x.Token != token).ToList();
                }
            }

            return true;
        }

        public async Task<bool> DeleteExpired(TKey userId)
        {
            var values = Get(userId).Where(x => x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow).ToList();

            _refreshTokens[userId] = values;

            return true;
        }

        public async Task<bool> DeleteExpired()
        {
            foreach (var user in _refreshTokens)
            {
                await DeleteExpired(user.Key);
            }

            return true;
        }

        public async Task<TUser?> GetById(TKey userId)
        {
            if (_options.GetUserById == null)
                return null;

            return _options.GetUserById(_serviceProvider, userId);
        }

        public async Task<int> GetNumberActiveTokens(TKey userId)
            => Get(userId).Where(x => (!x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.UtcNow))
              .Count();

        public async Task<string?> GetOldestToken(TKey userId)
            => Get(userId).Where(x => x.ExpiredDate.HasValue)
                .OrderBy(x => x.ExpiredDate).Select(x => x.Token).FirstOrDefault();

        public async Task<bool> IsValidToken(TKey userId, string token)
        {
            return Get(userId).Where(x => x.Token == token &&
                (!_options.TokenExpiredDays.HasValue || DateTime.UtcNow <= x.ExpiredDate))
                    .Any();
        }

        private List<InMemoryRefreshTokenModel> Get(TKey userId)
            => _refreshTokens.GetValueOrDefault(userId) ?? new List<InMemoryRefreshTokenModel>();
    }

    internal record InMemoryRefreshTokenModel(string Token, DateTime? ExpiredDate);
}
