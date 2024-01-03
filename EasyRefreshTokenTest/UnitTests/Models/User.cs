namespace EasyRefreshToken.Tests.UnitTests.Models;

public class User : IUser<int>
{
    public User(int id)
    {
        Id = id;
        FullName = string.Empty;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
}
