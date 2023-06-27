namespace EasyRefreshToken.Enums
{
    /// <summary>
    /// Defines the behaviors for the OnChangePasswordAsync method.
    /// </summary>
    public enum OnChangePasswordBehavior
    {
        /// <summary>
        /// Deletes all tokens. Login is required after using this option.
        /// </summary>
        DeleteAllTokens,

        /// <summary>
        /// Deletes all tokens for the user and generates a new token.
        /// </summary>
        DeleteAllTokensAndAddNewToken
    }
}
