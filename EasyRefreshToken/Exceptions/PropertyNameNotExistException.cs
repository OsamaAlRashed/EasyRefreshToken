using System;

namespace EasyRefreshToken.Exceptions
{
    public class PropertyNameNotExistException : Exception
    {
        public PropertyNameNotExistException() : base("Property name not exist in the given object.") { }
    }
}
