using System;
using System.Threading.Tasks;

namespace EasyRefreshToken.Abstractions;

/// <summary>
/// Contains common methods to manage the tokens
/// </summary>
/// <typeparam name="TUser">The type of the user's key.</typeparam>
/// <typeparam name="TKey">The type of the user</typeparam>
public interface ITokenRepository<TUser, TKey>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <param name="expiredDate"></param>
    /// <returns></returns>
    Task<string> Add(TKey userId, string token, DateTime? expiredDate);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<TUser?> GetById(TKey userId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> Delete();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> Delete(TKey userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> Delete(string token);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> DeleteExpired(TKey userId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> DeleteExpired();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<int> GetNumberActiveTokens(TKey userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string?> GetOldestToken(TKey userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> IsValidToken(TKey userId, string token);
}
