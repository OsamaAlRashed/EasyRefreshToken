using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRefreshToken.Models
{
    /// <summary>
    /// refresh token table
    /// </summary>
    /// <typeparam name="TUser">User</typeparam>
    /// <typeparam name="TKey">Key</typeparam>
    public class RefreshToken<TUser, TKey>
    {
        /// <summary>
        /// Key of Table
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Refresh Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expired Date of Token
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Identity User
        /// </summary>
        public TUser User { get; set; }

        /// <summary>
        /// Forgien Key
        /// </summary>
        public TKey UserId { get; set; }
    }
}
