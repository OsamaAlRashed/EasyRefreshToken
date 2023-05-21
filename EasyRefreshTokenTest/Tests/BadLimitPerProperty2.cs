using EasyRefreshToken.DependencyInjection;
using EasyRefreshTokenTest.Mock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EasyRefreshTokenTest.Tests
{
    public class BadLimitPerProperty2
    {
        [Fact]
        public async Task Setup_OutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                MaxNumberOfActiveDevices.Configure("asdsad", (UserType.Employee, -1), (UserType.Admin, 2)));
        }

        [Fact]

        public async Task Setup_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MaxNumberOfActiveDevices.Configure(null, (UserType.Employee, 1), (UserType.Admin, 2)));
        }

        [Fact]
        public async Task Setup_Exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MaxNumberOfActiveDevices.Configure(null, (null, 1), (UserType.Admin, 2)));
        }
    }
}
