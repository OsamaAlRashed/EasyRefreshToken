using EasyRefreshToken.TokenService;
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
        ITokenService<Guid> tokenService;
        AppDbContext context;
        public SaveChangesPropertyTest()
        {
            var provider = Startup.ConfigureService(op =>
            {
                op.SaveChanges = false;
            }).BuildServiceProvider();
            tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLogin_WithoutSaveChanges()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult = await tokenService.OnLogin(user.Id);

            var isExist = await context.RefreshTokens.Where(x => x.Token == tokenResult.Token).AnyAsync();

            Assert.False(isExist);
        }

        [Fact]
        public async Task OnLogin_WithSaveChanges()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult = await tokenService.OnLogin(user.Id);
            await context.SaveChangesAsync();

            var isExist = await context.RefreshTokens.Where(x => x.Token == tokenResult.Token).AnyAsync();

            Assert.True(isExist);
        }
    }

}
