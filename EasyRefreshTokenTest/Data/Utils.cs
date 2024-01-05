using System;
using System.Threading.Tasks;

namespace EasyRefreshToken.Tests.Data;

public class Utils
{
    private readonly AppDbContext context;

    public Utils(AppDbContext context)
    {
        this.context = context;
    }

    private async Task<LoginVM> GenerateUserBase(int number = 0, UserType? userType = null)
    {
        var user = new User
        {
            UserType = userType
        };

        if (number == 1)
        {
            user = new SubUser1();
        }
        if (number == 2)
        {
            user = new SubUser2();
        }

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new LoginVM { Id = user.Id };
    }

    public async Task<LoginVM> GenerateUser()
     => await GenerateUserBase(0);
    public async Task<LoginVM> GenerateUserSubUser1()
     => await GenerateUserBase(1);
    public async Task<LoginVM> GenerateUserSubUser2()
     => await GenerateUserBase(2);

    public async Task<LoginVM> GenerateAdmin()
     => await GenerateUserBase(0, UserType.Admin);

    public async Task<LoginVM> GenerateEmployee()
     => await GenerateUserBase(0, UserType.Employee);

    public class LoginVM
    {
        public Guid Id { get; set; }
    }
}
