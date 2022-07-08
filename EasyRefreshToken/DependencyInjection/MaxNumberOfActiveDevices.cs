using EasyRefreshToken.DependencyInjection.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Max Number Of Active Devices options
    /// </summary>
    public class MaxNumberOfActiveDevices
    {
        internal int? globalLimit;
        internal Dictionary<Type, int> limitPerType;
        internal ValueTuple<string, Dictionary<object, int>> limitPerProperty;
        internal MaxNumberOfActiveDevicesType type;
        private MaxNumberOfActiveDevices() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static MaxNumberOfActiveDevices Config(int limit)
        {
            if(limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            var result = new MaxNumberOfActiveDevices();
            result.type = MaxNumberOfActiveDevicesType.GlobalLimit;
            result.globalLimit = limit;
            return result;
        }

        /// <summary>
        /// Configure custom max number of active devices option.
        /// </summary>
        /// <param name="pairs">VlaueTuble that present every type with limit it.</param>
        /// <returns>return new instance from <code>MaxNumberOfActiveDevices</code></returns>
        public static MaxNumberOfActiveDevices Config(params (Type type, int limit)[] pairs)
        {
            var result = new MaxNumberOfActiveDevices();
            result.type = MaxNumberOfActiveDevicesType.LimitPerType;
            result.limitPerType = new Dictionary<Type, int>();
            foreach (var pair in pairs)
            {
                if(pair.limit <= 0)
                    throw new ArgumentOutOfRangeException("limit");
                result.limitPerType[pair.type] = pair.limit;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static MaxNumberOfActiveDevices Config(string propName, params (object value, int limit)[] pairs)
        {
            if (propName == null)
                throw new ArgumentNullException("property name can not bo null");

            var result = new MaxNumberOfActiveDevices();
            result.type = MaxNumberOfActiveDevicesType.LimitPerProperty;
            result.limitPerProperty = new ValueTuple<string, Dictionary<object, int>>();
            result.limitPerProperty.Item1 = propName;
            result.limitPerProperty.Item2 = new();
            foreach (var pair in pairs)
            {
                if (pair.limit <= 0)
                    throw new ArgumentOutOfRangeException("limit");
                result.limitPerProperty.Item2[pair.value] = pair.limit;
            }
            return result;
        }
    }
}
