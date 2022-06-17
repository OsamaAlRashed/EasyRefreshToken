using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Models;
using EasyRefreshToken.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.TokenService
{
    public class TokenService<TDbContext> : TokenService<TDbContext, IdentityUser<string> , string> where TDbContext : DbContext, IDbSetRefreshToken<IdentityUser<string>, string>
    {
        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default) : base(context, options)
        {
        }
    }

    public class TokenService<TDbContext, TUser> : TokenService<TDbContext, TUser, string> where TDbContext : DbContext, IDbSetRefreshToken<TUser, string>
    {
        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default): base(context, options)
        {
        }
    }

    public class TokenService<TDbContext, TUser, TKey> : ITokenService where TDbContext : DbContext, IDbSetRefreshToken<TUser, TKey>
    {
        private readonly TDbContext _context;
        private readonly RefreshTokenOptions _options;

        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default)
        {
            _context = context;
            _options = options?.Value ?? new RefreshTokenOptions();
        }

        public async Task<string> OnLogin<TKey>(TKey userId)
        {
            if (!_options.MultiDevice)
            {
                if (_options.BlockLoginWhenActive)
                {
                    var isExist = await _context.RefreshTokens.Where(x => x.Id.Equals(userId)
                        && (!x.ExpiredDate.HasValue || x.ExpiredDate.Value > DateTime.Now))
                        .AnyAsync();
                    if(isExist)
                        return null;
                }
                await Delete(userId);
            }
            return await Add(userId);
        }

        public async Task<bool> OnLogout(string oldToken) => await Delete(oldToken);

        public async Task<string> OnAccessTokenExpired<TKey>(TKey userId, string token)
        {
            var check = await _context.RefreshTokens.Where(x => x.UserId.Equals(userId) && x.Token == token
                && (!_options.TokenExpiredDays.HasValue || DateTime.Now <= x.ExpiredDate)).AnyAsync();
            if (check)
            {
                await Delete(token);
                return await Add(userId);
            }
            return null;
        }

        public async Task<string> OnChangePassword<TKey>(TKey userId)
        {
            if (_options.OnChangePasswordBehavior == Enums.OnChangePasswordBehavior.None)
                return null;
            await Delete(userId);
            if (_options.OnChangePasswordBehavior == Enums.OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken)
                return await Add(userId);
            return "";
        }

        public async Task<bool> Clear()
        {
            try
            {
                var allEntities = await _context.RefreshTokens.ToListAsync();

                _context.RefreshTokens.RemoveRange(allEntities);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Private 

        private async Task<string> Add<TKey>(TKey userId)
        {
            try
            {
                var refreshToken = new RefreshToken<TUser, TKey>
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

        private async Task<bool> Delete(string oldRefreshToken)
        {
            try
            {
                var oldRefreshTokenEntity = await _context.RefreshTokens.Where(x => x.Token == oldRefreshToken).FirstOrDefaultAsync();
                if (oldRefreshTokenEntity != null)
                {
                    _context.RefreshTokens.Remove(oldRefreshTokenEntity);

                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        private async Task<bool> Delete<TKey>(TKey userId)
        {
            try
            {
                var oldRefreshTokenEntity = await _context.RefreshTokens.Where(x => x.Id.Equals(userId)).ToListAsync();
                if (oldRefreshTokenEntity != null)
                {
                    _context.RefreshTokens.RemoveRange(oldRefreshTokenEntity);

                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        private string GenerateToken()
        {
            if (_options.GenerateTokenMethod != null)
                return _options.GenerateTokenMethod();
            return Helpers.GenerateRefreshToken();
        }

        #endregion

    }
}
