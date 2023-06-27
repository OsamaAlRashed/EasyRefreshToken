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

### 4. Documentation

**Common Documentation:**

- You should make your `TUser` inherit from `IUser`.
- Use `IUser<TKey>` if the key of your User is not `string` 

- Use `ITokenService<TKey>` that contains:

  - `Task<TokenResult> OnLoginAsync(TKey userId)`: Adds a new refresh token for the specified user upon login.
  - `Task<bool> OnLogoutAsync(string token)`: Deletes a refresh token for the user that owns this token upon logout.
  - `Task<TokenResult> OnAccessTokenExpiredAsync(TKey userId, string oldToken)`: Updates the refresh token for the specified user when the current access token has expired.
  - `Task<string> OnChangePasswordAsync(TKey userId)`: This method should be called when changing the user's password.
  - `Task<bool> ClearAsync()`: Clears all token entities.
  - `Task<bool> ClearExpiredAsync()`: Clears expired token entities.
  - `Task<bool> ClearAsync(TKey userId)`: Clears the token entities for the specified user.
  - `Task<bool> ClearExpiredAsync(TKey userId)`: Clears the expired token entities for the specified user.
  - `Task<bool> BlockAsync(TKey userId)`: Blocks a user, adding them to a blacklist and preventing them from obtaining a new token.
  - `Task<bool> UnblockAsync(TKey userId)`: Unblocks a user, removing them from the blacklist and allowing them to obtain a new token.

- Configure your service by options:
  - `int? TokenExpiredDays`:Sets the number of days until the token expires. If set to null, the token will never expire. The default value is 7 days.
  - `bool PreventingLoginWhenAccessToMaxNumberOfActiveDevices`: Sets a value indicating whether to prevent login operation when the maximum number of active devices is reached. If set to true and there is a valid token, login will be prevented. If set to false, the old token will be removed and a new token will be added. The default value is true.
  - `Func<string> TokenGenerationMethod`: Sets the method used for generating tokens.
  - `OnChangePasswordBehavior OnChangePasswordBehavior`: Sets the behavior of the OnChangePassword method. The default value is OnChangePasswordBehavior.DeleteAllTokens.
  - `MaxNumberOfActiveDevices MaxNumberOfActiveDevices`: Sets the maximum number of active devices per user type. If a type is not specified, the default value is `int.MaxValue`

**MaxNumberOfActiveDevices**
 - Global Limit: `MaxNumberOfActiveDevices.Configure(int.MaxValue)`
 - Limit Per Type: `MaxNumberOfActiveDevices.Configure((typeof(Admin), 1), (typeof(Employee), 2))`
 - Limit Per Property: `MaxNumberOfActiveDevices.Configure("UserType", (UserType.Admin, 1), (UserType.Employee, 2))`

**Notes**

- You can specify a lifetime of the Service (the default is Scoped).
- For `MaxNumberOfActiveDevices` use `MaxNumberOfActiveDevices.Configure()`.
- Note: when changing options, I highly recommend cleaning the table with `Clear`

**EF Core RefreshToken:**
- Create your own class `MyRefreshToken` and add to it the properties you want and make it inherit from `RefreshToken<TUser, TKey>`

- If you do not want to add new features, you can skip the previous step.

- In AppDbContext Class:
   `public DbSet<RefreshToken<TUser, TKey>> RefreshTokens { get; set; }`
or `public DbSet<MyRefreshToken> RefreshTokens { get; set; }`

- In Program Class: 
   `builder.Services.AddEFCoreRefreshToken<AppDbContext, RefreshToken<TUser, TKey>, TUser, TKey>();`
or `builder.Services.AddEFCoreRefreshToken<AppDbContext, MyRefreshToken, TUser, TKey>();`

- You have an additional option `bool SaveChanges`: Sets a value indicating whether changes should be automatically saved to the database. The default value is true.

- Don't forget:
  `Add-Migration`
  `Update-Database`
  
**In-Memory RefreshToken:**

- In Program Class: 
   `builder.Services.AddInMemoryRefreshToken<TUser, TKey>();`
   
- You have an additional option `Func<IServiceProvider, TKey, TUser>? GetUserById`: Sets a function to retrieve a user by their ID.

**Warning!!**: You must set `Func<IServiceProvider, TKey, TUser>? GetUserById` option, if you use `LimitPerProperty` or `LimitPerType`.

**Custom RefreshToken**

- Create your custom repository `TRepository`.
  
- In Program Class:
  `services.AddCustomRefreshToken<TUser, TKey, TRepository>()`;

### 5. Contributing
Contributions to EasyRefreshToken are welcome and encouraged! If you find any bugs, or issues, or have feature requests, please open a new issue on the GitHub repository. If you would like to contribute code, please fork the repository, make your changes, and submit a pull request.

### 6. License
EasyRefreshToken is licensed under the MIT License.
