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
        // ● properties
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Database Id or something similar.</para>
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Optional. The requestor name
        /// </summary> 
        string Name { get; set; }
        /// <summary>
        /// True when requestor is blocked by admins
        /// </summary>
        bool IsBlocked { get; set; }
    }
}
