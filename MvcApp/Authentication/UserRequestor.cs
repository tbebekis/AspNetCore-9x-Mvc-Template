namespace MvcApp
{
    /// <summary>
    /// Since this is an MVC application, the Requestor is a user or something like a <see cref="HttpClient"/> requesting an MVC resource, using user credentials.
    /// </summary>
    internal class UserRequestor : IRequestor, IUserClaimsProvider
    {
        // ● private
        static IRequestor fDefault;

        // ● constants
        public const string SDefaultId = "00000000-0000-0000-0000-000000000000";
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
        /// Designates the culture code to be used for subsequent calls, e.g. en-US
        /// </summary>
        public const string SCultureCodeClaimType = "CultureCode";
        /// <summary>
        /// A claim type for a private claim. 
        /// When true the user is impersonating another user
        /// </summary>
        public const string SIsImpersonationClaimType = "IsImpersonation";

        // ● public
        /// <summary>
        /// Creates and returns a claim list regarding this instance.
        /// <para>Claims are stored in the user cookie.</para>
        /// </summary>
        public List<Claim> GetUserClaimList(string AuthenticationScheme, bool IsImpersonation = false)
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ApplicationException("Cannot produce claims. No Id");

            if (string.IsNullOrWhiteSpace(AccountId))
                throw new ApplicationException("Cannot produce claims. No AccountId");

            List<Claim> ClaimList = new List<Claim>();

            ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, Id));
            ClaimList.Add(new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(Name) ? Name : "no name"));
            ClaimList.Add(new Claim(ClaimTypes.Email, !string.IsNullOrWhiteSpace(Email) ? Email : "no email"));

            // private claims
            ClaimList.Add(new Claim(SUserLevelClaimType, Level.ToString()));
            ClaimList.Add(new Claim(SAuthenticationSchemeClaimType, AuthenticationScheme));
            ClaimList.Add(new Claim(SCultureCodeClaimType, Session.CultureCode));
            ClaimList.Add(new Claim(SIsImpersonationClaimType, IsImpersonation.ToString()));

            return ClaimList;
        }
        /// <summary>
        /// Creates and returns a <see cref="ClaimsPrincipal"/> along with a claim list
        /// </summary>
        public ClaimsPrincipal CreateUserPrincipal(string AuthenticationScheme, bool IsImpersonation = false)
        {
            // create claim list
            List<Claim> ClaimList = GetUserClaimList(AuthenticationScheme, IsImpersonation);

            // identity and principal
            // NOTE: setting the second parameter actually authenticates the identity (IsAuthenticated returns true)
            ClaimsIdentity Identity = new ClaimsIdentity(ClaimList, AuthenticationScheme);
            ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);

            return Principal;
        }

        // ● get values from user claims
        static public T GetUserClaimValue<T>(IEnumerable<Claim> Claims, string ClaimType, T DefaultValue = default(T))
        {
            Claim Claim = Claims.Where(c => c.Type == ClaimType).FirstOrDefault();
            if (Claim == null)
                return DefaultValue;
            else
                return (T)Convert.ChangeType(Claim.Value, typeof(T));
        }
        static public string GetRequestorId(IEnumerable<Claim> Claims)
        {
            return GetUserClaimValue<string>(Claims, ClaimTypes.NameIdentifier);
        }
        static public string GetRequestorCultureCode(IEnumerable<Claim> Claims)
        {
            return GetUserClaimValue<string>(Claims, SCultureCodeClaimType);
        }
        static public UserLevel GetRequestorLevel(IEnumerable<Claim> Claims)
        {
            return GetUserClaimValue<UserLevel>(Claims, SUserLevelClaimType);
        }
        static public bool GetRequestorIsImpersonation(IEnumerable<Claim> Claims)
        {
            return GetUserClaimValue<bool>(Claims, SIsImpersonationClaimType);
        }
        static public string GetRequestorAuthenticationScheme(IEnumerable<Claim> Claims)
        {
            return GetUserClaimValue<string>(Claims, SAuthenticationSchemeClaimType);
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
        static public string DefaultId => SDefaultId;
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
