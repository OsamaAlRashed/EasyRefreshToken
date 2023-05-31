﻿using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken;
using EasyRefreshTokenTest.Mock;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.Tests
{
    public class BadLimitPerProperty1
    {
        private readonly ITokenService<Guid> _tokenService;
        private readonly AppDbContext _context;

        public BadLimitPerProperty1()
        {
            var provider = Startup.ConfigureService(op =>
            {
                op.MaxNumberOfActiveDevices = 
                    MaxNumberOfActiveDevices.Configure("BadUserType", (UserType.Employee, 1), (UserType.Admin, 2));
            }).BuildServiceProvider();
            _tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            _context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLoginUser()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await _tokenService.OnLoginAsync(user.Id));
        }
    }
}
