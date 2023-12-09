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

        public Task<string> AddAsync(TKey userId, string token, DateTime? expiredDate)
        {
            if (userId is null)
                throw new ArgumentNullException(nameof(userId));

            if (!_refreshTokens.TryGetValue(userId, out List<InMemoryRefreshTokenModel> values) || values == null)
            {
                values = new List<InMemoryRefreshTokenModel>();
            }

            values.Add(new InMemoryRefreshTokenModel(token, expiredDate));

            _refreshTokens[userId] = values;

            return Task.FromResult(token);
        }

        public Task<bool> DeleteAsync()
        {
            _refreshTokens.Clear();

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(TKey userId)
        {
            var result = _refreshTokens.TryRemove(userId, out _);
            
            return Task.FromResult(result);
        }

        public Task<bool> DeleteAsync(string token)
        {
            foreach (var user in _refreshTokens)
            {
                if(user.Value.Any(x => x.Token == token))
                {
                    _refreshTokens[user.Key] = user.Value.Where(x => x.Token != token).ToList();
                }
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeleteExpiredAsync(TKey userId)
        {
            var values = Get(userId)
                .Where(x => x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow)
                .ToList();

            _refreshTokens[userId] = values;

            return Task.FromResult(true);
        }

        public async Task<bool> DeleteExpiredAsync()
        {
            List<Task<bool>> tasks = new List<Task<bool>>();

            foreach (var user in _refreshTokens)
            {
                tasks.Add(DeleteExpiredAsync(user.Key));
            }

            var results = await Task.WhenAll(tasks);

            return !results.Any(x => x == false);
        }

        public Task<TUser?> GetByIdAsync(TKey userId)
        {
            if (_options.GetUserById == null)
                return Task.FromResult<TUser?>(null);

            var user = _options.GetUserById(_serviceProvider, userId);

            return Task.FromResult(user);
        }

        public Task<int> GetNumberOfActiveTokensAsync(TKey userId)
        {
            var numberOfActiveTokens = Get(userId)
                .Where(x => !x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.UtcNow)
                .Count();

            return Task.FromResult(numberOfActiveTokens);
        }

        public Task<string?> GetOldestTokenAsync(TKey userId)
        {
            var oldestToken = Get(userId)
                .Where(x => x.ExpiredDate.HasValue)
                .OrderBy(x => x.ExpiredDate)
                .Select(x => x.Token)
                .FirstOrDefault();

            return Task.FromResult(oldestToken);
        }

        public Task<bool> IsValidTokenAsync(TKey userId, string token)
        {
            var isValidToken = Get(userId)
                .Where(x => x.Token == token &&
                (!_options.TokenExpiredDays.HasValue || DateTime.UtcNow <= x.ExpiredDate))
                    .Any();

            return Task.FromResult(isValidToken);
        }

        private List<InMemoryRefreshTokenModel> Get(TKey userId)
            => _refreshTokens.GetValueOrDefault(userId) ?? new List<InMemoryRefreshTokenModel>();
    }

    internal record InMemoryRefreshTokenModel(string Token, DateTime? ExpiredDate);
}
