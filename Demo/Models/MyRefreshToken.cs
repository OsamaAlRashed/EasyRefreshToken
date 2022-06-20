using EasyRefreshToken.Models;

namespace Demo.Models
{
    public class MyRefreshToken : RefreshToken<User, Guid>
    {
        public MyRefreshToken()
        {
            DateCreated = DateTime.Now;
        }
        public DateTime DateCreated { get; set; }
    }
}
