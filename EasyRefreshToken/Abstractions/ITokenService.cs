using System.Threading.Tasks;
using EasyRefreshToken.Commons;

namespace EasyRefreshToken
{
    /// <summary>
    /// Contains common methods to deal with refresh tokens.
    /// </summary>
    /// <typeparam name="TKey">The type of the user's key.</typeparam>
    public interface ITokenService<TKey>
    {
        /// <summary>
        /// Adds a new refresh token for the specified user upon login.
        /// </summary>
        /// <param name="userId">The current user's identifier.</param>
        /// <returns>A <see cref="TokenResult"/> object containing the result of the operation.</returns>
        Task<TokenResult> OnLoginAsync(TKey userId);

        /// <summary>
        /// Deletes a refresh token for the user that owns this token upon logout.
        /// </summary>
        /// <param name="token">The refresh token to delete.</param>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> OnLogoutAsync(string token);

        /// <summary>
        /// Updates the refresh token for the specified user when the current access token has expired.
        /// </summary>
        /// <param name="userId">The specified user's identifier.</param>
        /// <param name="oldToken">The expired refresh token.</param>
        /// <param name="renewTheToken">Keep the same token with new life</param>
        /// <returns>A <see cref="TokenResult"/> object containing the result of the operation.</returns>
        Task<TokenResult> OnAccessTokenExpiredAsync(TKey userId, string oldToken, bool renewTheToken = false);

        /// <summary>
        /// This method should be called when changing the user's password.
        /// </summary>
        /// <param name="userId">The current user's identifier.</param>
        /// <returns>It depends on the OnChangePasswordBehavior option.</returns>
        Task<string> OnChangePasswordAsync(TKey userId);

        /// <summary>
        /// Clears all token entities.
        /// </summary>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> ClearAsync();

        /// <summary>
        /// Clears expired token entities.
        /// </summary>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> ClearExpiredAsync();

        /// <summary>
        /// Clears the token entities for the specified user.
        /// </summary>
        /// <param name="userId">The specified user's identifier.</param>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> ClearAsync(TKey userId);

        /// <summary>
        /// Clears the expired token entities for the specified user.
        /// </summary>
        /// <param name="userId">The specified user's identifier.</param>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> ClearExpiredAsync(TKey userId);

        /// <summary>
        /// Blocks a user, adding them to a blacklist and preventing them from obtaining a new token.
        /// Please note that the blacklist is an in-memory list.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> BlockAsync(TKey userId);

        /// <summary>
        /// Unblocks a user, removing them from the blacklist and allowing them to obtain a new token.
        /// Please note that the blacklist is an in-memory list.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>A Boolean value indicating whether the operation succeeded.</returns>
        Task<bool> UnblockAsync(TKey userId);
    }

}
