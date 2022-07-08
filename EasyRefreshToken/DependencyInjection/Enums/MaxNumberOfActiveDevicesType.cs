using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.DependencyInjection.Enums
{
    internal enum MaxNumberOfActiveDevicesType
    {
        GlobalLimit,
        LimitPerType,
        LimitPerProperty
    }
}
