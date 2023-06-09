using EasyRefreshToken;
using EasyRefreshTokenTest.EFCoreTests.Mocks;
using EasyRefreshTokenTest.InMemoryTests.Mocks;
using EasyRefreshTokenTest.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.InMemoryTests
{
    public class CustomOption1Test
    {
        private readonly ITokenService<Guid> _tokenService;
        private readonly AppDbContext _context;

        public CustomOption1Test()
        {
            var provider = InMemoryStartup.ConfigureService(op =>
            {
                op.MaxNumberOfActiveDevices = MaxNumberOfActiveDevices.Configure(3);
                op.PreventingLoginWhenAccessToMaxNumberOfActiveDevices = true;
                op.TokenExpiredDays = null;
                op.TokenGenerationMethod = () =>
                {
                    string s = "";
                    Random random = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        s += random.Next().ToString();
                    }
                    return s;
                };
                op.OnChangePasswordBehavior = OnChangePasswordBehavior.DeleteAllTokensAndAddNewToken;
            }).BuildServiceProvider();
            _tokenService = provider.GetRequiredService<ITokenService<Guid>>();
            _context = provider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task OnLogin_Ok()
        {
            Utils util = new Utils(_context);
            var user1 = await util.GenerateUser();

            //1
            var tokenResult = await _tokenService.OnLoginAsync(user1.Id);

            Assert.True(tokenResult.IsSucceeded);
        }

        [Fact]
        public async Task OnLogin_LimitOK()
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
        public async Task OnLogin_OverLimit()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id); //1
            var tokenResult2 = await _tokenService.OnLoginAsync(user.Id); //2
            var tokenResult3 = await _tokenService.OnLoginAsync(user.Id); //3
            var tokenResult4 = await _tokenService.OnLoginAsync(user.Id); //4

            var finalResult = tokenResult1.IsSucceeded
                && tokenResult2.IsSucceeded
                && tokenResult3.IsSucceeded;

            Assert.True(finalResult);
            Assert.False(tokenResult4.IsSucceeded);
        }

        [Fact]
        public async Task OnLogout()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult = await _tokenService.OnLoginAsync(user.Id);

            var isSucceded = await _tokenService.OnLogoutAsync(tokenResult.Token);

            Assert.True(isSucceded);
        }

        [Fact]
        public async Task OnAccessTokenExpired()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();
            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id);
            var tokenResult2 = await _tokenService.OnAccessTokenExpiredAsync(user.Id, tokenResult1.Token);
            Assert.True(tokenResult2.IsSucceeded);
        }

        [Fact]
        public async Task OnAccessTokenExpired_AfterLogout()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id);

            await _tokenService.OnLogoutAsync(tokenResult1.Token);

            var tokenResult2 = await _tokenService.OnAccessTokenExpiredAsync(user.Id, tokenResult1.Token);

            Assert.False(tokenResult2.IsSucceeded);
        }

        [Fact]
        public async Task ClearByUserId()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            await _tokenService.OnLoginAsync(user.Id);

            await _tokenService.ClearAsync(user.Id);

            var isExist = await _context.RefreshTokens.Where(x => x.UserId == user.Id).AnyAsync();

            Assert.False(isExist);
        }

        [Fact]
        public async Task Clear()
        {
            Utils util = new Utils(_context);
            var user1 = await util.GenerateUser();
            var user2 = await util.GenerateUser();

            await _tokenService.OnLoginAsync(user1.Id);
            await _tokenService.OnLoginAsync(user2.Id);

            await _tokenService.ClearAsync();

            var isExist = await _context.RefreshTokens.AnyAsync();

            Assert.False(isExist);
        }

        [Fact]
        public async Task OnChangePassword()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult = await _tokenService.OnLoginAsync(user.Id);

            var result = await _tokenService.OnChangePasswordAsync(user.Id);

            Assert.NotEqual("", result);
        }

        [Fact]
        public async Task OnAccessTokenExpired_After_OnChangePassword()
        {
            Utils util = new Utils(_context);
            var user = await util.GenerateUser();

            var tokenResult1 = await _tokenService.OnLoginAsync(user.Id);

            await _tokenService.OnChangePasswordAsync(user.Id);

            var tokenResult2 = await _tokenService.OnAccessTokenExpiredAsync(user.Id, tokenResult1.Token);

            Assert.False(tokenResult2.IsSucceeded);
        }

    }
}
