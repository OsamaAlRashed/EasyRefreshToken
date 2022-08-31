using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.DependencyInjection.Enums;
using EasyRefreshToken.Models;
using EasyRefreshToken.Result;
using EasyRefreshToken.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.TokenService
{
    internal class TokenService<TDbContext, TRefreshToken, TUser, TKey> : ITokenService<TKey>
        where TDbContext : DbContext
        where TRefreshToken : RefreshToken<TUser, TKey> , new()
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TDbContext _context;
        private readonly RefreshTokenOptions _options;

        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default)
        {
            _context = context;
            _options = options?.Value ?? new RefreshTokenOptions();
        }

        public async Task<TokenResult> OnLogin(TKey userId)
        {
            var user = await _context.Set<TUser>().SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (user == null)
                return new TokenResult
                {
                    ErrorMessage = $"User with id {userId} not found",
                    IsSucceded = false,
                };

            if (await IsAccessToLimit(userId, user))
            {
                var oldedToken = await GetOldedToken(userId);
                if (_options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices || oldedToken == null)
                    return new TokenResult
                    {
                        ErrorMessage = "Login not allowed because access to max number of active devices.",
                        IsSucceded = false,
                    };
                await Delete(x => x.Token == oldedToken);
            }
            return new TokenResult
            {
                Token = await Add(userId),
                IsSucceded = true
            };
        }

        public async Task<bool> OnLogout(string oldToken) => await Delete(x => x.Token == oldToken);

        public async Task<TokenResult> OnAccessTokenExpired(TKey userId, string token)
        {
            var userType = await _context.Set<TUser>().SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (userType == null)
                return new TokenResult
                {
                    ErrorMessage = $"User with id {userId} not found",
                    IsSucceded = false,
                };

            var check = await _context.Set<TRefreshToken>()
                .Where(x => x.UserId.Equals(userId) && x.Token == token
                    && (!_options.TokenExpiredDays.HasValue || DateTime.Now <= x.ExpiredDate)).AnyAsync();
            if (check)
            {
                await Delete(x => x.Token == token);
                return new TokenResult
                {
                    Token = await Add(userId),
                    IsSucceded = true
                };
            }
            return new TokenResult
            {
                ErrorMessage = $"User with id {userId} not found",
                IsSucceded = false,
            };
        }

        public async Task<string> OnChangePassword(TKey userId)
        {
            await Delete(x => x.UserId.Equals(userId));
            if (_options.OnChangePasswordBehavior == OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken)
                return await Add(userId);
            return "";
        }

        public async Task<bool> Clear()
            => await Delete(x => true);

        public async Task<bool> ClearExpired()
            => await Delete(x => x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.Now);

        public async Task<bool> ClearExpired(TKey userId)
            => await Delete(x => x.UserId.Equals(userId) && x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.Now);

        public async Task<bool> Clear(TKey userId) => await Delete(x => x.UserId.Equals(userId));

        #region Private 

        private async Task<string> Add(TKey userId)
        {
            try
            {
                var refreshToken =  new TRefreshToken()
                {
                    Token = GenerateToken(),
                    UserId = userId,
                    ExpiredDate = _options.TokenExpiredDays.HasValue ? DateTime.Now.AddDays(_options.TokenExpiredDays.Value) : null
                };
                _context.Add(refreshToken);
                if(_options.SaveChanges)
                    await _context.SaveChangesAsync();

                return refreshToken.Token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateToken()
        {
            if (_options.TokenGenerationMethod != null)
                return _options.TokenGenerationMethod();
            return Helpers.GenerateRefreshToken();
        }

        private async Task<bool> IsAccessToLimit(TKey userId, TUser user)
        {
            var limit = GetMaxNumberOfActiveDevicesPerUser(user);
            return limit.HasValue && await GetNumberActiveTokens(userId) >= limit;
        }

        private async Task<int> GetNumberActiveTokens(TKey userId)
            => await _context.Set<TRefreshToken>().Where(x => x.UserId.Equals(userId) && (!x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.Now)).CountAsync();

        private async Task<bool> Delete(Expression<Func<RefreshToken<TUser, TKey>, bool>> filter)
        {
            try
            {
                var oldRefreshTokens = await _context.Set<TRefreshToken>().Where(filter).ToListAsync();
                if (!oldRefreshTokens.Any())
                    return false;
                _context.RemoveRange(oldRefreshTokens);
                if (_options.SaveChanges)
                    await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GetOldedToken(TKey userId)
            => await _context.Set<TRefreshToken>().Where(x => x.UserId.Equals(userId) && x.ExpiredDate.HasValue)
            .OrderBy(x => x.ExpiredDate).Select(x => x.Token).FirstOrDefaultAsync();

        private int? GetMaxNumberOfActiveDevicesPerUser(TUser user = default)
        {
            if (_options.MaxNumberOfActiveDevices == null)
                return null;

            switch (_options.MaxNumberOfActiveDevices.type)
            {
                case MaxNumberOfActiveDevicesType.GlobalLimit:
                    return _options.MaxNumberOfActiveDevices.globalLimit;

                case MaxNumberOfActiveDevicesType.LimitPerType:
                    if(_options.MaxNumberOfActiveDevices.limitPerType.ContainsKey(user.GetType()))
                        return _options.MaxNumberOfActiveDevices.limitPerType[user.GetType()];
                    break;

                case MaxNumberOfActiveDevicesType.LimitPerProperty:
                    var propValue = GetPropertyValue(user);
                    if (propValue == null || !_options.MaxNumberOfActiveDevices.limitPerProperty.Item2.ContainsKey(propValue))
                        return null;
                    return _options.MaxNumberOfActiveDevices.limitPerProperty.Item2[propValue];
            }
            return null;
        }

        private object GetPropertyValue(TUser user)
        {
            var propName = _options.MaxNumberOfActiveDevices.limitPerProperty.Item1;

            var prop = user.GetType().GetProperties().Where(x => x.Name.ToLower() == propName.ToLower()).FirstOrDefault();
            if (prop == null)
                throw new Exception("property name not exist in the given object");

            return prop.GetValue(user);
        }

        #endregion
    }
}
