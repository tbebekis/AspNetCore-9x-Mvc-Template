namespace MvcApp.Authentication
{
    /// <summary>
    /// Allows subscribing to events raised during cookie authentication.
    /// <para>SEE: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie#react-to-back-end-changes</para>
    /// </summary>
    internal class UserCookieAuthEvents : CookieAuthenticationEvents
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public UserCookieAuthEvents()
        {           
        }

        // ● public overrides
        /// <summary>
        /// <para><strong>WARNING: </strong>here HttpContext.User.Identity.IsAuthenticated is yet false 
        /// because the authentication validation is not finished yet</para>
        /// https://stackoverflow.com/questions/73294941/how-to-update-user-claims-stored-in-authentication-cookie-in-net-core-6
        /// </summary>
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            try
            {
                if (context.Principal.Identity.IsAuthenticated)
                {
                    Claim Claim = context.Principal.FindFirst(ClaimTypes.NameIdentifier); // we have Requestor.Id stored in ClaimTypes.NameIdentifier claim
                    string RequestorId = WLib.GetClaimValue<string>(Claim);
                    IRequestor Requestor = DataStore.GetRequestorById(RequestorId); 

                    // it is the Id claim and must be there
                    if (Claim != null && Requestor != null)
                    {
                        // Requestor is blocked  but we still have a logged-in Requestor
                        // so we must sign-out and return
                        if (Requestor.IsBlocked)
                        {
                            context.RejectPrincipal();

                            // WARNING: here HttpContext.User.Identity.IsAuthenticated is yet false 
                            // because the authentication validation is not finished yet
                            IUserRequestContext UserRequestContext = App.GetService<IUserRequestContext>();
                            await UserRequestContext.SignOutAsync();
                            return;
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
