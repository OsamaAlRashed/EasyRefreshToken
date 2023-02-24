using EasyRefreshToken.Abstractions;
using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken.Models;
using EasyRefreshToken.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.Identity
{
    internal class TokenService<TDbContext, TRefreshToken, TIdentityUser, TKey> 
        : Service.TokenService<TDbContext, TRefreshToken, TUser, TKey>
        where TDbContext : DbContext
        where TRefreshToken : RefreshToken<TUser, TKey>, new()
        where TUser : IdentityUser<TKey>
        where TIdentityUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public TokenService(TDbContext context, IOptions<RefreshTokenOptions> options = default)
            : base(context, options)
        {

        }
    }
}
