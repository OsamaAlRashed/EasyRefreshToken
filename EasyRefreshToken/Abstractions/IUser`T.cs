namespace EasyRefreshToken
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface IUser<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the entity.
        /// </summary>
        TKey Id { get; set; }
    }
}
