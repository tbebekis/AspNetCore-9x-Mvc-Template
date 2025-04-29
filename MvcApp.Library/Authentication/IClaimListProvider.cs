namespace MvcApp.Library
{
    /// <summary>
    /// Constructs a list of claims
    /// </summary>
    public interface IClaimListProvider
    {
        /// <summary>
        /// Creates and returns a claim list regarding this instance
        /// </summary>
        List<Claim> GetUserClaimList(string AuthenticationScheme, bool IsImpersonation = false);
    }
}
