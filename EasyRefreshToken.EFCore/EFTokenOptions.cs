using EasyRefreshToken.DependencyInjection;

namespace EasyRefreshToken.EFCore
{
    /// <summary>
    /// Options to control the behavior of the token service in an Entity Framework context.
    /// </summary>
    public class EFTokenOptions : RefreshTokenOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether changes should be automatically saved to the database. The default value is true.
        /// </summary>
        public bool SaveChanges { get; set; } = true;
    }
}
