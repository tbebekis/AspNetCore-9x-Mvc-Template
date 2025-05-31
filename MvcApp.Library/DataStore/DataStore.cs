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

        /// <summary>
        /// The Cache. It is provided by the Lib
        /// </summary>
        static IWebAppCache Cache => Lib.Cache;
        /// <summary>
        /// The eviction timeout of an entry from the cache, in minutes. 
        /// <para>Defaults to 0 which means "use the timeouts of the internal implementation".</para>
        /// </summary>
        static int CacheTimeoutMinutes => Lib.AppSettings.Defaults.CacheTimeoutMinutes;
        /// <summary>
        /// The number of items in a page when paging is involved.
        /// </summary>
        static int DefaultPageSize => Lib.AppSettings.Defaults.PageSize;


        // ● public
        /// <summary>
        /// Initializes the data store
        /// </summary>
        static public void Initialize()
        {
            // nothing
        }
 

        /// <summary>
        /// Returns application's <see cref="DbContext"/>
        /// </summary>
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
        static public ItemDataResult<IUserRequestor> GetAppUserById(string Id)
        {
            ItemDataResult<IUserRequestor> Result = new();

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
        static public ItemDataResult<IUserRequestor> ValidateUserCredentials(string UserName, string Password)
        {
            ItemDataResult<IUserRequestor> Result = new();

            using (AppDbContext context = GetDbContext())
            {
                AppUser User = context.Users.FirstOrDefault(x => x.UserName == UserName);
                if (User != null)
                {
                    if (Hasher.Validate(Password, User.Password, User.PasswordSalt))
                        Result.Item = User;
                }
            }

            return Result;
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

        // ● properties
        /// <summary>
        /// The <see cref="CultureInfo"/> culture of the current request.
        /// </summary>
        static public CultureInfo Culture => Lib.AppContext.Culture;
 
    }
}
