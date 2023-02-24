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
    public class BadLimitPerProperty2
    {
        [Fact]
        public async Task Setup_OutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                MaxNumberOfActiveDevices.Config("asdsad", (UserType.Employee, -1), (UserType.Admin, 2)));
        }

        [Fact]

        public async Task Setup_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MaxNumberOfActiveDevices.Config(null, (UserType.Employee, 1), (UserType.Admin, 2)));
        }

        [Fact]
        public async Task Setup_Exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MaxNumberOfActiveDevices.Config(null, (null, 1), (UserType.Admin, 2)));
        }
    }
}
