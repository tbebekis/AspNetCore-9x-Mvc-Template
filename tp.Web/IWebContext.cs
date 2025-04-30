namespace tp.Web
{

    /// <summary>
    /// Represents the web application context.
    /// <para>There is a single instance of this object, which is assigned to the <see cref="WLib.WebContext"/> property.</para>
    /// </summary>
    public interface IWebContext: IAppContext
    {
        // ● methods
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
        T GetService<T>(IServiceScope Scope = null);

        /// <summary>
        /// Returns the current <see cref="HttpContext"/>
        /// </summary>
        HttpContext GetHttpContext();

        // ● properties
        /// <summary>
        /// The <see cref="IWebHostEnvironment"/>
        /// </summary>
        IWebHostEnvironment WebHostEnvironment { get; }     
 
        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        string ContentRootPath { get; }
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        string WebRootPath { get; }
        /// <summary>
        /// The physical path of the output folder
        /// <para>e.g. C:\MyApp\bin\Debug\net9.0\  </para>
        /// </summary>
        string BinPath { get; }

        /// <summary>
        /// True when the application is running in development mode
        /// </summary>
        bool InDevMode { get; }

        /// <summary>
        /// The <see cref="CultureInfo"/> culture of the current request.
        /// <para>CAUTION: The culture of each HTTP Request is set by a lambda in ConfigureServices().
        /// This property here uses that setting to return its value.
        /// </para>
        /// </summary>
        CultureInfo Culture { get; }
    }
}
