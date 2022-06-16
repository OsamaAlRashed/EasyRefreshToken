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
}
