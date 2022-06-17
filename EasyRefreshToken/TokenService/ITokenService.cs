using System;
using System.Threading.Tasks;

namespace EasyRefreshToken.TokenService
{

    /// <summary>
    /// Service contains commonlly method to 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ITokenService<in TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// this method shoud be called by login.
        /// </summary>
        /// <param name="userId">user' Id for cuurent user</param>
        /// <returns>new refresh token</returns>
        Task<string> OnLogin(TKey userId);

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
        Task<string> OnAccessTokenExpired(TKey userId, string oldToken);

        /// <summary>
        /// this method shoud be called by Change Password
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> OnChangePassword(TKey userId);

        /// <summary>
        /// clear token table
        /// </summary>
        /// <returns>true if success, false if faild</returns>
        Task<bool> Clear();

        /// <summary>
        /// clear Expired token table
        /// </summary>
        /// <returns>true if success, false if faild</returns>
        Task<bool> ClearExpired();

        /// <summary>
        /// clear token for user
        /// </summary>
        /// <returns>true if success, false if faild</returns>
        Task<bool> Clear(TKey userId);

        /// <summary>
        /// clear Expired token for user
        /// </summary>
        /// <returns>true if success, false if faild</returns>
        Task<bool> ClearExpired(TKey userId);
    }
}
