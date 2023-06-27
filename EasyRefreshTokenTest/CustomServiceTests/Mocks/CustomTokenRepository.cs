using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Tests.Mocks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRefreshToken.Tests.CustomServiceTests.Mocks;

internal record InMemoryRefreshTokenModel(string Token, DateTime? ExpiredDate);

public class CustomTokenRepository : ITokenRepository<User, Guid>
{
    private readonly RefreshTokenOptions _options;
    private ConcurrentDictionary<Guid, List<InMemoryRefreshTokenModel>> _refreshTokens = new();

    public CustomTokenRepository(IOptions<RefreshTokenOptions> options)
    {
        _options = options?.Value ?? new RefreshTokenOptions();
    }

    public async Task<string> AddAsync(Guid userId, string token, DateTime? expiredDate)
    {
        if (!_refreshTokens.TryGetValue(userId, out List<InMemoryRefreshTokenModel> values) || values == null)
        {
            values = new List<InMemoryRefreshTokenModel>();
        }

        values.Add(new InMemoryRefreshTokenModel(token, expiredDate));

        _refreshTokens[userId] = values;

        return token;
    }

    public async Task<bool> DeleteAsync()
    {
        _refreshTokens.Clear();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId)
        => _refreshTokens.TryRemove(userId, out _);

    public async Task<bool> DeleteAsync(string token)
    {
        foreach (var user in _refreshTokens)
        {
            if (user.Value.Any(x => x.Token == token))
            {
                _refreshTokens[user.Key] = user.Value.Where(x => x.Token != token).ToList();
            }
        }

        return true;
    }

    public async Task<bool> DeleteExpiredAsync(Guid userId)
    {
        var values = Get(userId).Where(x => x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow).ToList();

        _refreshTokens[userId] = values;
        return true;
    }

    public async Task<bool> DeleteExpiredAsync()
    {
        foreach (var user in _refreshTokens)
        {
            await DeleteExpiredAsync(user.Key);
        }

        return true;
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        return null;
    }

    public async Task<int> GetNumberOfActiveTokensAsync(Guid userId)
        => Get(userId).Where(x => (!x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.UtcNow))
          .Count();

    public async Task<string?> GetOldestTokenAsync(Guid userId)
        => Get(userId).Where(x => x.ExpiredDate.HasValue)
            .OrderBy(x => x.ExpiredDate).Select(x => x.Token).FirstOrDefault();

    public async Task<bool> IsValidTokenAsync(Guid userId, string token) 
        => Get(userId).Any(x => x.Token == token &&
        (!_options.TokenExpiredDays.HasValue || DateTime.UtcNow <= x.ExpiredDate));

    private List<InMemoryRefreshTokenModel> Get(Guid userId)
        => _refreshTokens.GetValueOrDefault(userId) ?? new List<InMemoryRefreshTokenModel>();

}
