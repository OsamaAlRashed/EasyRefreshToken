﻿using EasyRefreshToken.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<MyRefreshToken> RefreshTokens { get; set; }
    }
}
