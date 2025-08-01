﻿namespace MvcApp.Authentication
{
    static public class UserRequestorExtensions
    {
        /// <summary>
        /// Generates and returns a claim list regarding a <see cref="IRequestor"/>.
        /// <para>Claims are stored in the user cookie.</para>
        /// </summary>
        static public List<Claim> GenerateUserClaimList(this IUserRequestor Requestor, string AuthenticationScheme, bool IsImpersonation = false)
        {
            if (string.IsNullOrWhiteSpace(Requestor.Id))
                throw new ApplicationException("Cannot produce claims. No Id");

            if (string.IsNullOrWhiteSpace(Requestor.UserName))
                throw new ApplicationException("Cannot produce claims. No UserName");

            List<Claim> ClaimList = new List<Claim>();

            ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, Requestor.Id));
            ClaimList.Add(new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(Requestor.Name) ? Requestor.Name : "no name"));
            ClaimList.Add(new Claim(ClaimTypes.Email, !string.IsNullOrWhiteSpace(Requestor.Email) ? Requestor.Email : "no email"));

            // private claims
            ClaimList.Add(new Claim(UserRequestor.SAuthenticationSchemeClaimType, AuthenticationScheme));
            ClaimList.Add(new Claim(UserRequestor.SIsImpersonationClaimType, IsImpersonation.ToString()));

            return ClaimList;
        }
        /// <summary>
        /// Creates and returns a <see cref="ClaimsPrincipal"/> along with a claim list for a specified <see cref="IUserRequestor"/>.
        /// </summary>
        static public ClaimsPrincipal CreateUserPrincipal(this IUserRequestor Requestor, string AuthenticationScheme, bool IsImpersonation = false)
        {
            // create claim list
            List<Claim> ClaimList = GenerateUserClaimList(Requestor, AuthenticationScheme, IsImpersonation);

            // identity and principal
            // NOTE: setting the second parameter actually authenticates the identity (IsAuthenticated returns true)
            ClaimsIdentity Identity = new ClaimsIdentity(ClaimList, AuthenticationScheme);
            ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);

            return Principal;
        }
    }
}
