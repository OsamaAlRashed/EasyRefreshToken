using System;

namespace EasyRefreshToken.Abstractions
{
    /// <summary>
    /// Provides the date time 
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the datetime now
        /// </summary>
        public DateTime Now { get; }
    }
}
