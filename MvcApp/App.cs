namespace MvcApp
{
    /// <summary>
    /// Represetns this application
    /// </summary>
    static internal partial class App
    {

        /// <summary>
        /// Returns a <see cref="IServiceProvider"/>.
        /// <para>If a <see cref="IServiceScope"/> is specified, then the <see cref="IServiceScope.ServiceProvider"/> is returned.</para>
        /// <para>Otherwise, the <see cref="IServiceProvider"/> is returned from the <see cref="IHttpContextAccessor.HttpContext.RequestServices"/> property.</para>
        /// <para>Finally, and if not a <see cref="HttpContext"/> is available, the <see cref="RootServiceProvider"/> is returned.</para>
        /// </summary>
        static public IServiceProvider GetServiceProvider(IServiceScope Scope = null)
        {
            if (Scope != null)
                return Scope.ServiceProvider;

            HttpContext HttpContext = HttpContextAccessor?.HttpContext;
            return HttpContext?.RequestServices ?? RootServiceProvider;
        }

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
        static public T GetService<T>(IServiceScope Scope = null)
        {
            IServiceProvider ServiceProvider = GetServiceProvider(Scope);
            return ServiceProvider.GetService<T>();
        }
        /// <summary>
        /// Returns the current <see cref="HttpContext"/>
        /// </summary>
        static public HttpContext GetHttpContext() => HttpContextAccessor.HttpContext;

        // ● properties
        /// <summary>
        /// Application settings, coming from appsettings.json
        /// </summary>
        static public AppSettings AppSettings { get; private set; } 
        /// <summary>
        /// The application context
        /// </summary>
        static public IAppContext AppContext { get; private set; }
        /// <summary>
        /// This <see cref="IServiceProvider"/> is the root service provider and is assigned in <see cref="AppStartUp.AddMiddlewares(WebApplication)"/>
        /// <para><strong>WARNING</strong>: do <strong>NOT</strong> use this service provider to resolve "Scoped" services.</para>
        /// </summary>
        static internal IServiceProvider RootServiceProvider { get; private set; }
        /// <summary>
        /// <see cref="IHttpContextAccessor"/> is a singleton service and this property is assigned in <see cref="AppStartUp.AddMiddlewares(WebApplication)"/>
        /// </summary>
        static internal IHttpContextAccessor HttpContextAccessor { get; private set; }
        /// <summary>
        /// The <see cref="IWebHostEnvironment"/>
        /// </summary>
        static public IWebHostEnvironment WebHostEnvironment { get; private set; }

        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        static public string ContentRootPath => WebHostEnvironment.ContentRootPath;
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        static public string WebRootPath => WebHostEnvironment.WebRootPath;
        /// <summary>
        /// The physical path of the output folder
        /// <para>e.g. C:\MyApp\bin\Debug\net9.0\  </para>
        /// </summary>
        static public string BinPath => System.AppContext.BaseDirectory;

        /// <summary>
        /// True when the application is running in development mode
        /// </summary>
        static public bool InDevMode => WebHostEnvironment.IsDevelopment();
    }
}
