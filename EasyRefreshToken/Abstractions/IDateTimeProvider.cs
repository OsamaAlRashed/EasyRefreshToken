using System;

namespace EasyRefreshToken.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Now { get; }
    }
}
