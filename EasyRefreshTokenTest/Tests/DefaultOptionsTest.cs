using EasyRefreshToken.Service;
using EasyRefreshTokenTest.Mock;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.Tests
{
    public class DefaultOptionsTest
    {
        ITokenService<Guid> tokenService;
        AppDbContext context;
        public DefaultOptionsTest()
        { 
            var provider = Startup.ConfigureService(default).BuildServiceProvider();
            tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLogin_Ok()
        {
            Utils util = new Utils(context);
            var user1 = await util.GenerateUser();

            //1
            var tokenResult = await tokenService.OnLogin(user1.Id);

            Assert.True(tokenResult.IsSucceded);
        }

        [Fact]
        public async Task OnLogin_UserNotFound()
        {
            var tokenResult = await tokenService.OnLogin(Guid.NewGuid());

            Assert.False(tokenResult.IsSucceded);
        }

        [Fact]
        public async Task OnLogin_LimitOK()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2
            var tokenResult3 = await tokenService.OnLogin(user.Id); //3
            var tokenResult4 = await tokenService.OnLogin(user.Id); //4
            var tokenResult5 = await tokenService.OnLogin(user.Id); //5
            var tokenResult6 = await tokenService.OnLogin(user.Id); //6
            var tokenResult7 = await tokenService.OnLogin(user.Id); //7

            var finalResult = tokenResult1.IsSucceded
                && tokenResult2.IsSucceded
                && tokenResult3.IsSucceded
                && tokenResult4.IsSucceded
                && tokenResult5.IsSucceded
                && tokenResult6.IsSucceded
                && tokenResult7.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLogout()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult = await tokenService.OnLogin(user.Id);

            var isSucceded = await tokenService.OnLogout(tokenResult.Token);

            Assert.True(isSucceded);
        }

        [Fact]
        public async Task OnAccessTokenExpired()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();
            var tokenResult1 = await tokenService.OnLogin(user.Id);
            var tokenResult2 = await tokenService.OnAccessTokenExpired(user.Id, tokenResult1.Token);
            Assert.True(tokenResult2.IsSucceded);
        }

        [Fact]
        public async Task OnAccessTokenExpired_AfterLogout()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult1 = await tokenService.OnLogin(user.Id);

            await tokenService.OnLogout(tokenResult1.Token);

            var tokenResult2 = await tokenService.OnAccessTokenExpired(user.Id, tokenResult1.Token);

            Assert.False(tokenResult2.IsSucceded);
        }

        [Fact]
        public async Task ClearByUserId()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            await tokenService.OnLogin(user.Id);

            await tokenService.Clear(user.Id);

            var isExist = await context.RefreshTokens.Where(x => x.UserId == user.Id).AnyAsync();

            Assert.False(isExist);
        }

        [Fact]
        public async Task Clear()
        {
            Utils util = new Utils(context);
            var user1 = await util.GenerateUser();
            var user2 = await util.GenerateUser();

            await tokenService.OnLogin(user1.Id);
            await tokenService.OnLogin(user2.Id);

            await tokenService.Clear();

            var isExist = await context.RefreshTokens.AnyAsync();

            Assert.False(isExist);
        }

        [Fact]
        public async Task OnChangePassword()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult = await tokenService.OnLogin(user.Id);

            var result = await tokenService.OnChangePassword(user.Id);

            Assert.Equal("", result);
        }

        [Fact]
        public async Task OnAccessTokenExpired_After_OnChangePassword()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult1 = await tokenService.OnLogin(user.Id);

            await tokenService.OnChangePassword(user.Id);

            var tokenResult2 = await tokenService.OnAccessTokenExpired(user.Id, tokenResult1.Token);

            Assert.False(tokenResult2.IsSucceded);
        }

        [Fact]
        public async Task OnAccessTokenExpired_By_RefreshTokenExpired()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult1 = await tokenService.OnLogin(user.Id);

            var token = context.RefreshTokens.Where(x => x.Token == tokenResult1.Token).FirstOrDefault();
            token.ExpiredDate = token.ExpiredDate.Value.AddDays(-7);
            await context.SaveChangesAsync();

            var tokenResult2 = await tokenService.OnAccessTokenExpired(user.Id, tokenResult1.Token);

            Assert.False(tokenResult2.IsSucceded);
        }

    }
}
