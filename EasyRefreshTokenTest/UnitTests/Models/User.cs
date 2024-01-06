namespace EasyRefreshToken.Tests.UnitTests.Models;

public class User(int id) : IUser<int>
{
    public int Id { get; set; } = id;
    public string FullName { get; set; } = string.Empty;
}
