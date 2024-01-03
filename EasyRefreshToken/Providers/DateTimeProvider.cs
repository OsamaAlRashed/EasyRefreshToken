using EasyRefreshToken.Abstractions;
using System;

namespace EasyRefreshToken.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Now => DateTime.UtcNow;
    }
}
