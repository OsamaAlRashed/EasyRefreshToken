﻿using EasyRefreshToken;
using Microsoft.AspNetCore.Identity;
using System;

namespace EasyRefreshTokenTest.Mocks;

public class User : IdentityUser<Guid>, IUser<Guid>
{
    public UserType? UserType { get; set; }
}

public enum UserType
{
    Admin,
    Employee
}
