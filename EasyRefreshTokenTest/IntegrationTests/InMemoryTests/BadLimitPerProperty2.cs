using EasyRefreshToken.Exceptions;
using EasyRefreshToken.Tests.Data;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshToken.Tests.IntegrationTests.InMemoryTests;

public class BadLimitPerProperty2
{
    [Fact]
    public async Task Setup_OutOfRangeException()
    {
        Assert.Throws<LimitOutOfRangeException>(() =>
            MaxNumberOfActiveDevices.Configure("asdsad", (UserType.Employee, -1), (UserType.Admin, 2)));
    }

    [Fact]

    public async Task Setup_ArgumentNullException()
    {
        Assert.Throws<PropertyNameNullException>(() =>
            MaxNumberOfActiveDevices.Configure(null, (UserType.Employee, 1), (UserType.Admin, 2)));
    }

    [Fact]
    public async Task Setup_Exception()
    {
        Assert.Throws<PropertyNameNullException>(() =>
            MaxNumberOfActiveDevices.Configure(null, (null, 1), (UserType.Admin, 2)));
    }
}
