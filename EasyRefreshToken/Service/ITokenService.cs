using EasyRefreshToken.Result;
using System.Threading.Tasks;

namespace EasyRefreshToken
{
    /// <summary>
    /// Contains common methods to deal with the refresh token.
    /// </summary>
    /// <typeparam name="TKey">The key of user</typeparam>
    public interface ITokenService<TKey>
    {
        /// <summary>
        /// Adds new token for a specified user
        /// </summary>
        /// <param name="userId">Current user Id</param>
        /// <returns>New refresh token</returns>
        Task<TokenResult> OnLoginAsync(TKey userId);

        /// <summary>
        /// Delete token
        /// </summary>
        /// <param name="token">The refresh token</param>
        /// <returns>Returns a Boolean value to determine if the operation succeeded</returns>
        Task<bool> OnLogoutAsync(string token);

        /// <summary>
        /// Updates the token for a specified user
        /// </summary>
        /// <param name="userId">Specified user Id</param>
        /// <param name="oldToken">Refresh token</param>
        /// <returns>New token</returns>
        Task<TokenResult> OnAccessTokenExpiredAsync(TKey userId, string oldToken);

        /// <summary>
        /// this method shoud be called by Change Password
        /// </summary>
        /// <param name="userId">current user Id</param>
        /// <returns>It depends on OnChangePasswordBehavior option</returns>
        Task<string> OnChangePasswordAsync(TKey userId);

        /// <summary>
        /// Clears the token entity
        /// </summary>
        /// <returns>Returns a Boolean value to determine if the operation succeeded</returns>
        Task<bool> ClearAsync();

        /// <summary>
        /// Clears the expired token in the entity
        /// </summary>
        /// <returns>Returns a Boolean value to determine if the operation succeeded</returns>
        Task<bool> ClearExpiredAsync();

        /// <summary>
        /// Clears the token entity for a specified user
        /// </summary>
        /// <param name="userId">Specified user Id</param>
        /// <returns>Returns a Boolean value to determine if the operation succeeded</returns>
        Task<bool> ClearAsync(TKey userId);
        /// <summary>
        /// Clears the expired token in the entity for a specified user
        /// </summary>
        /// <param name="userId">Specified user Id</param>
        /// <returns>Returns a Boolean value to determine if the operation succeeded</returns>
        Task<bool> ClearExpiredAsync(TKey userId);
    }
}
