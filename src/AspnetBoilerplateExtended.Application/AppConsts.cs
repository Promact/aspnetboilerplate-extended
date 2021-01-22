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
        public const string AlphanumericRegex = "^[a-zA-Z0-9][a-zA-Z0-9_@.,()*/#&+-_ ]*$";
        public const string AlphanumericRegexForDatabase = "^.[a-zA-Z0-9_@.,()*/#&+-_ ]*$";
        public const string DecimalRegex = @"^[0-9]{0,5}(\.[0-9]{0,2})?$";
        public const string ApplicationIsAlreadyExist = "Application should be unique";
        public const string UseOnlyAlphaNumericForApplication = "Application name should be valid.";
        public const string DoesNotExist = "Application does not exist";
        public const string DashSymbol = "-";
        public const string ExcelFileExtention = ".xlsx";
        public const string ExportFilename = "ExportedFile";
        public static string ExcelFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string FileNotExistMessage = "File does not exists";
    }
}
