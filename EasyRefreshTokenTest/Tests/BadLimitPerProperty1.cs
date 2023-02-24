using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Result;
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
    public class BadLimitPerProperty1
    {
        ITokenService<Guid> tokenService;
        AppDbContext context;
        public BadLimitPerProperty1()
        {
            var provider = Startup.ConfigureService(op =>
            {
                op.MaxNumberOfActiveDevices = 
                    MaxNumberOfActiveDevices.Config("BadUserType", (UserType.Employee, 1), (UserType.Admin, 2));
            }).BuildServiceProvider();
            tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLoginUser()
        {
            Utils util = new Utils(context);
            var user = await util.GenerateUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await tokenService.OnLogin(user.Id));
        }
    }
}
