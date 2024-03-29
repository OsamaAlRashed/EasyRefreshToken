﻿using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshToken.Tests.IntegrationTests.CustomServiceTests.Mocks;

public class CustomServiceStartup
{
    public static ServiceCollection ConfigureService(Action<RefreshTokenOptions> options = default)
    {
        var services = new ServiceCollection();

        services.AddCustomRefreshToken<User, Guid, CustomTokenRepository>(options);

        return services;
    }
}
