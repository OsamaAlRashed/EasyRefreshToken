using EasyRefreshToken;
using EasyRefreshToken.EFCore;
using EasyRefreshToken.Abstractions;
using Microsoft.Extensions.Options;
using EasyRefreshToken.Tests.UnitTests.Models;
using NSubstitute;
using Xunit;
using System.Threading.Tasks;
using EasyRefreshToken.Commons;
using System;
using EasyRefreshToken.Providers;

namespace EasyRefreshToken.Tests.UnitTests;

public class EFTokenServiceTest
{
    private readonly EFTokenService<User, int> _sut;
    private readonly ITokenRepository<User, int> _tokenRepository
        = Substitute.For<ITokenRepository<User, int>>();
    private readonly IOptions<EFTokenOptions> _options;
    private readonly IDateTimeProvider _dateTimeProvider;

    public EFTokenServiceTest()
    {
        _dateTimeProvider = new DateTimeProvider();
        _sut = new EFTokenService<User, int>(_tokenRepository, _options, _dateTimeProvider);
    }

    [Fact]
    public async Task OnLoginAsync_ShouldReturnNewToken_WhenTheArgsAreValid()
    {
        // Arrange
        int userId = 1;

        _tokenRepository.GetByIdAsync(userId).Returns(new User(userId));
        _tokenRepository.GetNumberOfActiveTokensAsync(userId).Returns(0);
        _tokenRepository.GetOldestTokenAsync(userId).Returns(string.Empty);
        _tokenRepository.DeleteAsync(string.Empty).Returns(true);

        // Act
        var tokenResult = await _sut.OnLoginAsync(userId);

        // Assert
        Assert.True(tokenResult.IsSucceeded);
        Assert.False(string.IsNullOrEmpty(tokenResult.Token));
    }

    [Fact]
    public async Task OnLogoutAsync_ShouldRemoveTokenAndReturnTrue()
    {
        // Arrange
        string token = "token";
        _tokenRepository.DeleteAsync(token).Returns(true);

        // Act
        bool result = await _sut.OnLogoutAsync(token);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task OnAccessTokenExpiredAsync_ShouldReturnANewToken_WhenTheTokenIsValid()
    {
        // Arrange
        int userId = 1;
        string oldToken = "oldToken";
        string newToken = "newToken";

        _tokenRepository.IsValidTokenAsync(userId, oldToken).Returns(true);
        _tokenRepository.DeleteAsync(oldToken).Returns(true);
        _tokenRepository.AddAsync(userId, oldToken, DateTime.MaxValue).Returns(newToken);

        // Act
        var tokenResult = await _sut.OnAccessTokenExpiredAsync(userId, oldToken, true);

        // Assert
        Assert.True(tokenResult.IsSucceeded);
        Assert.Equal(newToken, tokenResult.Token);
    }


}
