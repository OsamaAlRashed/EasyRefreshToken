using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRefreshToken.Models
{
    /// <summary>
    /// Refresh token entity
    /// </summary>
    /// <typeparam name="TUser">User</typeparam>
    /// <typeparam name="TKey">Key</typeparam>
    public class RefreshToken<TUser, TKey>
    {
        /// <summary>
        /// Key of entity
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expired date of token
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Identity user
        /// </summary>
        public TUser User { get; set; }

        /// <summary>
        /// Forgien key
        /// </summary>
        public TKey UserId { get; set; }
    }
}
