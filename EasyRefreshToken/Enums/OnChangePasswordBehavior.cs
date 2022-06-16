using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.Enums
{
    /// <summary>
    /// OnChangePassword Method Behaviors
    /// </summary>
    public enum OnChangePasswordBehavior
    {
        /// <summary>
        /// No thing.
        /// </summary>
        None,
        /// <summary>
        /// Delete all Tokens, login is required after this step
        /// </summary>
        DeleteAllTokens,
        /// <summary>
        /// Delete all tokens and add new token
        /// </summary>
        DeleteAllTokensAndAddNewToken
    }
}
