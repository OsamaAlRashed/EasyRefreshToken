using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.DependencyInjection.Enums;
using EasyRefreshToken.Models;
using EasyRefreshToken.Result;
using EasyRefreshToken.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasyRefreshToken.Service
{
    internal partial class TokenService<TDbContext, TRefreshToken, TUser, TKey> : ITokenService<TKey>
        where TDbContext : DbContext
        where TRefreshToken : RefreshToken<TUser, TKey> , new()
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TDbContext _context;
        private readonly RefreshTokenOptions _options;

        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default)
        {
            _context = context;
            _options = options?.Value ?? new RefreshTokenOptions();
        }

        public async Task<TokenResult> OnLoginAsync(TKey userId)
        {
            var user = await _context.Set<TUser>()
                .Where(x => x.Id.Equals(userId))
                .SingleOrDefaultAsync();

            if (user == null)
                return TokenResult.SetFailed($"User with id {userId} not found.", 404);

            if (await IsAccessToLimit(user))
            {
                var oldedToken = await GetOldestToken(userId);
                if (_options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices || oldedToken == null)
                    return TokenResult.SetFailed("Login not allowed because access to max number of active devices.", 401);

                await Delete(x => x.Token == oldedToken);
            }

            return TokenResult.SetSuccess(await Add(userId));
        }

        public async Task<bool> OnLogoutAsync(string oldToken) => await Delete(x => x.Token == oldToken);

        public async Task<TokenResult> OnAccessTokenExpiredAsync(TKey userId, string token)
        {
            var user = await _context.Set<TUser>().SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (user == null)
                return TokenResult.SetFailed($"User with id {userId} not found", 404);

            var check = await _context.Set<TRefreshToken>()
                .Where(x => x.UserId.Equals(userId) && x.Token == token
                    && (!_options.TokenExpiredDays.HasValue || DateTime.UtcNow <= x.ExpiredDate))
                .AnyAsync();

            if (check)
            {
                await Delete(x => x.Token == token);
                return TokenResult.SetSuccess(await Add(userId));
            }

            return TokenResult.SetFailed( $"{token} not found", 404);
        }

        public async Task<string> OnChangePasswordAsync(TKey userId)
        {
            await Delete(x => x.UserId.Equals(userId));
            if (_options.OnChangePasswordBehavior == OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken)
                return await Add(userId);

            return null;
        }

        public async Task<bool> ClearAsync()
            => await Delete(x => true);

        public async Task<bool> ClearExpiredAsync()
            => await Delete(x => x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow);

        public async Task<bool> ClearExpiredAsync(TKey userId)
            => await Delete(x => x.UserId.Equals(userId) && x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow);

        public async Task<bool> ClearAsync(TKey userId) => await Delete(x => x.UserId.Equals(userId));
    }

    internal partial class TokenService<TDbContext, TRefreshToken, TUser, TKey> : ITokenService<TKey>
        where TDbContext : DbContext
        where TRefreshToken : RefreshToken<TUser, TKey>, new()
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private async Task<string> Add(TKey userId)
        {
            var refreshToken = new TRefreshToken()
            {
                Token = _options.TokenGenerationMethod(),
                UserId = userId,
                ExpiredDate = _options.TokenExpiredDays.HasValue ? DateTime.UtcNow.AddDays(_options.TokenExpiredDays.Value) : null
            };
            _context.Add(refreshToken);
            if (_options.SaveChanges)
                await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        private async Task<bool> IsAccessToLimit(TUser user)
        {
            var limit = GetMaxNumberOfActiveDevicesPerUser(user);
            return limit.HasValue && await GetNumberActiveTokens(user.Id) >= limit;
        }

        private async Task<int> GetNumberActiveTokens(TKey userId)
            => await _context.Set<TRefreshToken>().Where(x => x.UserId.Equals(userId) && (!x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.UtcNow)).CountAsync();

        private async Task<bool> Delete(Expression<Func<RefreshToken<TUser, TKey>, bool>> filter)
        {
            var oldRefreshTokens = await _context.Set<TRefreshToken>().Where(filter).ToListAsync();
            if (!oldRefreshTokens.Any())
                return false;

            _context.RemoveRange(oldRefreshTokens);
            if (_options.SaveChanges)
                await _context.SaveChangesAsync();

            return true;
        }

        private async Task<string> GetOldestToken(TKey userId)
            => await _context.Set<TRefreshToken>()
                .Where(x => x.UserId.Equals(userId) && x.ExpiredDate.HasValue)
                .OrderBy(x => x.ExpiredDate).Select(x => x.Token).FirstOrDefaultAsync();

        private int? GetMaxNumberOfActiveDevicesPerUser(TUser user = default)
        {
            if (_options.MaxNumberOfActiveDevices == null)
                return null;

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
                        return null;

                    return _options.MaxNumberOfActiveDevices.LimitPerProperty.Item2[propValue];
            }

            return null;
        }

    }
}
