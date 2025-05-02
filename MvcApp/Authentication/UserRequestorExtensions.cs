namespace MvcApp
{
    static public class UserRequestorExtensions
    {
        /// <summary>
        /// Creates and returns a claim list regarding this instance.
        /// <para>Claims are stored in the user cookie.</para>
        /// </summary>
        static public List<Claim> GetUserClaimList(this IRequestor Requestor, string AuthenticationScheme, bool IsImpersonation = false)
        {
            if (string.IsNullOrWhiteSpace(Requestor.Id))
                throw new ApplicationException("Cannot produce claims. No Id");

            if (string.IsNullOrWhiteSpace(Requestor.AccountId))
                throw new ApplicationException("Cannot produce claims. No AccountId");

            List<Claim> ClaimList = new List<Claim>();

            ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, Requestor.Id));
            ClaimList.Add(new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(Requestor.Name) ? Requestor.Name : "no name"));
            ClaimList.Add(new Claim(ClaimTypes.Email, !string.IsNullOrWhiteSpace(Requestor.Email) ? Requestor.Email : "no email"));

            // private claims
            ClaimList.Add(new Claim(UserRequestor.SUserLevelClaimType, Requestor.Level.ToString()));
            ClaimList.Add(new Claim(UserRequestor.SAuthenticationSchemeClaimType, AuthenticationScheme));
            ClaimList.Add(new Claim(UserRequestor.SIsImpersonationClaimType, IsImpersonation.ToString()));

            return ClaimList;
        }
        /// <summary>
        /// Creates and returns a <see cref="ClaimsPrincipal"/> along with a claim list
        /// </summary>
        static public ClaimsPrincipal CreateUserPrincipal(this IRequestor Requestor, string AuthenticationScheme, bool IsImpersonation = false)
        {
            // create claim list
            List<Claim> ClaimList = GetUserClaimList(Requestor, AuthenticationScheme, IsImpersonation);

            // identity and principal
            // NOTE: setting the second parameter actually authenticates the identity (IsAuthenticated returns true)
            ClaimsIdentity Identity = new ClaimsIdentity(ClaimList, AuthenticationScheme);
            ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);

            return Principal;
        }
    }
}
