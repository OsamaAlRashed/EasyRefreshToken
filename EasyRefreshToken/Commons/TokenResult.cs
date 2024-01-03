using System;

namespace EasyRefreshToken
{
    /// <summary>
    /// Represents the result of a token operation.
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the process was successful.
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Gets or sets the new refresh token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the error message, if any.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception that occurred, if any.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Creates a successful token result with the specified token.
        /// </summary>
        /// <param name="token">The new refresh token.</param>
        /// <param name="code">The success code (default is 200).</param>
        /// <returns>A new instance of <see cref="TokenResult"/> representing a successful result.</returns>
        public static TokenResult SetSuccess(string token, int code = 200) 
            => new TokenResult
            {
                IsSucceeded = true,
                Token = token,
                Code = code
            };

        /// <summary>
        /// Creates a failed token result with the specified error message and code.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="code">The error code (default is 400).</param>
        /// <returns>A new instance of <see cref="TokenResult"/> representing a failed result.</returns>
        public static TokenResult SetFailed(string errorMessage, int code = 400) 
            => new TokenResult
            {
                IsSucceeded = false,
                Code = code,
                ErrorMessage = errorMessage
            };

        /// <summary>
        /// Creates an exception token result with the specified exception and code.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="code">The error code (default is 500).</param>
        /// <returns>A new instance of <see cref="TokenResult"/> representing an exception result.</returns>
        public static TokenResult SetException(Exception exception, int code = 500) 
            => new TokenResult
            {
                IsSucceeded = false,
                Code = code,
                Exception = exception,
                ErrorMessage = exception.Message
            };
    }
}
