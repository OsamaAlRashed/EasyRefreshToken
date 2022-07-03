using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Custom Max Number Of Active Devices options
    /// </summary>
    public class CustomMaxNumberOfActiveDevices : Dictionary<Type, int> 
    {
        private CustomMaxNumberOfActiveDevices() { }

        /// <summary>
        /// Configure custom max number of active devices option.
        /// </summary>
        /// <param name="pairs">VlaueTuble that present every type with limit it.</param>
        /// <returns>return new instance from <code>CustomMaxNumberOfActiveDevices</code></returns>
        public static CustomMaxNumberOfActiveDevices Config(params (Type type, int limit)[] pairs)
        {
            var result = new CustomMaxNumberOfActiveDevices();

            foreach (var pair in pairs)
            {
                if (!result.ContainsKey(pair.type))
                {
                    result[pair.type] = pair.limit;
                }
            }
            return result;
        }
    }
}
