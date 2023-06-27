using System;
using System.Threading.Tasks;

namespace EasyRefreshToken.Abstractions
{
    /// <summary>
    /// Contains common methods to manage the tokens
    /// </summary>
    /// <typeparam name="TUser">The type of the user's key.</typeparam>
    /// <typeparam name="TKey">The type of the user</typeparam>
    public interface ITokenRepository<TUser, TKey>
    {
        /// <summary>
        /// Adds a new token.
        /// <param name="userId">The user's key.</param>
        /// <param name="token">The token to add.</param>
        /// <param name="expiredDate">The expiration date of the token (optional).</param>
        /// <returns>The new token.</returns>
        Task<string> AddAsync(TKey userId, string token, DateTime? expiredDate);

        /// <summary>
        /// Retrieves a user by Id.
        /// </summary>
        /// Retrieves a user by their key.
        /// <returns>A user as an instance of TUser, or null if not found.</returns>
        Task<TUser?> GetByIdAsync(TKey userId);

        /// <summary>
        /// Deletes all tokens.
        /// </summary>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Deletes all tokens for a specific user.
        /// </summary>
        /// <param name="userId">The user's key.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<bool> DeleteAsync(TKey userId);

        /// <summary>
        /// Deletes a token by its value.
        /// </summary>
        /// <param name="token">The token to delete.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<bool> DeleteAsync(string token);

        /// <summary>
        /// Deletes expired tokens for a specific user.
        /// </summary>
        /// <param name="userId">The user's key.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<bool> DeleteExpiredAsync(TKey userId);

        /// <summary>
        /// Deletes all expired tokens from the repository.
        /// </summary>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        Task<bool> DeleteExpiredAsync();

        /// <summary>
        /// Retrieves the number of active tokens for a specific user.
        /// </summary>
        /// <param name="userId">The user's key.</param>
        /// <returns>The number of active tokens.</returns>
        Task<int> GetNumberOfActiveTokensAsync(TKey userId);

        /// <summary>
        /// Retrieves the oldest token for a specific user.
        /// </summary>
        /// <param name="userId">The user's key.</param>
        /// <returns>Oldest token as a string, or null if not found.</returns>
        Task<string?> GetOldestTokenAsync(TKey userId);

        /// <summary>
        /// Checks if a token is valid for a specific user.
        /// </summary>
        /// <param name="userId">The user's key.</param>
        /// <param name="token">The token to validate.</param>
        /// <returns>A boolean value indicating whether the token is valid.</returns>
        Task<bool> IsValidTokenAsync(TKey userId, string token);
    }
}
