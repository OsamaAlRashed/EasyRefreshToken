using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.Utils
{
    /// <summary>
    /// Helpers methods
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Default token generater 
        /// </summary>
        /// <returns>unique refresh token</returns>
        internal static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Get value from property name by reflection 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static object GetPropertyValue(object user, string propName)
        {
            var prop = user.GetType().GetProperties().Where(x => x.Name.ToLower() == propName.ToLower()).FirstOrDefault();
            if (prop == null)
                throw new Exception("property name not exist in the given object");

            return prop.GetValue(user);
        }
    }
}
