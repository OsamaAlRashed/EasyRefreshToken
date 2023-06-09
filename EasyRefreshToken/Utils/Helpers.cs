using System;
using System.Linq;
using System.Security.Cryptography;

namespace EasyRefreshToken.Utils
{
    internal static class Helpers
    {
        internal static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        internal static object GetPropertyValue(object @object, string propName)
        {
            var prop = @object.GetType().GetProperties().Where(x => x.Name.ToLower() == propName?.ToLower()).FirstOrDefault();
            return prop == null ? throw new ArgumentNullException("Property name not exist in the given object.") : prop.GetValue(@object);
        }
    }
}
