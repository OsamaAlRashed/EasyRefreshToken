using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshTokenTest.Mocks;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace EasyRefreshTokenTest.CustomServiceTests.Mocks
{
    public class CustomTokenRepository : ITokenRepository<User, Guid>
    {
        private readonly RefreshTokenOptions _options;

        public CustomTokenRepository(IOptions<RefreshTokenOptions> options)
        {
            _options = options?.Value ?? new RefreshTokenOptions();
        }

        public async Task<string> Add(Guid userId, string token, DateTime? expiredDate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteExpired(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteExpired()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetNumberActiveTokens(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetOldestToken(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValidToken(Guid userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}
