namespace MvcApp
{
    /// <summary>
    /// Represents the web application context
    /// <para>There is a single instance of this class, which is assigned to the <see cref="Lib.AppContext"/> property.</para>
    /// </summary>
    internal class AppContext : IMvcAppContext
    {
        IWebAppCache fCache;

        // ● public
        /// <summary>
        /// Returns a service specified by a type argument. If the service is not registered an exception is thrown.
        /// <para>WARNING: "Scoped" services can NOT be resolved from the "root" service provider. </para>
        /// <para>There are two solutions to the "Scoped" services problem:</para>
        /// <para> ● Use <c>HttpContext.RequestServices</c>, a valid solution since we use a "Scoped" service provider to create the service,  </para>
        /// <para> ● or add <c> .UseDefaultServiceProvider(options => options.ValidateScopes = false)</c> in the <c>CreateHostBuilder</c>() of the Program class</para>
        /// <para>see: https://github.com/dotnet/runtime/issues/23354 and https://devblogs.microsoft.com/dotnet/announcing-ef-core-2-0-preview-1/ </para>
        /// <para>SEE: https://www.milanjovanovic.tech/blog/using-scoped-services-from-singletons-in-aspnetcore</para>
        /// <para>SEE: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines#scoped-service-as-singleton</para>
        /// </summary>
        public T GetService<T>(IServiceScope Scope = null)
        {
            return App.GetService<T>(Scope);
        }
        /// <summary>
        /// Returns the current <see cref="HttpContext"/>
        /// </summary>
        public HttpContext GetHttpContext() => App.GetHttpContext();
 
        // ● properties
        /// <summary>
        /// The <see cref="IWebHostEnvironment"/>
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment => App.WebHostEnvironment;
 
        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        public string ContentRootPath => App.ContentRootPath;
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        public string WebRootPath => App.WebRootPath;
        /// <summary>
        /// The physical path of the output folder
        /// <para>e.g. C:\MyApp\bin\Debug\net9.0\  </para>
        /// </summary>
        public string BinPath => App.BinPath;

        /// <summary>
        /// True when the application is running in development mode
        /// </summary>
        public bool InDevMode => App.InDevMode;

        /// <summary>
        /// The default requestor
        /// </summary>
        public IRequestor DefaultRequestor => UserRequestor.Default;
        /// <summary>
        /// True when the current user/requestor is authenticated with the cookie authentication scheme.
        /// </summary>
        public bool IsAuthenticated => GetService<IUserRequestContext>().IsAuthenticated;

        /// <summary>
        /// The <see cref="CultureInfo"/> culture of the current request.
        /// <para>CAUTION: The culture of each HTTP Request is set by a lambda in ConfigureServices().
        /// This property here uses that setting to return its value.
        /// </para>
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                CultureInfo Result = CultureInfo.GetCultureInfo("en-US");

                HttpContext HttpContext = GetHttpContext();
                if (HttpContext != null)
                {
                    IRequestCultureFeature Feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    if (Feature != null)
                        Result = Feature.RequestCulture.Culture;
                }
                    
                return Result;      

            }
        }
        /// <summary>
        /// Application settings, coming from appsettings.json
        /// </summary>
        public AppSettings AppSettings => App.AppSettings;

        /// <summary>
        /// Represents an application memory cache.
        /// </summary>
        public IWebAppCache Cache
        {
            get
            {
                if (fCache == null)
                {
                    WebAppMemoryCache Instance = new WebAppMemoryCache(GetService<IMemoryCache>());
                    Instance.DefaultEvictionTimeoutMinutes = App.AppSettings.CacheTimeoutMinutes;
                    fCache = Instance;
                }
                return fCache;
            }
        }

    }
}
