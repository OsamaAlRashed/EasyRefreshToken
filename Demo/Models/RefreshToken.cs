using EasyRefreshToken.Models;

namespace Demo.Models;

public class RefreshToken : RefreshToken<User, Guid>
{
    public RefreshToken()
    {
        DateCreated = DateTime.Now.ToLocalTime();
    }
    public DateTime DateCreated { get; set; }
}