### **EasyRefreshToken 6.0.8**

**What's new??** 
- Max number of active devices per user type (TPT).
- Max number of active devices per user property (this option uses Reflection, so may be slow!)

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

- for `MaxNumberOfActiveDevices` use `CustomMaxNumberOfActiveDevices.Config()`.
   
- Note: when change on options, I highly recommend cleaning the table by `Clear`

- Enjoy ... 
