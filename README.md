EasyRefreshToken is a library to manage refresh token opreation Easy ..

- In AppDbContext Class:

`public class AppDbContext : public class AppDbContext : IDbSetRefreshToken<User, Guid>`



- in Program Class: 

  `builder.Services.AddRefreshToken<AppDbContext, User, Guid>();`

- now you can user **TokenService** that contains:
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
