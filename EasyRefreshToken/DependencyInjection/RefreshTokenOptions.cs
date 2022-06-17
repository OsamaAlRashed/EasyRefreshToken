using EasyRefreshToken.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Options to control on Token Service
    /// </summary>
    public class RefreshTokenOptions
    {
        /// <summary>
        /// Max number Of Active Devices per user, if set null will be unlimited
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? MaxNumberOfActiveDevices { get; set; } = null;

        /// <summary>
        /// The number of days until the token expires, if it is configured as null, the code will never expire.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? TokenExpiredDays { get; set; } = 7;

        /// <summary>
        /// if set true and there is valid token, then login operation will be prevent.
        /// if set false, then old token will be removed and add a new token  
        /// </summary>
        public bool PreventingLoginWhenAccessToMaxNumberOfActiveDevices { get; set; } = true;

        /// <summary>
        /// Determination generation method
        /// </summary>
        public Func<string> TokenGenerationMethod { get; set; } = null;

        /// <summary>
        /// Determination OnChangePassword Method Behavior
        /// </summary>
        public OnChangePasswordBehavior OnChangePasswordBehavior { get; set; } = OnChangePasswordBehavior.DeleteAllTokens;

    }
}
