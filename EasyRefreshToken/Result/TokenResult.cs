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
    }
}
