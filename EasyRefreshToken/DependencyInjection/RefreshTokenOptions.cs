using EasyRefreshToken.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Options to control the behavior of the token service.
    /// </summary>
    public class RefreshTokenOptions
    {
        /// <summary>
        /// Gets or sets the number of days until the token expires. If set to null, the token will never expire. Default value is 7 days.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? TokenExpiredDays { get; set; } = 7;

        /// <summary>
        /// Gets or sets a value indicating whether to prevent login operation when the maximum number of active devices is reached. If set to true and there is a valid token, login will be prevented. If set to false, the old token will be removed and a new token will be added. Default value is true.
        /// </summary>
        public bool PreventingLoginWhenAccessToMaxNumberOfActiveDevices { get; set; } = true;

        /// <summary>
        /// Gets or sets the method used for generating tokens.
        /// </summary>
        public Func<string> TokenGenerationMethod { get; set; } = Helpers.GenerateRefreshToken;

        /// <summary>
        /// Gets or sets the behavior of the OnChangePassword method. Default value is OnChangePasswordBehavior.DeleteAllTokens.
        /// </summary>
        public OnChangePasswordBehavior OnChangePasswordBehavior { get; set; } = OnChangePasswordBehavior.DeleteAllTokens;

        /// <summary>
        /// Gets or sets the maximum number of active devices per user type. If a type is not specified, the default value from <see cref="MaxNumberOfActiveDevices"/> will be used.
        /// </summary>
        public MaxNumberOfActiveDevices MaxNumberOfActiveDevices { get; set; }
            = MaxNumberOfActiveDevices.Configure(int.MaxValue);
    }
}
