using EasyRefreshToken;
using Microsoft.AspNetCore.Identity;
using System;

namespace EasyRefreshToken.Tests.Data;

public class User : IdentityUser<Guid>, IUser<Guid>
{
    public UserType? UserType { get; set; }
}

public class SubUser1 : User { }

public class SubUser2 : User { }

public enum UserType
{
    Admin,
    Employee
}
