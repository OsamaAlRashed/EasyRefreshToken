namespace EasyRefreshToken.Result
{
    /// <summary>
    /// Present some functions result
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSucceded"></param>
        /// <param name="token"></param>
        /// <param name="errorMessage"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenResult Success(string token)
            => new(true, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static TokenResult Failed(string errorMessage)
            => new(false, null, errorMessage);
    }

}
