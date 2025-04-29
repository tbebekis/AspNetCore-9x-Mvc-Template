namespace MvcApp
{
    /// <summary>
    /// Request context for browser clients (cookies)
    /// <para>NOTE: This is a Scoped Service (i.e. one instance per HTTP Request) </para>
    /// </summary>
    internal class UserRequestContext : RequestContext, IUserRequestContext
    {
        // ● private
        List<Claim> UserClaimList;
        IRequestor fRequestor;
        string fCultureCode;

        string GetCookieAuthScheme => Lib.SCookieAuthScheme;

        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// <para>If the user is found, then it sets the cookie too.</para>
        /// </summary>
        IRequestor GetRequestor(string Id)
        {
            IRequestor Result = null;
 
            if (!string.IsNullOrWhiteSpace(Id))
            {
                Result = DataStore.GetRequestor(Id);
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
            UserClaimList = this.HttpContext.User.Claims.ToList();   
        }

        // ● public 
        /// <summary>
        /// Sign-in. Authenticates a specified, already validated, Visitor
        /// </summary>
        public async Task SignInAsync(IRequestor R, bool IsPersistent, bool IsImpersonation)
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
                    string Id = UserRequestor.GetRequestorId(UserClaimList);  // we have Requestor.Id stored in ClaimTypes.NameIdentifier claim
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
        /// The culture (language) of the current request specified as a culture code (en-US, el-GR)
        /// </summary>
        public override string CultureCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fCultureCode))
                {
                    fCultureCode = UserRequestor.GetRequestorCultureCode(UserClaimList);
                    fCultureCode = !string.IsNullOrWhiteSpace(fCultureCode) ? fCultureCode : App.AppSettings.DefaultCultureCode;
                }

                return fCultureCode;
            }
            set
            {
                if (!Sys.IsSameText(fCultureCode, value))
                {
                    fCultureCode = value;
                    Session.CultureCode = fCultureCode;
                }
            }
        }

        /// <summary>
        /// True when the user is authenticated with the cookie authentication scheme.
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
            get { return UserRequestor.GetRequestorIsImpersonation(UserClaimList); }   
            private set {  Session.Set<bool>("IsImpersonation", value);  }
        }

    }
}
