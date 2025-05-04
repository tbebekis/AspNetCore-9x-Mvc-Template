namespace MvcApp.Library
{
    static public partial class DataStore
    {
        // ● private
        /// <summary>
        /// Validates the password of a user/requestor
        /// </summary>
        static bool ValidatePassword(string PlainTextPassword, string Base64SaltKey, string Base64HashedPassword)
        {
            return true;

            /* TODO: ValidatePassword - Password should come from database
             
            if (string.IsNullOrWhiteSpace(PlainTextPassword) || string.IsNullOrWhiteSpace(Base64SaltKey))
                return false;

            var Settings = GetSettings();
            string SuperUserPassword = Settings.General.SuperUserPassword;
            if (!string.IsNullOrWhiteSpace(SuperUserPassword) && (PlainTextPassword == SuperUserPassword))
                return true;

            return Hasher.Validate(PlainTextPassword, Base64HashedPassword, Base64SaltKey);
            */
        }
 
        // ● public
        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// </summary>
        static public IRequestor GetRequestorById(string Id)
        {
            // TODO: Requestor should come from database
            return Lib.AppContext.DefaultRequestor;
        }
        /// <summary>
        /// Returns a user from database found under a specified database Id, if any, else null.
        /// </summary>
        static public AppUser GetUserById(string Id)
        {
            // TODO: Requestor should come from database
            return AppUser.Default;
        }
        /// <summary>
        /// Returns a user from database found under a specified user Id, if any, else null.
        /// </summary>
        static public AppUser GetUserByUserId(string UserId)
        {
            // TODO: Requestor should come from database
            return AppUser.Default;
        }
 
        /// <summary>
        /// Validates the specified credentials and returns a Visitor on success, else null.
        /// </summary>
        static public ItemResponse<IRequestor> ValidateRequestor(string UserId, string Password)
        {
            ItemResponse<IRequestor> Result = new ItemResponse<IRequestor>();

 
            if (string.IsNullOrWhiteSpace(UserId))
            {
                Result.AddError(L("Requestor.NotRegistered"));
            }
            else
            {
                AppUser AppUser = GetUserByUserId(UserId);

                if (AppUser == null)
                {
                    Result.AddError(L("Requestor.NotRegistered"));
                }
                else if (string.IsNullOrWhiteSpace(AppUser.Password) || string.IsNullOrWhiteSpace(AppUser.PasswordSalt))
                {
                    Result.AddError(L("Requestor.NotRegistered"));
                }
                else if (!ValidatePassword(Password, AppUser.PasswordSalt, AppUser.Password))
                {
                    Result.AddError(L("Requestor.InvalidPassword"));
                }
                else if (AppUser.Level != UserLevel.Admin && !AppUser.IsActivated)
                {
                    Result.AddError(L("Requestor.NotActivated"));
                }
                else if (AppUser.Level != UserLevel.Admin && AppUser.IsBlocked)
                {
                    Result.AddError(L("Requestor.NotValidRequestor"));
                }
                else
                {
                    Result.Item = AppUser;
                }
            }      


            if (Result.Item == null && (Result.Errors == null || Result.Errors.Count == 0))
                Result.AddError(L("Error.Unknown"));

            return Result;
        }
        /// <summary>
        /// Returns true if the requestor is impersonating another user, by using a super user password
        /// </summary>
        static public bool GetIsImpersonation(string PlainTextPassword)
        {
            // TODO: GetIsImpersonation() - Requestor should come from database
            // bool IsImpersonation = !string.IsNullOrWhiteSpace(Settings.General.SuperUserPassword) && Settings.General.SuperUserPassword == M.Password;
            return false;
        }
    }
}
