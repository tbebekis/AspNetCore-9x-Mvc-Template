namespace MvcApp.Library
{

    /// <summary>
    /// DataStore is the class for database operations.
    /// <para><strong>NOTE: </strong>This implementation does nothing actually related to database operations. It's here just for demonstration purposes.</para>
    /// </summary>
    static public partial class DataStore
    {

        // ● private
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the current (Session's) culture code, e.g. el-GR
        /// </summary>
        static string L(string Key, params object[] Args)
        {
            string S = Res.GetString(Key);
            if ((Args != null) && (Args.Length > 0))
                S = string.Format(S, Args);
            return S;
        }

        // ● public
        /// <summary>
        /// Initializes the data store
        /// </summary>
        static public void Initialize()
        {
            // nothing
        }

        static public AppDbContext GetDbContext()
        {
            HttpContext HttpContext = WLib.GetHttpContext();

            IServiceScope Scope = HttpContext.RequestServices.CreateScope();
            AppDbContext Result = Scope.ServiceProvider.GetService<AppDbContext>();

            return Result;
        }


        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// </summary>
        static public ItemDataResult<IRequestor> GetAppUserById(string Id)
        {
            ItemDataResult<IRequestor> Result = new();

            using (AppDbContext context = GetDbContext())
            {
                AppUser User = context.Users.FirstOrDefault(x => x.Id == Id);
                if (User == null)
                {
                    Result.AddError($"User not found. Id {Id}");
                }
                else
                {
                    Result.Item = User;
                }
            }
                
            return Result;
        }
        /// <summary>
        /// Validates the specified user credentials and returns a <see cref="IRequestor"/> on success, else null.
        /// </summary>
        static public ItemDataResult<IRequestor> ValidateUserCredentials(string UserId, string Password)
        {
            return null;
        }
        /// <summary>
        /// Returns true if the requestor is impersonating another user, by using a super user password
        /// </summary>
        static public bool GetIsUserImpersonation(string PlainTextPassword)
        {
            // TODO: GetIsImpersonation() - Requestor should come from database
            // bool IsImpersonation = !string.IsNullOrWhiteSpace(Settings.General.SuperUserPassword) && Settings.General.SuperUserPassword == M.Password;
            return false;
        }
    }
}
