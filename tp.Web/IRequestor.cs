namespace tp.Web
{

    /// <summary>
    /// May represent
    /// <list type="bullet">
    /// <item><strong>in an MVC application:</strong> a user or something like a <see cref="HttpClient"/> requesting an MVC resource, using user credentials.</item>
    /// <item><strong>in a WebAPI application:</strong> a client application or service requesting a WebAPI resource, using a JWT Access Token.</item>
    /// </list>
    /// </summary>
    public interface IRequestor
    {
        // ● public
        /// <summary>
        /// Creates and returns a claim list regarding this instance.
        /// <para>Claims are stored in the user cookie.</para>
        /// </summary>
        List<Claim> GetUserClaimList(string AuthenticationScheme, bool IsImpersonation = false);
        /// <summary>
        /// Creates and returns a <see cref="ClaimsPrincipal"/> along with a claim list
        /// </summary>
        ClaimsPrincipal CreateUserPrincipal(string AuthenticationScheme, bool IsImpersonation = false);

        // ● properties
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Database Id or something similar.</para>
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// The level of a user, i.e. Guest, Admin, User, etc.
        /// </summary>
        UserLevel Level { get; set; }
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para><c>Email</c> or <c>UserName</c>, when <see cref="Level"/> is <see cref="UserLevel.User"/>, <see cref="UserLevel.Admin"/> or <see cref="UserLevel.Guest"/>.</para>
        /// <para><c>ClientId</c> when <see cref="Level"/> is <see cref="UserLevel.ClientApp"/> or <see cref="UserLevel.Service"/>.</para>
        /// </summary> 
        string AccountId { get; set; }
        /// <summary>
        /// Optional. The requestor name
        /// </summary> 
        string Name { get; set; }
        /// <summary>
        /// Optional. The requestor email
        /// </summary> 
        string Email { get; set; }
        /// <summary>
        /// True when requestor is blocked by admins
        /// </summary>
        bool IsBlocked { get; set; }
    }
}
