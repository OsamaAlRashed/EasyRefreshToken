﻿using EasyRefreshToken.InMemory;
using EasyRefreshToken.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshToken.Tests.IntegrationTests.InMemoryTests.Mocks;

public class InMemoryStartup
{
    public static ServiceCollection ConfigureService(Action<InMemoryTokenOptions<User, Guid>> options = default)
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(op => op.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddInMemoryRefreshToken(options);

        return services;
    }
}
