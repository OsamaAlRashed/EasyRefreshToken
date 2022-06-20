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
    public static class Helpers
    {
        /// <summary>
        /// default generation
        /// </summary>
        /// <returns>unique refresh token</returns>
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
