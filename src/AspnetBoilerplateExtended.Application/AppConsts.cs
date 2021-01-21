namespace AspnetBoilerplateExtended
{
    public class AppConsts
    {
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";
        public const string InvalidEmail = "Invalid Email";
        public static string Route = "/account/reset-password/";
        public static string ResetPasswordMailSubject = "Reset your password at CET AT";
        public static string UserNotExistMessage = "Email address is not registered";
        public static string InvalidPasswordMessage = "Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.";
        public static string AuthenticationFailedMessage = "Authentication failed try again with new link";
        public static string ResetPasswordMailBody1 = "If it wasn’t you, please disregard this email and make sure you can still log in to your account.";
        public static string ResetPasswordMailBody2 = "If it was you, then please use the following link to reset your password:";
        public static string ResetPasswordMailBody3 = "You have submitted a password change request. ";
        public static string ResetPasswordMailLinkTitle = "Reset Password";
        public static string CheersMessage = "Cheers! ";
        public static string CetAtTeamMessage = "The CET AT Team ";
    }
}
