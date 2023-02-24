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
        internal int? globalLimit;
        internal Dictionary<Type, int> limitPerType;
        internal ValueTuple<string, Dictionary<object, int>> limitPerProperty;
        internal MaxNumberOfActiveDevicesType type;
        private MaxNumberOfActiveDevices() { }

        /// <summary>
        /// Configures number of active devices by global limit.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns>New Instance from MaxNumberOfActiveDevices</returns>
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
        /// Configures number of active devices by user type (TPT).
        /// </summary>
        /// <param name="pairs">ValueTuble that present every type with limit it.</param>
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
        /// Configures number of active devices by user type (Property).
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
