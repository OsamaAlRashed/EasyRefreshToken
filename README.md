### **EasyRefreshToken 6.0.2**

**What's new??** 
- More overload for Clear method and Support limited active devices.

**Documentation** 

- In AppDbContext Class:

  `public class AppDbContext :IdentityDbContext<User, IdentityRole<Guid>, Guid>, IDbSetRefreshToken<User, Guid>`

- in Program Class: 

  `builder.Services.AddRefreshToken<AppDbContext, User, Guid>();`

- don't forget:
  `Add-Migration`
  `Update-Database`

- now you can use **TokenService** that contains:

  - OnLogin
  - OnLogout
  - OnAccessTokenExpired
  - OnChangePassword
  - Clear

- and you can control with many options:

  - MaxNumberOfActiveDevices
  - TokenExpiredDays
  - PreventingLoginWhenAccessToMaxNumberOfActiveDevices
  - GenerateTokenMethod
  - TokenGenerationMethod

- note: when change on options, I highly recommend cleaning the table by `Clear`

- Enjoy ... 
