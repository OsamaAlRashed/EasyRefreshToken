namespace EasyRefreshToken.Abstractions
{
    /// <summary>
    /// Represents User Entity
    /// </summary>
    /// <typeparam name="TKey">Represents the key of the entity</typeparam>
    public interface IUser<TKey>
    {
        /// <summary>
        /// Represents the primary key of the entity
        /// </summary>
        public TKey Id { get; set; }
    }
}
