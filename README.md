### **EasyRefreshToken 1.0**

- .Net 7: 7.1.0
- .Net 6: 6.1.0
- .Net 5: 5.1.0

**What's new??** 
- Remove dependencies on microsoft identity.
- Add IUser intreface.

**Migrate to _.1.0**
- You must make `TUser` inherit from `IUser`.
- Change TokenService namespace to Service.

**Documentation** 

- Create your own class "MyRefreshToken" and add to it the properties you want and make it inherit from `RefreshToken<TUser, TKey>`
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

- Now you can use **ITokenService<TKey>** that contains:

  - `OnLogin`
  - `OnLogout`
  - `OnAccessTokenExpired`
  - `OnChangePassword`
  - `Clear`
  - `ClearExpired`

- And you can control with many options:

  - `MaxNumberOfActiveDevices`
  - `TokenExpiredDays`
  - `PreventingLoginWhenAccessToMaxNumberOfActiveDevices`
  - `TokenGenerationMethod`
  - `OnChangePasswordBehavior`
  - `SaveChanges`

- For `MaxNumberOfActiveDevices` use `MaxNumberOfActiveDevices.Config()`.
   
- Note: when change on options, I highly recommend cleaning the table by `Clear`

- Enjoy ... 
