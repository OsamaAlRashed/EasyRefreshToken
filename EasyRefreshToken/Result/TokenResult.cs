using System;

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
        /// Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message (if there)
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenResult SetSuccess(string token) => new ()
        {
            IsSucceded = true,
            Token = token
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TokenResult SetFailed(string errorMessage, int code = 400) => new()
        {
            ErrorMessage = errorMessage,
            IsSucceded = false,
            Code = code,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TokenResult SetException(Exception exception, int code = 500) => new()
        {
            Code = code,
            Exception = exception,
            ErrorMessage = exception.Message,
            IsSucceded = false
        };
    }

}
