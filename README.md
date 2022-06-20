### **EasyRefreshToken 6.0.3**

**What's new??** 
- Remove IDbSetRefreshToken.
- Customize your refresh token entity.
- Support .Net 5

**Documentation** 

- Create your own class "MyRefreshToken" and add to it the properties you want and make it inherit from `RefreshToken<TUser, TKey>`
- If you do not want to add new features, you can skip the previous step.

- In AppDbContext Class:

  `public DbSet<MyRefreshToken> RefreshTokens { get; set; }`

- In Program Class: 

   `builder.Services.AddRefreshToken<AppDbContext, RefreshToken<TUser, TKey>, TUser, TKey>();`
or `builder.Services.AddRefreshToken<AppDbContext, MyRefreshToken, User, Guid>();`

- don't forget:
  `Add-Migration`
  `Update-Database`

- now you can use **ITokenService<TKey>** that contains:

  - `OnLogin`
  - `OnLogout`
  - `OnAccessTokenExpired`
  - `OnChangePassword`
  - `Clear`
  - `ClearExpired`

- and you can control with many options:

  - `MaxNumberOfActiveDevices`
  - `TokenExpiredDays`
  - `PreventingLoginWhenAccessToMaxNumberOfActiveDevices`
  - `TokenGenerationMethod`
  - `OnChangePasswordBehavior`

- Note: when change on options, I highly recommend cleaning the table by `Clear`

- Enjoy ... 
