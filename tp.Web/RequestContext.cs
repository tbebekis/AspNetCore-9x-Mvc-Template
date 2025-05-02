namespace tp.Web
{
    /// <summary>
    /// Represents a context regarding the current HTTP request.
    /// <para><strong>NOTE</strong>: Whatever information is intended to have the lifetime of the HTTP request should be added in this object.</para>
    /// </summary>
    public abstract class RequestContext: IRequestContext
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public RequestContext(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContext = HttpContextAccessor.HttpContext;
        }

 

        // ● properties
        /// <summary>
        /// The http context
        /// </summary>
        public HttpContext HttpContext { get; }
        /// <summary>
        /// The http request
        /// </summary>
        public HttpRequest Request => HttpContext.Request;
        /// <summary>
        /// The query string as a collection of key-value pairs
        /// </summary>
        public IQueryCollection Query => Request.Query;   

        /// <summary>
        /// The user or api client of the current request
        /// </summary>
        public virtual IRequestor Requestor { get; set; }
        /// <summary>
        /// True when the request is authenticated.
        /// </summary>
        public abstract bool IsAuthenticated { get; }
    }

}
