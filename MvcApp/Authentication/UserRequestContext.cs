namespace MvcApp.Authentication
{
    /// <summary>
    /// Request context for browser clients (cookies)
    /// <para>NOTE: This is a Scoped Service (i.e. one instance per HTTP Request) </para>
    /// </summary>
    internal class UserRequestContext : RequestContext, IUserRequestContext
    {
        // ● private          
        IRequestor fRequestor;

        string GetCookieAuthScheme => Lib.SCookieAuthScheme;
        List<Claim> UserClaimList => this.HttpContext.User.Claims.ToList();

        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// <para>If the user is found, then it sets the cookie too.</para>
        /// </summary>
        IRequestor GetRequestor(string Id)
        {
            IRequestor Result = null;
 
            if (!string.IsNullOrWhiteSpace(Id))
            {
                ItemDataResult<IUserRequestor> ItemResult = DataStore.GetAppUserById(Id);
                Result = ItemResult.Succeeded ? ItemResult.Item : null;
            }  

            return Result;
        }
 

        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public UserRequestContext(IHttpContextAccessor HttpContextAccessor)
            : base(HttpContextAccessor)
        {       
        }

        // ● public 
        /// <summary>
        /// Sign-in. Authenticates a specified, already validated, Visitor
        /// </summary>
        public async Task SignInAsync(IUserRequestor R, bool IsPersistent, bool IsImpersonation)
        {
            // await Task.CompletedTask;
            this.IsImpersonation = IsImpersonation;
 
            ClaimsPrincipal Principal = R.CreateUserPrincipal(GetCookieAuthScheme, IsImpersonation);

            // properties
            AuthenticationProperties AuthProperties = new AuthenticationProperties();
            AuthProperties.AllowRefresh = true;
            AuthProperties.IssuedUtc = DateTime.UtcNow;
            //AuthProperties.ExpiresUtc = DateTime.UtcNow.AddDays(30); // overrides the ExpireTimeSpan option of CookieAuthenticationOptions set with AddCookie
            AuthProperties.IsPersistent = IsPersistent;

            // authenticate the principal under the scheme
            await HttpContext.SignInAsync(GetCookieAuthScheme, Principal, AuthProperties);
            this.Requestor = R;
        }
        /// <summary>
        /// Sign-out.
        /// </summary>
        public async Task SignOutAsync()
        {
            IsImpersonation = false;

            string Scheme = GetCookieAuthScheme;

            await HttpContext.SignOutAsync(Scheme);
            this.Requestor = null;
        }

        // ● properties
        /// <summary>
        /// The current Requestor (the session Requestor)
        /// <para>NOTE: Setting or unsetting the Requestor, sets or unsets the Requestor cookie too.</para>
        /// </summary>
        public override IRequestor Requestor
        {
            get
            {
                if (fRequestor == null)
                {
                    string Id = UserRequestor.GetUserId(UserClaimList);  // we have Requestor.Id stored in ClaimTypes.Sid claim
                    fRequestor = !string.IsNullOrWhiteSpace(Id) ? GetRequestor(Id) : UserRequestor.Default;
                }

                return fRequestor;
            }
            set
            {
                // right after a logout
                if (value == null)
                {
                    fRequestor = UserRequestor.Default;
                }
                // right after the login
                else
                {
                    fRequestor = value;
                }
            }

        }
        /// <summary>
        /// True when the current user/requestor is authenticated with the cookie authentication scheme.
        /// </summary>
        public override bool IsAuthenticated
        {
            get
            {
                bool Result = HttpContext.User.Identity.IsAuthenticated;
                if (Result)
                    Result = HttpContext.User.Identity.AuthenticationType == GetCookieAuthScheme;

                return Result;
            }
        }
        /// <summary>
        /// True when the Visitor has loged-in usin the SuperUserPassword
        /// </summary>
        public bool IsImpersonation
        {
            get { return UserRequestor.GetIsUserImpersonation(UserClaimList); }   
            private set {  Session.Set<bool>("IsImpersonation", value);  }
        }

    }
}
