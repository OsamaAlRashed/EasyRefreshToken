using EasyRefreshToken.Models;
using System;

namespace EasyRefreshTokenTest.Mock
{
    public class MyRefreshToken : RefreshToken<User, Guid>
    {
        public MyRefreshToken()
        {
            DateCreated = DateTime.Now;
        }
        public DateTime DateCreated { get; set; }
    }
}
