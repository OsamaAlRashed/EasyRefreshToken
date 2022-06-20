using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Models;
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
    public class TokenService<TDbContext, TRefreshToken, TUser, TKey> : ITokenService<TKey>
        where TDbContext : DbContext
        where TRefreshToken : RefreshToken<TUser, TKey> , new()
        where TKey : IEquatable<TKey>
    {
        private readonly TDbContext _context;
        private readonly RefreshTokenOptions _options;

        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default)
        {
            _context = context;
            _options = options?.Value ?? new RefreshTokenOptions();
        }

        public async Task<string> OnLogin(TKey userId)
        {
            if (await IsAccessToLimit(userId))
            {
                if (_options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices)
                    return null;
                var oldedToken = await GetOldedToken(userId);
                if (oldedToken == null)
                    return null;
                await Delete(x => x.Token == oldedToken);
            }
            return await Add(userId);
        }

        public async Task<bool> OnLogout(string oldToken) => await Delete(x => x.Token == oldToken);

        public async Task<string> OnAccessTokenExpired(TKey userId, string token)
        {
            var check = await _context.Set<TRefreshToken>().Where(x => x.UserId.Equals(userId) && x.Token == token
                && (!_options.TokenExpiredDays.HasValue || DateTime.Now <= x.ExpiredDate)).AnyAsync();
            if (check)
            {
                await Delete(x => x.Token == token);
                return await Add(userId);
            }
            return null;
        }

        public async Task<string> OnChangePassword(TKey userId)
        {
            if (_options.OnChangePasswordBehavior == Enums.OnChangePasswordBehavior.None)
                return null;
            await Delete(x => x.UserId.Equals(userId));
            if (_options.OnChangePasswordBehavior == Enums.OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken)
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

        private async Task<bool> IsAccessToLimit(TKey userId)
            => _options.MaxNumberOfActiveDevices.HasValue && await GetNumberActiveTokens(userId) >= _options.MaxNumberOfActiveDevices.Value;

        private bool IsMultiDevices() => !_options.MaxNumberOfActiveDevices.HasValue || _options.MaxNumberOfActiveDevices > 1;

        private async Task<int> GetNumberActiveTokens(TKey userId)
            => await _context.Set<TRefreshToken>().Where(x => x.UserId.Equals(userId) && (!x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.Now)).CountAsync();

        private async Task<bool> Delete(Expression<Func<RefreshToken<TUser, TKey>, bool>> filter)
        {
            try
            {
                var oldRefreshTokens = await _context.Set<TRefreshToken>().Where(filter).ToListAsync();
                _context.RemoveRange(oldRefreshTokens);
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
        #endregion

    }
}
