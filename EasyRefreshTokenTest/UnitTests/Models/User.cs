namespace EasyRefreshToken.Tests.UnitTests.Models;

public class User : IUser<int>
{
    public int Id { get; set; }
    public string FullName { get; set; }
}
