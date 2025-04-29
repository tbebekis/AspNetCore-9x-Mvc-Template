namespace MvcApp
{
    /// <summary>
    /// <para>SEE: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie#react-to-back-end-changes</para>
    /// </summary>
    internal class UserCookieAuthEvents : CookieAuthenticationEvents
    {
        // ● private
        IUserRequestContext fUserRequestContext;

        string GetCookieAuthScheme() => Lib.SCookieAuthScheme;

        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public UserCookieAuthEvents(IUserRequestContext UserRequestContext)
        {
            this.fUserRequestContext = UserRequestContext;
        }

        // ● public overrides
        /// <summary>
        /// https://stackoverflow.com/questions/73294941/how-to-update-user-claims-stored-in-authentication-cookie-in-net-core-6
        /// </summary>
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            try
            {
                if (fUserRequestContext != null && context.Principal.Identity.IsAuthenticated)
                {
                    Claim Claim = context.Principal.FindFirst(ClaimTypes.NameIdentifier); // we have Requestor.Id stored in ClaimTypes.NameIdentifier claim
                    IRequestor Requestor = fUserRequestContext.Requestor;

                    // it is the Id claim and must be there
                    if (Claim != null && Requestor != null)
                    {
                        // Requestor is blocked  but we still have a logged-in Requestor
                        // so we must sign-out and return
                        if (Requestor.IsBlocked)
                        {
                            context.RejectPrincipal();
                            await fUserRequestContext.SignOutAsync();
                            return;
                        }

                        // check for a culture change
                        Claim = context.Principal.FindFirst(UserRequestor.SCultureCodeClaimType); 
                        string CultureCode = Claim != null ? Claim.Value : App.AppSettings.DefaultCultureCode;

                        // if the culture has changed then create a new principal with the updated claims
                        // and replace the old principal
                        if (CultureCode != Session.CultureCode)
                        {
                            Claim = context.Principal.FindFirst(UserRequestor.SIsImpersonationClaimType);
                            bool IsImpersonation = Claim != null ? Convert.ToBoolean(Claim.Value) : false;
                            ClaimsPrincipal Principal = Requestor.CreateUserPrincipal(GetCookieAuthScheme(), IsImpersonation);
                            context.ReplacePrincipal(Principal);
                            context.ShouldRenew = true;       // renew the cookie
                        }
                    }
                }
            }
            catch
            {
                // do nothing
            }
        }

    }
}
