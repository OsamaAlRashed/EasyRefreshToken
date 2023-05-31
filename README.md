### **EasyRefreshToken 1.2**


  <table>
  <tbody>

  <tr> <td> 
    <a href="https://www.nuget.org/packages/EasyRefreshToken/">
      <img alt="Nuget" src="https://img.shields.io/nuget/dt/EasyRefreshToken?color=blue&label=EasyRefreshToken&logo=nuget&style=flate">
    </a>
  </td> 
  </tr>
    
  </tbody>
  <table>

- .Net 7: 7.1.2
- .Net 6: 6.1.2
- .Net 5: 5.1.2

**Documentation** 
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

- Enjoy ... 
