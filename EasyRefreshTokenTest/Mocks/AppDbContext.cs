using EasyRefreshToken.Tests.IntegrationTests.EFCoreTests.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EasyRefreshToken.Tests.Mocks;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MyRefreshToken> RefreshTokens { get; set; }
}
