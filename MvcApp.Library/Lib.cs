namespace MvcApp.Library
{

    /// <summary>
    /// Represents this library
    /// </summary>
    static public partial class Lib
    {
 
        /// <summary>
        /// Initializes the library
        /// </summary>
        static public void Initialize(IMvcAppContext AppContext, AppSettings AppSettings)
        {
            if (Lib.AppContext == null)
            {
                Lib.AppContext = AppContext;                  
                Lib.AppSettings = AppSettings;
            }
        }
 
        static public bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT 
                || Environment.OSVersion.Platform == PlatformID.Win32Windows 
                || Environment.OSVersion.Platform == PlatformID.WinCE;
        }
        static public bool IsLinux()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }
 
        // ● IAppContext related
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
            return AppContext.GetService<T>(Scope);
        }
        /// <summary>
        /// Returns the current <see cref="HttpContext"/>
        /// </summary>
        static public HttpContext GetHttpContext() => AppContext.GetHttpContext();

        /// <summary>
        /// The <see cref="IWebHostEnvironment"/>
        /// </summary>
        static public IWebHostEnvironment WebHostEnvironment => AppContext.WebHostEnvironment;
 
        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        static public string ContentRootPath => AppContext.ContentRootPath;
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        static public string WebRootPath => AppContext.WebRootPath;
        /// <summary>
        /// The physical path of the output folder
        /// <para>e.g. C:\MyApp\bin\Debug\net9.0\  </para>
        /// </summary>
        static public string BinPath => AppContext.BinPath;

        /// <summary>
        /// True when the application is running in development mode
        /// </summary>
        static public bool InDevMode => AppContext.InDevMode;

        // ● miscs
        /// <summary>
        /// Returns the list of supported cultures.
        /// <para>This setting may come from application settings, a database or elsewhere.</para>
        /// </summary>
        static public List<string> GetSupportedCultureCodes()
        {
            List<string> Result = new List<string>();
            if (AppSettings.SupportedCultures != null && AppSettings.SupportedCultures.Count > 0)
                Result.AddRange(AppSettings.SupportedCultures);
            else
                Result.Add(AppSettings.DefaultCultureCode);

            return Result;
        }
        /// <summary>
        /// Returns the list of supported cultures.
        /// <para>This setting may come from application settings, a database or elsewhere.</para>
        /// </summary>
        static public List<CultureInfo> GetSupportedCultures()
        {
            List<string> List =  GetSupportedCultureCodes();
            List<CultureInfo> Result = new List<CultureInfo>();

            foreach (string CultureCode in List)
                Result.Add(CultureInfo.GetCultureInfo(CultureCode));   

            return Result;
        }

        // ● properties
        /// <summary>
        /// The context of this application
        /// </summary>
        static public IMvcAppContext AppContext { get; private set; }
        /// <summary>
        /// Application settings, coming from appsettings.json
        /// </summary>
        static public AppSettings AppSettings { get; private set; }
    }
}
