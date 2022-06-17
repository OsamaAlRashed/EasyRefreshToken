**EasyRefreshToken 6.0.2**

What's new?? 
More overload for Clear method and Support limited active devices.

EasyRefreshToken is a library to manage refresh token operations with EF Core easy ..

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
  - MultiDevice
  - TokenExpiredDays
  - BlockLoginWhenActive
  - GenerateTokenMethod
  - OnChangePasswordBehavior
 
- Enjoy ... 
