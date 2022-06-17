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
        /// if true, more than one device will be allowed to access the account at the same time
        /// if false, only one device is allowed to access the account at the same time
        /// </summary>
        public bool MultiDevice { get; set; } = false;

        /// <summary>
        /// The number of days until the token expires, if it is configured as null, the code will never expire.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? TokenExpiredDays { get; set; } = 7;

        /// <summary>
        /// if true and there is valid token, then login operation will be blocked.
        /// note: this option valid when MultiDevice is false.
        /// </summary>
        public bool BlockLoginWhenActive { get; set; } = false;

        /// <summary>
        /// Determination generation method
        /// </summary>
        public Func<string> GenerateTokenMethod { get; set; } = null;

        /// <summary>
        /// Determination OnChangePassword Method Behavior
        /// </summary>
        public OnChangePasswordBehavior OnChangePasswordBehavior { get; set; } = OnChangePasswordBehavior.DeleteAllTokens;

    }
}
