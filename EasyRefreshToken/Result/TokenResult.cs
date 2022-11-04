using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRefreshToken.Result
{
    /// <summary>
    /// Present some functions result
    /// </summary>
    public class TokenResult
    {
        public TokenResult(bool isSucceded = false, string token = null, string errorMessage = null)
        {
            IsSucceded = isSucceded;
            Token = token;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// if IsSucceded true then the process ok.
        /// </summary>
        public bool IsSucceded { get; set; }

        /// <summary>
        /// new refersh token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Error message (if there)
        /// </summary>
        public string ErrorMessage { get; set; }


        public static TokenResult Success(string token)
            => new TokenResult(true, token);

        public static TokenResult Faild(string errorMessage)
            => new TokenResult(false, null, errorMessage);
    }

}
