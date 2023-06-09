# EasyRefreshToken

[![NuGet Package](https://img.shields.io/nuget/v/EasyRefreshToken.svg)](https://www.nuget.org/packages/EasyRefreshToken)
[![License](https://img.shields.io/github/license/OsamaAlRashed/EasyRefreshToken.svg)](https://github.com/OsamaAlRashed/EasyRefreshToken/blob/main/LICENSE)
[![downloads](https://img.shields.io/nuget/dt/EasyRefreshToken)](https://www.nuget.org/packages/EasyRefreshToken)

## Overview

EasyRefreshToken is a .NET library that provides an easy-to-use token service for refreshing access tokens in ASP.NET Core applications.
It offers a flexible and customizable solution for managing refresh tokens, token expiration, preventing simultaneous logins, and more.

With EasyRefreshToken, you can integrate refresh token functionality into your ASP.NET Core applications without the need to write complex code or manage token-related logic manually. It simplifies the process of refreshing access tokens and ensures a secure and seamless user experience.

## Features

- Automatic management of refresh tokens and token expiration.
- Flexible options for configuring token behavior, such as token expiration duration and preventing simultaneous logins.
- Support for different storage mechanisms, including in-memory and Entity Framework Core.
- Customizable token generation methods.
- Integration with ASP.NET Core Identity.
- Lightweight and minimal dependencies.

## Getting Started

Follow these steps to get started with EasyRefreshToken in your ASP.NET Core application:

### 1. Installation

Install the EasyRefreshToken package from NuGet by using the following command:

```sh
// In-Memory Refrsh Token 
dotnet add package EasyRefreshToken.InMemory

// EF Core Cache Refresh Token
dotnet add package EasyRefreshToken.EFCore
```

### 2. Configuration

In the ConfigureServices method of your Startup.cs / Program.cs file, add the EasyRefreshToken services and configure the options:


```cs
using EasyRefreshToken.InMemory;

// ...

// Add EasyRefreshToken services
builder.Services.AddInMemoryRefreshToken(options =>
{
    // Configure the options as needed
    options.TokenExpiredDays = 7;
    options.PreventingLoginWhenAccessToMaxNumberOfActiveDevices = true;
    // ...
});

// Other service configurations...

```

### 3. Usage

Inject the ITokenService into your controller or service where you want to use the token service functionality:

```cs
using EasyRefreshToken;

// ...

private readonly ITokenService<string> _tokenService;

public MyRepository(ITokenService<string> tokenService)
{
    _tokenService = tokenService;
}

public async Task<LoginResponse> Login(LoginDto dto)
{
    // Login logic ...
   
    TokenResult result = await _tokenService.OnLoginAsync(userId);

    if (result.IsSucceeded)
    {
        // Return the new token to the client
    }
    else
    {
        // Handle the error
    }
}
```

### 4. Documentation
- Make your `TUser` inherit from `IUser`.
- Create your own class `MyRefreshToken` and add to it the properties you want and make it inherit from `RefreshToken<TUser, TKey>`
- If you do not want to add new features, you can skip the previous step.

- In AppDbContext Class:
   `public DbSet<RefreshToken<TUser, TKey>> RefreshTokens { get; set; }`
or `public DbSet<MyRefreshToken> RefreshTokens { get; set; }`

- In Program Class: 

   `builder.Services.AddRefreshToken<AppDbContext, RefreshToken<TUser, TKey>, TUser, TKey>();`
or `builder.Services.AddRefreshToken<AppDbContext, MyRefreshToken, TUser, TKey>();`

- Don't forget:
  `Add-Migration`
  `Update-Database`

- Now you can use `ITokenService<TKey>` that contains:

  - `OnLoginAsync`
  - `OnLogoutAsync`
  - `OnAccessTokenExpiredAsync`
  - `OnChangePasswordAsync`
  - `ClearAsync`
  - `ClearExpiredAsync`

- And you can control with many options:

  - `MaxNumberOfActiveDevices`
  - `TokenExpiredDays`
  - `PreventingLoginWhenAccessToMaxNumberOfActiveDevices`
  - `TokenGenerationMethod`
  - `OnChangePasswordBehavior`
  - `SaveChanges`

- You can specifies a life time of the Service (the default is Scoped).
    
- For `MaxNumberOfActiveDevices` use `MaxNumberOfActiveDevices.Configure()`.
   
- Note: when change on options, I highly recommend cleaning the table by `Clear`

### 5. Contributing
Contributions to EasyRefreshToken are welcome and encouraged! If you find any bugs, issues, or have feature requests, please open a new issue on the GitHub repository. If you would like to contribute code, please fork the repository, make your changes, and submit a pull request.

### 6. License
EasyRefreshToken is licensed under the MIT License.
