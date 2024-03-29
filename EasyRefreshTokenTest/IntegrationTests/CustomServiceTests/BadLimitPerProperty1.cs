﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using EasyRefreshToken.Tests.Data;
using System.Linq;
using EasyRefreshToken.Exceptions;
using EasyRefreshToken.Tests.IntegrationTests.InMemoryTests.Mocks;

namespace EasyRefreshToken.Tests.IntegrationTests.CustomServiceTests;

public class BadLimitPerProperty1
{
    private readonly ITokenService<Guid> _tokenService;
    private readonly AppDbContext _context;

    public BadLimitPerProperty1()
    {
        var provider = InMemoryStartup.ConfigureService(op =>
        {
            op.GetUserById = (serviceProvider, id) =>
            {
                return serviceProvider.GetRequiredService<AppDbContext>()
                    .Set<User>().Where(x => x.Id == id)
                    .FirstOrDefault();
            };
            op.MaxNumberOfActiveDevices =
                MaxNumberOfActiveDevices.Configure("BadUserType", (UserType.Employee, 1), (UserType.Admin, 2));
        }).BuildServiceProvider();
        _tokenService = provider.GetRequiredService<ITokenService<Guid>>();
        _context = provider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public async Task OnLoginUser()
    {
        Utils util = new Utils(_context);
        var user = await util.GenerateUser();

        await Assert.ThrowsAsync<PropertyNameNotExistException>(async ()
            => await _tokenService.OnLoginAsync(user.Id));
    }
}
