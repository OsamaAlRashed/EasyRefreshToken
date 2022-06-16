using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRefreshToken.Models
{
    public interface IDbSetRefreshToken<TUser, TKey>
    {
        public DbSet<RefreshToken<TUser, TKey>> RefreshTokens { get; set; }
    }

    public interface IDbSetRefreshToken<TUser> where TUser : IdentityUser<string>
    {
        public DbSet<RefreshToken<TUser, string>> RefreshTokens { get; set; }
    }

    public interface IDbSetRefreshToken
    {
        public DbSet<RefreshToken<IdentityUser<string>, string>> RefreshTokens { get; set; }
    }

}
