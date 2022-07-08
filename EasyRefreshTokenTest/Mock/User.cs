using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EasyRefreshTokenTest.Mock
{
    public class User : IdentityUser<Guid>
    {
        public UserType? UserType { get; set; }
    }

    public enum UserType
    {
        Admin,
        Employee
    }

}
