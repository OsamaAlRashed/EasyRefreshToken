using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Service;
using EasyRefreshToken.Utils;
using EasyRefreshTokenTest.Mock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.Tests
{
    public class CustomLimitPerTypeTest
    {
        ITokenService<Guid> tokenService;
        AppDbContext context;
        public CustomLimitPerTypeTest()
        {
            var provider = Startup.ConfigureService(op =>
            {
                op.MaxNumberOfActiveDevices = MaxNumberOfActiveDevices.Config((typeof(SubUser1), 1), (typeof(SubUser2), 2));
            }).BuildServiceProvider();
            tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLoginUser_Limit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2
            var tokenResult3 = await tokenService.OnLogin(user.Id); //3

            var finalResult = tokenResult1.IsSucceded
                && tokenResult2.IsSucceded
                && tokenResult3.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginSubUser1_Limit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUserSubUser1();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1

            var finalResult = tokenResult1.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginSubUser2_Limit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUserSubUser2();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2

            var finalResult = tokenResult1.IsSucceded
                && tokenResult2.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginUser_OverLimit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2
            var tokenResult3 = await tokenService.OnLogin(user.Id); //3
            var tokenResult4 = await tokenService.OnLogin(user.Id); //4

            var finalResult = tokenResult1.IsSucceded
                && tokenResult2.IsSucceded
                && tokenResult3.IsSucceded
                && tokenResult4.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginSubUser1_OverLimit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUserSubUser1();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2

            Assert.True(tokenResult1.IsSucceded);
            Assert.False(tokenResult2.IsSucceded);
        }

        [Fact]
        public async Task OnLoginSubUser2_OverLimit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUserSubUser2();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2
            var tokenResult3 = await tokenService.OnLogin(user.Id); //3

            var finalResult = tokenResult1.IsSucceded
                && tokenResult2.IsSucceded;

            Assert.True(finalResult);
            Assert.False(tokenResult3.IsSucceded);
        }
    }
}
