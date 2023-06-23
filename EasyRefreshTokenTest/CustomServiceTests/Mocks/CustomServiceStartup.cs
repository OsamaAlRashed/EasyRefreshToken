using EasyRefreshToken.DependencyInjection;
using EasyRefreshToken;
using EasyRefreshTokenTest.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyRefreshTokenTest.CustomServiceTests.Mocks
{
    public class CustomServiceStartup
    {
        public static ServiceCollection ConfigureService(Action<RefreshTokenOptions> options = default)
        {
            var services = new ServiceCollection();

            services.AddCustomRefreshToken<User, Guid, CustomTokenRepository>(options);

            return services;
        }
    }
}
