using System;

namespace EasyRefreshToken.EFCore
{
    /// <summary>
    /// Represents a refresh token entity.
    /// </summary>
    /// <typeparam name="TUser">The type of the associated user entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class RefreshToken<TUser, TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the refresh token entity.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the refresh token string.
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the refresh token.
        /// </summary>
        public virtual DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Gets or sets the associated user entity.
        /// </summary>
        public virtual TUser User { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated user.
        /// </summary>
        public virtual TKey UserId { get; set; }
    }
}
