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
- Determine the number of tokens for each user according to the global limit, its type, or one of its properties.
- Support for different storage mechanisms, including in-memory and Databases (By Entity Framework Core).
- Customizable the Repository Implementation.
- Customizable token generation methods.
- Integration with ASP.NET Core Identity.
- Lightweight and minimal dependencies.
- Support .Net5, .Net6, and .Net7

## Getting Started

Follow these steps to get started with EasyRefreshToken in your ASP.NET Core application:

### 1. Installation

Install the EasyRefreshToken package from NuGet by using the following command:

```sh
// In-Memory Refresh Token 
dotnet add package EasyRefreshToken.InMemory

// EF Core Cache Refresh Token
dotnet add package EasyRefreshToken.EFCore

// Custom Refresh Token
dotnet add package EasyRefreshToken
```

### 2. Configuration

In the ConfigureServices method of your Startup.cs / Program.cs file, add the EasyRefreshToken services, and configure the options:


```cs
using EasyRefreshToken.InMemory;

// ...

// Add EasyRefreshToken services
builder.Services.AddInMemoryRefreshToken(options =>
{
    // Configure the options as needed
    options.TokenExpiredDays = 7;
    // ...
});

// Other service configurations...

```

### 3. Usage

Make your `TUser` inherit from `IUser`:

```cs
public class User: IUser { }
```

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
### 4. Contributing
Contributions to EasyRefreshToken are welcome and encouraged! If you find any bugs, or issues, or have feature requests, please open a new issue on the GitHub repository. If you would like to contribute code, please fork the repository, make your changes, and submit a pull request.

### 5. License
EasyRefreshToken is licensed under the MIT License.
