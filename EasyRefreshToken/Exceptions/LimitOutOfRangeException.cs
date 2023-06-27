using System;

namespace EasyRefreshToken.Exceptions
{
    public class LimitOutOfRangeException : Exception
    {
        public LimitOutOfRangeException() : base("The limit must be greater than zero.") { }
    }
}
