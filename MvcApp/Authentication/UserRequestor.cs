namespace MvcApp
{
    /// <summary>
    /// Since this is an MVC application, the Requestor is a user or something like a <see cref="HttpClient"/> requesting an MVC resource, using user credentials.
    /// </summary>
    internal class UserRequestor : IRequestor 
    {
        // ● private
        static IRequestor fDefault;

        // ● constants
        /// <summary>
        /// A claim type for a private claim. 
        /// Designates the level of a user, i.e. Admin, User, Guest, Service, etc.
        /// </summary>
        public const string SUserLevelClaimType = "UserLevel";
        /// <summary>
        /// A claim type for a private claim.
        /// Designates the scheme used in authentication, i.e. Cookies, JWT, etc.
        /// </summary>
        public const string SAuthenticationSchemeClaimType = "AuthenticationScheme";
        /// <summary>
        /// A claim type for a private claim. 
        /// When true the user is impersonating another user
        /// </summary>
        public const string SIsImpersonationClaimType = "IsImpersonation";
 
        // ● get values from user claims
 
        static public string GetRequestorId(IEnumerable<Claim> Claims)
        {
            return WLib.GetClaimValue<string>(Claims, ClaimTypes.NameIdentifier);
        }
        static public UserLevel GetRequestorLevel(IEnumerable<Claim> Claims)
        {
            return WLib.GetClaimValue<UserLevel>(Claims, SUserLevelClaimType);
        }
        static public bool GetRequestorIsImpersonation(IEnumerable<Claim> Claims)
        {
            return WLib.GetClaimValue<bool>(Claims, SIsImpersonationClaimType);
        }
        static public string GetRequestorAuthenticationScheme(IEnumerable<Claim> Claims)
        {
            return WLib.GetClaimValue<string>(Claims, SAuthenticationSchemeClaimType);
        }

        // ● properties
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Database Id or something similar.</para>
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// The level of a user, i.e. Guest, Admin, User, etc.
        /// </summary>
        public UserLevel Level { get; set; }
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para><c>Email</c> or <c>UserName</c>, when <see cref="Level"/> is <see cref="UserLevel.User"/>, <see cref="UserLevel.Admin"/> or <see cref="UserLevel.Guest"/>.</para>
        /// <para><c>ClientId</c> when <see cref="Level"/> is <see cref="UserLevel.ClientApp"/> or <see cref="UserLevel.Service"/>.</para>
        /// </summary> 
        public string AccountId { get; set; }
        /// <summary>
        /// Optional. The requestor name
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        /// Optional. The requestor email
        /// </summary> 
        public string Email { get; set; }
        /// <summary>
        /// True when requestor is blocked by admins
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// The id of the default requestor
        /// </summary>
        static public string DefaultId => DataStore.SDefaultId;
        /// <summary>
        /// The default requestor
        /// </summary>
        static public IRequestor Default
        {
            get
            {
                if (fDefault == null)
                {
                    fDefault = new UserRequestor();
                    fDefault.Id = DefaultId;
                    fDefault.AccountId = "Default";
                    fDefault.Name = "Default";
                    fDefault.Email = "";
                    fDefault.IsBlocked = false;
                    fDefault.Level = UserLevel.Guest;
                }

                return fDefault;
            }
        }

    }
}
