using EasyRefreshToken.DependencyInjection.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyRefreshToken.DependencyInjection
{
    /// <summary>
    /// Options to control on token service
    /// </summary>
    public class RefreshTokenOptions
    {
        /// <summary>
        /// The number of days until the token expires, if set null, the code will never expire.
        /// Default value is 7 days
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? TokenExpiredDays { get; set; } = 7;

        /// <summary>
        /// If set true and there is valid token, then login operation will be prevent.
        /// If set false, then old token will be removed and add a new token  
        /// Default value is true
        /// </summary>
        public bool PreventingLoginWhenAccessToMaxNumberOfActiveDevices { get; set; } = true;

        /// <summary>
        /// Determination generation method
        /// </summary>
        public Func<string> TokenGenerationMethod { get; set; } = null;

        /// <summary>
        /// Determination OnChangePassword Method Behavior
        /// Dfault value is OnChangePasswordBehavior.DeleteAllTokens
        /// </summary>
        public OnChangePasswordBehavior OnChangePasswordBehavior { get; set; } = OnChangePasswordBehavior.DeleteAllTokens;

        /// <summary>
        /// Max number Of Active Devices per every type of user, if type not set will take <code>MaxNumberOfActiveDevices</code> option.
        /// </summary>
        public MaxNumberOfActiveDevices MaxNumberOfActiveDevices { get; set; }

        /// <summary>
        /// Save changes in database automaticlly. The default value is true.
        /// </summary>
        public bool SaveChanges { get; set; } = true;
    }
}
