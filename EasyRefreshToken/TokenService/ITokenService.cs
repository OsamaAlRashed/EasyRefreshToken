using System.Threading.Tasks;

namespace EasyRefreshToken.TokenService
{
    /// <summary>
    /// Service contains commonlly method to 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// this method shoud be called by login.
        /// </summary>
        /// <param name="userId">user' Id for cuurent user</param>
        /// <returns>new refresh token</returns>
        Task<string> OnLogin<TKey>(TKey userId);

        /// <summary>
        /// this method shoud be called by logout.
        /// </summary>
        /// <param name="token">current user token</param>
        /// <returns>true if success, false if faild</returns>
        Task<bool> OnLogout(string token);

        /// <summary>
        /// this method shoud be called by Refresh Access Token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldToken"></param>
        /// <returns></returns>
        Task<string> OnAccessTokenExpired<TKey>(TKey userId, string oldToken);

        /// <summary>
        /// this method shoud be called by Change Password
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> OnChangePassword<TKey>(TKey userId);

        /// <summary>
        /// clear refresh token table
        /// </summary>
        /// <returns>true if success, false if faild</returns>
        Task<bool> Clear();
    }
}
