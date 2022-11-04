﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.DependencyInjection.Enums
{
    /// <summary>
    /// OnChangePassword method behaviors
    /// </summary>
    public enum OnChangePasswordBehavior
    {
        /// <summary>
        /// Delete all tokens, login is required after this option
        /// </summary>
        DeleteAllTokens,
        /// <summary>
        /// Delete all tokens for user and return new token
        /// </summary>
        DeleteAllTokensAndAddNewToken
    }
}
