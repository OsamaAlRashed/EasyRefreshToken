using System;

namespace EasyRefreshToken.Exceptions
{
    public class PropertyNameNullException : Exception
    {
        public PropertyNameNullException() : base("The property name cannot be null.") { }
    }
}
