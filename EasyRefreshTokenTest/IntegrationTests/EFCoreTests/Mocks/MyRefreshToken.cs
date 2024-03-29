﻿using EasyRefreshToken.EFCore;
using EasyRefreshToken.Tests.Data;
using System;

namespace EasyRefreshToken.Tests.IntegrationTests.EFCoreTests.Mocks;

public class MyRefreshToken : RefreshToken<User, Guid>
{
    public MyRefreshToken()
    {
        DateCreated = DateTime.Now;
    }

    public DateTime DateCreated { get; set; }
}
