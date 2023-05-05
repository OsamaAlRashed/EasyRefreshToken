using System;
using System.Linq;
using System.Security.Cryptography;

namespace EasyRefreshToken.Utils
{
    /// <summary>
    /// Represents Helper methods
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Generates random string as base 64
        /// </summary>
        /// <returns>Unique refresh token</returns>
        internal static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Gets a value from property name by reflection 
        /// </summary>
        /// <param name="object">The object that contains property</param>
        /// <param name="propName">Property's name</param>
        /// <returns>A value of a given property as object</returns>
        /// <exception cref="Exception"></exception>
        internal static object GetPropertyValue(object @object, string propName)
        {
            var prop = @object.GetType().GetProperties().Where(x => x.Name.ToLower() == propName?.ToLower()).FirstOrDefault();
            return prop == null ? throw new ArgumentNullException("Property name not exist in the given object.") : prop.GetValue(@object);
        }
    }
}
