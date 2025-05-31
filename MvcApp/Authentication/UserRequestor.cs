namespace MvcApp.Authentication
{
    public class UserRequestor: IUserRequestor
    {
        // ● private
        static UserRequestor fDefault;

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



        public UserRequestor() 
        {
        }
        public UserRequestor(AppUser AppUser) 
        {
            Id = AppUser.Id;
            UserName = AppUser.UserName;
            Name = AppUser.Name;
            Email = AppUser.Email;
            IsBlocked = AppUser.IsBlocked;
        }

        // ● get values from user claims
        /// <summary>
        /// Returns the user id from claims.
        /// <para>This is a database table Id.</para>
        /// </summary>
        static public string GetUserId(IEnumerable<Claim> Claims)
        {
            return WLib.GetClaimValue<string>(Claims, ClaimTypes.Sid);
        }
        /// <summary>
        /// Returns true if the requestor is impersonating another user
        /// </summary>
        static public bool GetIsUserImpersonation(IEnumerable<Claim> Claims)
        {
            return WLib.GetClaimValue<bool>(Claims, SIsImpersonationClaimType);
        }
        /// <summary>
        /// Returns the authentication scheme of the requestor
        /// </summary>
        static public string GetUserAuthenticationScheme(IEnumerable<Claim> Claims)
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
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Email or UserName.</para>
        /// </summary> 
        public string UserName { get; set; }
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
        static public string DefaultId => Sys.SDefaultId;
        /// <summary>
        /// The default requestor
        /// </summary>
        static public IUserRequestor Default
        {
            get
            {
                if (fDefault == null)
                {
                    fDefault = new UserRequestor();
                    fDefault.Id = DefaultId;
                    fDefault.UserName = "Default";
                    fDefault.Name = "Default";
                    fDefault.Email = "";
                    fDefault.IsBlocked = false;
                }

                return fDefault;
            }
        }
    }
}
