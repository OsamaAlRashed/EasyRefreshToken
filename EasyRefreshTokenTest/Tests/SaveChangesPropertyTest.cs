using EasyRefreshToken;
using EasyRefreshTokenTest.Mock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.Tests
{
    public class SaveChangesPropertyTest
    {
        private readonly ITokenService<Guid> _tokenService;
        private readonly AppDbContext _context;

        public SaveChangesPropertyTest()
        {
            var provider = Startup.ConfigureService(op =>
            {
                op.SaveChanges = false;
            }).BuildServiceProvider();
            _tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            _context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLogin_WithoutSaveChanges()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult = await _tokenService.OnLoginAsync(user.Id);

            var isExist = await _context.RefreshTokens.Where(x => x.Token == tokenResult.Token).AnyAsync();

            Assert.False(isExist);
        }

        [Fact]
        public async Task OnLogin_WithSaveChanges()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult = await _tokenService.OnLoginAsync(user.Id);
            await _context.SaveChangesAsync();

            var isExist = await _context.RefreshTokens.Where(x => x.Token == tokenResult.Token).AnyAsync();

            Assert.True(isExist);
        }
    }

}
