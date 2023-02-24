using EasyRefreshToken.Result;
using System.Threading.Tasks;

namespace EasyRefreshToken.Service
{
    /// <summary>
    /// Service contains commonlly method to deal with refresh token 
    /// </summary>
    public interface ITokenService<TKey>
    {
        /// <summary>
        /// Adds new token for user
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <returns>new refresh token</returns>
        Task<TokenResult> OnLogin(TKey userId);

        /// <summary>
        /// Delete token
        /// </summary>
        /// <param name="token">current token</param>
        /// <returns>true if success, false if faild</returns>
        Task<bool> OnLogout(string token);

        /// <summary>
        /// Update refresh token
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <param name="oldToken">current token</param>
        /// <returns>new token</returns>
        Task<TokenResult> OnAccessTokenExpired(TKey userId, string oldToken);

        /// <summary>
        /// this method shoud be called by Change Password
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <returns>It depends on OnChangePasswordBehavior option</returns>
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
        /// <param name="userId">current user Id</param>
        /// <returns></returns>
        Task<bool> Clear(TKey userId);
        /// <summary>
        /// clear Expired token for user
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <returns></returns>
        Task<bool> ClearExpired(TKey userId);
    }
}
