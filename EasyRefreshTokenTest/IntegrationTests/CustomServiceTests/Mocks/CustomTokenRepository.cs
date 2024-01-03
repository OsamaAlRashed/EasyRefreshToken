using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Tests.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRefreshToken.Tests.IntegrationTests.CustomServiceTests.Mocks;

internal record InMemoryRefreshTokenModel(string Token, DateTime ExpiredDate);

public class CustomTokenRepository : ITokenRepository<User, Guid>
{
    private readonly RefreshTokenOptions _options;
    private ConcurrentDictionary<Guid, List<InMemoryRefreshTokenModel>> _refreshTokens = new();
    private readonly IDateTimeProvider _dateTimeProvider;

    public CustomTokenRepository(IOptions<RefreshTokenOptions> options, IDateTimeProvider dateTimeProvider)
    {
        _options = options?.Value ?? new RefreshTokenOptions();
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<string> AddAsync(Guid userId, string token, DateTime expiredDate)
    {
        if (!_refreshTokens.TryGetValue(userId, out List<InMemoryRefreshTokenModel> values) || values == null)
        {
            values = new List<InMemoryRefreshTokenModel>();
        }

        values.Add(new InMemoryRefreshTokenModel(token, expiredDate));

        _refreshTokens[userId] = values;

        return Task.FromResult(token);
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
        var values = Get(userId)
            .Where(x => x.ExpiredDate <= _dateTimeProvider.Now)
            .ToList();

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
        => Get(userId).Where(x => x.ExpiredDate >= DateTime.UtcNow)
          .Count();

    public async Task<string> GetOldestTokenAsync(Guid userId)
        => Get(userId)
            .OrderBy(x => x.ExpiredDate)
            .Select(x => x.Token)
            .FirstOrDefault();

    public async Task<bool> IsValidTokenAsync(Guid userId, string token)
        => Get(userId)
            .Any(x => 
                x.Token == token &&
                DateTime.UtcNow <= x.ExpiredDate);

    private List<InMemoryRefreshTokenModel> Get(Guid userId)
        => _refreshTokens.GetValueOrDefault(userId) ?? new List<InMemoryRefreshTokenModel>();

}
