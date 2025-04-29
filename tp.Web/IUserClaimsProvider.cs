namespace tp.Web
{
    /// <summary>
    /// Constructs the list of user claims
    /// </summary>
    public interface IUserClaimsProvider
    {
        /// <summary>
        /// Creates and returns a claim list regarding this instance
        /// </summary>
        List<Claim> GetUserClaimList(string AuthenticationScheme, bool IsImpersonation = false);
    }
}
