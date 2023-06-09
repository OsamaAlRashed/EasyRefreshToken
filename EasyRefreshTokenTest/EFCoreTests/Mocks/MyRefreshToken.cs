using EasyRefreshToken.EFCore;
using EasyRefreshTokenTest.Mocks;
using System;

namespace EasyRefreshTokenTest.EFCoreTests.Mocks;

public class MyRefreshToken : RefreshToken<User, Guid>
{
    public MyRefreshToken()
    {
        DateCreated = DateTime.Now;
    }

    public DateTime DateCreated { get; set; }
}
