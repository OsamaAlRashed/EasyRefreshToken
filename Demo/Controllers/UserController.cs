using Demo.Models;
using Demo.ViewModels;
using EasyRefreshToken.TokenService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenService tokenService;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateUser()
        {
            var user = new User
            {
                UserName = "User" + GetNumberToken(),
                Email = "user" + GetNumberToken() + "@email.com"
            };
            var password = "demo";
            var result = await userManager.CreateAsync(user, password);

            return Ok(new { user.Id, user.UserName , password});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            var user = await userManager.FindByNameAsync(vm.UserName);
            if (user == null)
                return BadRequest("User Not Found");
            await signInManager.CheckPasswordSignInAsync(user, vm.Password, false);

            // must generate access token and some logic ...
            //

            // add refresh token
            var token = await tokenService.OnLogin(user.Id);

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string token)
        {
            // remove refresh token
            var result = await tokenService.OnLogout(token);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Clear(Guid? userId, bool onlyExpired)
        {
            bool result = true;
            if(userId == null)
            {
                if (onlyExpired)
                {
                    result = await tokenService.ClearExpired();
                }
                else
                {
                    result = await tokenService.Clear();
                }
            }
            else
            {
                if (onlyExpired)
                {
                    result = await tokenService.ClearExpired(userId);
                }
                else
                {
                    result = await tokenService.Clear(userId);
                }
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(Guid userId, string token)
        {
            var newRefershToken = await tokenService.OnAccessTokenExpired(userId, token);
            return Ok(newRefershToken);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(Guid userId)
        {
            var result = await tokenService.OnChangePassword(userId);
            return Ok(result);
        }


        //
        private static string GetNumberToken(int size = 4)
        {
            Random random = new();
            var token = "";
            int c = 0;
            while (c < size)
            {
                int x = random.Next(0, 9);
                token += x.ToString();
                c++;
            }
            return token;
        }
    }
}
