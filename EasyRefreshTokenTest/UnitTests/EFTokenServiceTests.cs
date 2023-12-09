using Xunit;
using Moq;
using System.Threading.Tasks;
using EasyRefreshToken;
using EasyRefreshToken.EFCore;
using EasyRefreshToken.Abstractions;
using Microsoft.Extensions.Options;
using System;
using EasyRefreshToken.Tests.UnitTests.Models;
using EasyRefreshToken.Commons;

public class EFTokenServiceTests
{
    private readonly ITokenService<int> _tokenService;
    private readonly Mock<ITokenRepository<User, int>> tokenRepositoryMock;
    private readonly IOptions<EFTokenOptions> options;

    public EFTokenServiceTests()
    {
        tokenRepositoryMock = new Mock<ITokenRepository<User, int>>();
        options = Options.Create(new EFTokenOptions() { });
        _tokenService = new EFTokenService<User, int>(tokenRepositoryMock.Object, options);
    }

    //[Fact]
    //public async Task OnLoginAsync_ShouldReturnTokenResult()
    //{
    //    // Arrange
    //    int userId = 1;
    //    var expectedResult = new TokenResult() 
    //    { 
    //        IsSucceeded = true,
    //        Token = Helpers.GenerateRefreshToken(),
    //        Code = 200
    //    };

    //    tokenRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new User() { Id = 1 });
    //    tokenRepositoryMock.Setup(x => x.GetNumberOfActiveTokensAsync(userId)).ReturnsAsync(0);
    //    tokenRepositoryMock.Setup(x => x.AddAsync(userId, It.IsAny<string>(), null)).ReturnsAsync(expectedResult.Token);

    //    // Act
    //    var actual = await _tokenService.OnLoginAsync(userId);

    //    // Assert
    //    Assert.Equal(expectedResult, actual);
    //}

    //[Fact]
    //public async Task OnLogoutAsync_ShouldReturnTrue()
    //{
    //    // Arrange
    //    string token = "refreshToken";
    //    tokenRepositoryMock.Setup(x => x.DeleteAsync(token)).ReturnsAsync(true);

    //    // Act
    //    var result = await tokenService.OnLogoutAsync(token);

    //    // Assert
    //    Assert.True(result);
    //    tokenRepositoryMock.Verify(x => x.DeleteAsync(token), Times.Once);
    //}


}
