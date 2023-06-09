using EasyRefreshToken.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using EasyRefreshToken;
using EasyRefreshTokenTest.EFCoreTests.Mocks;
using EasyRefreshTokenTest.Mocks;
using EasyRefreshTokenTest.InMemoryTests.Mocks;
using System.Linq;

namespace EasyRefreshTokenTest.InMemoryTests
{
    public class CustomLimitPerTypeTest
    {
        private readonly ITokenService<Guid> _tokenService;
        private readonly AppDbContext _context;
        public CustomLimitPerTypeTest()
        {
            var provider = InMemoryStartup.ConfigureService(op =>
            {
                op.GetUserById = (serviceProvider, id) =>
                {
                    return serviceProvider.GetRequiredService<AppDbContext>()
                        .Set<User>().Where(x => x.Id == id)
                        .FirstOrDefault();
                };
                op.MaxNumberOfActiveDevices = MaxNumberOfActiveDevices.Configure((typeof(SubUser1), 1), (typeof(SubUser2), 2));
            }).BuildServiceProvider();
            _tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            _context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLoginUser_Limit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1
            var tokenResult2 = await _tokenService.OnLoginAsync(user.Id); //2
            var tokenResult3 = await _tokenService.OnLoginAsync(user.Id); //3

            var finalResult = tokenResult1.IsSucceeded
                && tokenResult2.IsSucceeded
                && tokenResult3.IsSucceeded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginSubUser1_Limit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUserSubUser1();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1

            var finalResult = tokenResult1.IsSucceeded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginSubUser2_Limit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUserSubUser2();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1
            var tokenResult2 = await _tokenService.OnLoginAsync(user.Id); //2

            var finalResult = tokenResult1.IsSucceeded
                && tokenResult2.IsSucceeded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginUser_OverLimit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1
            var tokenResult2 = await _tokenService.OnLoginAsync(user.Id); //2
            var tokenResult3 = await _tokenService.OnLoginAsync(user.Id); //3
            var tokenResult4 = await _tokenService.OnLoginAsync(user.Id); //4

            var finalResult = tokenResult1.IsSucceeded
                && tokenResult2.IsSucceeded
                && tokenResult3.IsSucceeded
                && tokenResult4.IsSucceeded;

            Assert.True(finalResult);
        }

        [Fact]
        public async Task OnLoginSubUser1_OverLimit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUserSubUser1();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1
            var tokenResult2 = await _tokenService.OnLoginAsync(user.Id); //2

            Assert.True(tokenResult1.IsSucceeded);
            Assert.False(tokenResult2.IsSucceeded);
        }

        [Fact]
        public async Task OnLoginSubUser2_OverLimit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUserSubUser2();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1
            var tokenResult2 = await _tokenService.OnLoginAsync(user.Id); //2
            var tokenResult3 = await _tokenService.OnLoginAsync(user.Id); //3

            var finalResult = tokenResult1.IsSucceeded
                && tokenResult2.IsSucceeded;

            Assert.True(finalResult);
            Assert.False(tokenResult3.IsSucceeded);
        }
    }
}
