using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Service;
using EasyRefreshTokenTest.Mock;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.Tests
{
    public class LimitPerProperty
    {
        ITokenService<Guid> tokenService;
        AppDbContext context;
        public LimitPerProperty()
        {
            var provider = Startup.ConfigureService(op =>
            {
                op.MaxNumberOfActiveDevices = MaxNumberOfActiveDevices.Configure("UserType", (UserType.Employee, 1), (UserType.Admin, 2));
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
        public async Task OnLoginEmployee_Limit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateEmployee();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1

            var finalResult = tokenResult1.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginAdmin_Limit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateAdmin();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2

            var finalResult = tokenResult1.IsSucceded
                && tokenResult2.IsSucceded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginEmpoyee_OverLimit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateEmployee();

            var tokenResult1 = await tokenService.OnLogin(user.Id); //1
            var tokenResult2 = await tokenService.OnLogin(user.Id); //2

            Assert.True(tokenResult1.IsSucceded);
            Assert.False(tokenResult2.IsSucceded);
        }

        [Fact]
        public async Task OnLoginAdmin_OverLimit()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateAdmin();

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
