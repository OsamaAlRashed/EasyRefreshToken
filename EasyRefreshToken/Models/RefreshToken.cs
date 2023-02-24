using System;

namespace EasyRefreshToken.Models
{
    /// <summary>
    /// The entity that represent refresh token
    /// </summary>
    /// <typeparam name="TUser">User</typeparam>
    /// <typeparam name="TKey">Key</typeparam>
    public class RefreshToken<TUser, TKey>
    {
        /// <summary>
        /// Primary key of entity
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Expired date of token
        /// </summary>
        public virtual DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Identity user
        /// </summary>
        public virtual TUser User { get; set; }

        /// <summary>
        /// Forgien key
        /// </summary>
        public virtual TKey UserId { get; set; }
    }
}
