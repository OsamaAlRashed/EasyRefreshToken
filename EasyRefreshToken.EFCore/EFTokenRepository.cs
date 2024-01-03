using EasyRefreshToken.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasyRefreshToken.EFCore
{
    internal class EFTokenRepository<TDbContext, TRefreshToken, TUser, TKey> : ITokenRepository<TUser, TKey>
        where TDbContext : DbContext
        where TRefreshToken : RefreshToken<TUser, TKey>, new()
        where TUser : class, IUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TDbContext _context;
        private readonly EFTokenOptions _options;


        public EFTokenRepository(TDbContext context, IOptions<EFTokenOptions> options)
        {
            _context = context;
            _options = options?.Value ?? new EFTokenOptions();
        }

        public async Task<bool> IsValidTokenAsync(TKey key, string token)
        {
            return await _context.Set<TRefreshToken>()
                .Where(x => x.UserId.Equals(key) &&
                            x.Token == token && 
                            DateTime.UtcNow <= x.ExpiredDate)
                .AnyAsync();
        }

        public async Task<bool> DeleteAsync()
        {
            return await DeleteAsync(x => true);
        }

        public async Task<bool> DeleteAsync(TKey key)
        {
            return await DeleteAsync(x => x.UserId.Equals(key));
        }

        public async Task<bool> DeleteAsync(string token)
        {
            return await DeleteAsync(x => x.Token.Equals(token));
        }

        public async Task<bool> DeleteExpiredAsync()
            => await DeleteAsync(x => x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow);

        public async Task<bool> DeleteExpiredAsync(TKey key)
            => await DeleteAsync(x => x.UserId.Equals(key) && x.ExpiredDate.HasValue && x.ExpiredDate < DateTime.UtcNow);

        public async Task<TUser?> GetByIdAsync(TKey userId)
        {
            return await _context.Set<TUser>()
                .Where(x => x.Id.Equals(userId))
                .FirstOrDefaultAsync();
        }

        public async Task<string> AddAsync(TKey userId, string token, DateTime? expiredDate)
        {
            var refreshToken = new TRefreshToken()
            {
                Token = token,
                UserId = userId,
                ExpiredDate = expiredDate
            };
            _context.Add(refreshToken);
            if (_options.SaveChanges)
                await _context.SaveChangesAsync();

            return refreshToken.Token;
        }
        public async Task<int> GetNumberOfActiveTokensAsync(TKey userId)
            => await _context.Set<TRefreshToken>()
            .Where(x => x.UserId.Equals(userId) && (!x.ExpiredDate.HasValue || x.ExpiredDate >= DateTime.UtcNow))
            .CountAsync();

        public async Task<string?> GetOldestTokenAsync(TKey userId)
            => await _context.Set<TRefreshToken>()
                .Where(x => x.UserId.Equals(userId) && x.ExpiredDate.HasValue)
                .OrderBy(x => x.ExpiredDate).Select(x => x.Token).FirstOrDefaultAsync();

        private async Task<bool> DeleteAsync(Expression<Func<RefreshToken<TUser, TKey>, bool>> filter)
        {
            var oldRefreshTokens = await _context.Set<TRefreshToken>().Where(filter).ToListAsync();
            if (!oldRefreshTokens.Any())
                return false;

            _context.RemoveRange(oldRefreshTokens);
            if (_options.SaveChanges)
                await _context.SaveChangesAsync();

            return true;
        }
    }
}
