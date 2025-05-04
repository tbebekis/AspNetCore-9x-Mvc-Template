namespace tp.Web
{
    /// <summary>
    /// Represents a context regarding the current HTTP request.
    /// <para><strong>NOTE</strong>: Whatever information is intended to have the lifetime of the HTTP request should be added in this object.</para>
    /// </summary>
    public interface IRequestContext
    {  

        // ● properties
        /// <summary>
        /// The http context
        /// </summary>
        HttpContext HttpContext { get; }
        /// <summary>
        /// The http request
        /// </summary>
        HttpRequest Request { get; }
        /// <summary>
        /// The query string as a collection of key-value pairs
        /// </summary>
        IQueryCollection Query { get; }
 
        /// <summary>
        /// The user or api client of the current request
        /// </summary>
        IRequestor Requestor { get; set; }
        /// <summary>
        /// True when the request is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
