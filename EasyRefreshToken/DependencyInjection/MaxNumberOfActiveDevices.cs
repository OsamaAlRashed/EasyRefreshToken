using EasyRefreshToken.DependencyInjection.Enums;
using System;
using System.Collections.Generic;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Max Number Of Active Devices options
    /// </summary>
    public class MaxNumberOfActiveDevices
    {
        internal int? GlobalLimit { get; set; }
        internal Dictionary<Type, int> LimitPerType { get; set; }
        internal ValueTuple<string, Dictionary<object, int>> LimitPerProperty { get; set; }
        internal MaxNumberOfActiveDevicesType Type { get; set; }
        private MaxNumberOfActiveDevices() { }

        /// <summary>
        /// Configures number of active devices by global limit.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns>New Instance from MaxNumberOfActiveDevices</returns>
        public static MaxNumberOfActiveDevices Configure(int limit)
        {
            if(limit <= 0)
                throw new ArgumentOutOfRangeException("limit");

            var result = new MaxNumberOfActiveDevices
            {
                Type = MaxNumberOfActiveDevicesType.GlobalLimit,
                GlobalLimit = limit
            };

            return result;
        }

        /// <summary>
        /// Configures number of active devices by user type (TPT).
        /// </summary>
        /// <param name="pairs">ValueTuble that present every type with limit it.</param>
        /// <returns>return new instance from <code>MaxNumberOfActiveDevices</code></returns>
        public static MaxNumberOfActiveDevices Configure(params (Type type, int limit)[] pairs)
        {
            var result = new MaxNumberOfActiveDevices
            {
                Type = MaxNumberOfActiveDevicesType.LimitPerType,
                LimitPerType = new Dictionary<Type, int>()
            };
            foreach (var (type, limit) in pairs)
            {
                if(limit <= 0)
                    throw new ArgumentOutOfRangeException("limit");
                result.LimitPerType[type] = limit;
            }

            return result;
        }

        /// <summary>
        /// Configures number of active devices by user type (Property).
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static MaxNumberOfActiveDevices Configure(string propName, params (object value, int limit)[] pairs)
        {
            if (propName == null)
                throw new ArgumentNullException("property name can not be null");

            var result = new MaxNumberOfActiveDevices
            {
                Type = MaxNumberOfActiveDevicesType.LimitPerProperty,
                LimitPerProperty = new ValueTuple<string, Dictionary<object, int>>(propName, new())
            };
            foreach (var (value, limit) in pairs)
            {
                if (limit <= 0)
                    throw new ArgumentOutOfRangeException("limit");
                result.LimitPerProperty.Item2[value] = limit;
            }

            return result;
        }
    }
}
