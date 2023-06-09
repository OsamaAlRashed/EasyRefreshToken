using EasyRefreshToken.DependencyInjection.Enums;
using System;
using System.Collections.Generic;

namespace EasyRefreshToken
{
    /// <summary>
    /// Represents the options for configuring the maximum number of active devices.
    /// </summary>
    public class MaxNumberOfActiveDevices
    {
        internal int GlobalLimit { get; set; } = int.MaxValue;
        internal Dictionary<Type, int> LimitPerType { get; set; }
        internal (string PropertyName, Dictionary<object, int> ValuePerLimit) LimitPerProperty { get; set; }
        internal MaxNumberOfActiveDevicesType Type { get; set; }
        private MaxNumberOfActiveDevices() { }

        /// <summary>
        /// Configures the maximum number of active devices using a global limit.
        /// </summary>
        /// <param name="limit">The maximum number of active devices allowed globally.</param>
        /// <returns>A new instance of <see cref="MaxNumberOfActiveDevices"/>.</returns>
        public static MaxNumberOfActiveDevices Configure(int limit)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException(nameof(limit), "The limit must be greater than zero.");

            var result = new MaxNumberOfActiveDevices
            {
                Type = MaxNumberOfActiveDevicesType.GlobalLimit,
                GlobalLimit = limit
            };

            return result;
        }

        /// <summary>
        /// Configures the maximum number of active devices based on user type (Table-Per-Type).
        /// </summary>
        /// <param name="pairs">A collection of tuples representing each user type along with its associated limit.</param>
        /// <returns>A new instance of <see cref="MaxNumberOfActiveDevices"/>.</returns>
        public static MaxNumberOfActiveDevices Configure(params (Type type, int limit)[] pairs)
        {
            var result = new MaxNumberOfActiveDevices
            {
                Type = MaxNumberOfActiveDevicesType.LimitPerType,
                LimitPerType = new Dictionary<Type, int>()
            };

            foreach (var (type, limit) in pairs)
            {
                if (limit <= 0)
                    throw new ArgumentOutOfRangeException(nameof(limit), "The limit must be greater than zero.");
                result.LimitPerType[type] = limit;
            }

            return result;
        }

        /// <summary>
        /// Configures the maximum number of active devices based on user type (Property).
        /// </summary>
        /// <param name="propertyName">The name of the user property to use for limiting active devices.</param>
        /// <param name="pairs">A collection of tuples representing each property value along with its associated limit.</param>
        /// <returns>A new instance of <see cref="MaxNumberOfActiveDevices"/>.</returns>
        public static MaxNumberOfActiveDevices Configure(string propertyName, params (object value, int limit)[] pairs)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName), "The property name cannot be null.");

            var result = new MaxNumberOfActiveDevices
            {
                Type = MaxNumberOfActiveDevicesType.LimitPerProperty,
                LimitPerProperty = new ValueTuple<string, Dictionary<object, int>>(propertyName, new())
            };

            foreach (var (value, limit) in pairs)
            {
                if (limit <= 0)
                    throw new ArgumentOutOfRangeException(nameof(limit), "The limit must be greater than zero.");
                result.LimitPerProperty.Item2[value] = limit;
            }

            return result;
        }
    }
}
