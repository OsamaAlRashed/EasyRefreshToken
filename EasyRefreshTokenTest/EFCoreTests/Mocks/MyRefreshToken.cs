using EasyRefreshToken.EFCore;
using EasyRefreshToken.Tests.Mocks;
using System;

namespace EasyRefreshToken.Tests.EFCoreTests.Mocks;

public class MyRefreshToken : RefreshToken<User, Guid>
{
    public MyRefreshToken()
    {
        DateCreated = DateTime.Now;
    }

    public DateTime DateCreated { get; set; }
}
