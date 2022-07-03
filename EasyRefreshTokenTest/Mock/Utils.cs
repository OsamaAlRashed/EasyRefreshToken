using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshTokenTest.Mock
{
    public class Utils
    {
        private readonly AppDbContext context;

        public Utils(AppDbContext context)
        {
            this.context = context;
        }

        private async Task<LoginVM> GenerateUserBase(int number = 0)
        {
            var user = new User();
            if(number == 1)
            {
                user = new SubUser1();
            }
            if (number == 2)
            {
                user = new SubUser2();
            }
            var userName = "User" + GetNumberToken();
            var password = "demo";
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new LoginVM { Id = user.Id, UserName = user.UserName, Password = password };
        }

        public async Task<LoginVM> GenerateUser()
         => await GenerateUserBase(0);
        public async Task<LoginVM> GenerateUserSubUser1()
         => await GenerateUserBase(1);
        public async Task<LoginVM> GenerateUserSubUser2()
         => await GenerateUserBase(2);

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
        public class LoginVM
        {
            public Guid Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
