using Microsoft.AspNetCore.Localization;

namespace tp.Web
{
    /// <summary>
    /// Represents this library
    /// </summary>
    static public partial class WLib
    {
        static IObjectMapper fObjectMapper;

        /// <summary>
        /// Initializes this class
        /// </summary>
        static public void Initialize(IWebContext WebContext)
        {
            if (WLib.WebContext == null)
            {
                WLib.WebContext = WebContext;
            }
        }

        // ● IWebContext related
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
        static public T GetService<T>(IServiceScope Scope = null) => WebContext.GetService<T>(Scope);
        /// <summary>
        /// Returns the current <see cref="Microsoft.AspNetCore.Http.HttpContext"/>
        /// </summary>
        static public HttpContext GetHttpContext() => WebContext.GetHttpContext();
        /// <summary>
        ///  Returns the current <see cref="Microsoft.AspNetCore.Http.HttpContext.Request"/>
        /// </summary>
        static public HttpRequest GetHttpRequest() => WebContext.GetHttpContext().Request;
        /// <summary>
        /// Returns the current <see cref="Microsoft.AspNetCore.Http.HttpContext.Request.Query"/>
        /// </summary>
        static public IQueryCollection GetQuery() => GetHttpRequest().Query;

        /// <summary>
        /// The <see cref="IWebHostEnvironment"/>
        /// </summary>
        static public IWebHostEnvironment WebHostEnvironment => WebContext.WebHostEnvironment;

        /// <summary>
        /// The physical "root path", i.e. the root folder of the application
        /// <para> e.g. C:\MyApp</para>
        /// </summary>
        static public string ContentRootPath => WebContext.ContentRootPath;
        /// <summary>
        /// The physical "web root" path, i.e. the path to the "wwwroot" folder
        /// <para>e.g. C:\MyApp\wwwwroot</para>
        /// </summary>
        static public string WebRootPath => WebContext.WebRootPath;
        /// <summary>
        /// The physical path of the output folder
        /// <para>e.g. C:\MyApp\bin\Debug\net9.0\  </para>
        /// </summary>
        static public string BinPath => WebContext.BinPath;

        /// <summary>
        /// True when the application is running in development mode
        /// </summary>
        static public bool InDevMode => WebContext.InDevMode;

        // ● Url
        /// <summary>
        /// Returns the relative Url of a request, along with the Query String, url-encoded.
        /// <note>SEE: https://stackoverflow.com/questions/28120222/get-raw-url-from-microsoft-aspnet-http-httprequest </note>
        /// </summary>
        static public string GetRelativeRawUrlEncoded(HttpRequest R = null)
        {
            R = R ?? GetHttpRequest();

            // use the IHttpRequestFeature   
            // the returned value is not UrlDecoded
            string Result = R.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            // if empty string, then build the URL manually
            if (string.IsNullOrEmpty(Result))
                Result = $"{R.PathBase}{R.Path}{R.QueryString}";

            return Result;
        }

        // ● query string 
        /// <summary>
        /// Returns a value from query string, if any, else returns a default value.
        /// </summary>
        static public string GetQueryValue(string Key, string Default = "")
        {
            try
            {   
                IQueryCollection QS = GetQuery();
                return QS != null && QS.ContainsKey(Key) ? QS[Key].ToString() : Default;
            }
            catch
            {
            }

            return Default;

        }
        /// <summary>
        /// Returns a value from query string, if any, else returns a default value.
        /// </summary>
        static public int GetQueryValue(string Key, int Default = 0)
        {
            try
            {
                string S = GetQueryValue(Key, "");
                return !string.IsNullOrWhiteSpace(S) ? Convert.ToInt32(S) : Default;
            }
            catch
            {
            }

            return Default;
        }
        /// <summary>
        /// Returns a value from query string, if any, else returns a default value.
        /// </summary>
        static public bool GetQueryValue(string Key, bool Default = false)
        {
            try
            {
                string S = GetQueryValue(Key, "");
                return !string.IsNullOrWhiteSpace(S) ? Convert.ToBoolean(S) : Default;
            }
            catch
            {
            }

            return Default;

        }

        /// <summary>
        /// Returns the value of a query string parameter.
        /// <para>NOTE: When a parameter is included more than once, e.g. ?page=1&amp;page=2 then the result will be 1,2 hence this function returns an array.</para>
        /// </summary>
        static public string[] GetQueryValueArray(string Key)
        {
            try
            {
                IQueryCollection QS = GetQuery();
                return QS[Key].ToArray();
            }
            catch
            {
            }

            return new string[0];
        }

        // ● Claims
        /// <summary>
        /// Returns the value of a claim, if any, or a default value.
        /// </summary>
        static public T GetClaimValue<T>(IEnumerable<Claim> Claims, string ClaimType, T DefaultValue = default(T))
        {
            Claim Claim = Claims.Where(c => c.Type == ClaimType).FirstOrDefault();
            return GetClaimValue(Claim, DefaultValue);
        }
        /// <summary>
        /// Returns the value of a claim, if not null, else returns a default value.
        /// </summary>
        static public T GetClaimValue<T>(Claim Claim, T DefaultValue = default(T))
        {
             return Claim != null? (T)Convert.ChangeType(Claim.Value, typeof(T)) : DefaultValue;
        }

        // ● Miscs
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the culture code of the current request, e.g. el-GR
        /// </summary>
        static public string Localize(string Key)
        {
            return Res.GetString(Key, Key, WebContext.Culture);
        }
 
        /// <summary>
        /// Returns true when the request is an Ajax request
        /// </summary>
        static public bool IsAjaxRequest(HttpRequest R = null)
        {
            R = R ?? GetHttpRequest();
            return string.Equals(R.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) 
                || string.Equals(R.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
        }
        
        // ● properties
        /// <summary>
        /// Returns the <see cref="IWebContext"/>
        /// </summary>
        static public IWebContext WebContext { get; private set; }
        /// <summary>
        /// Returns the <see cref="IObjectMapper"/> object mapper.
        /// <para>The application may provide its own mapper.</para>
        /// <para>The default mapper is a wrapper to the excellent AutoMaper library.</para>
        /// <para>SEE: https://automapper.org/ </para>
        /// </summary>
        static public IObjectMapper ObjectMapper
        {
            get
            {
                if (fObjectMapper == null)
                    fObjectMapper = new ObjectMapper();

                return fObjectMapper;
            }
            set
            {
                fObjectMapper = value;
            }
        }
        /// <summary>
        /// The <see cref="CultureInfo"/> culture of the current request.
        /// </summary>
        static public CultureInfo Culture
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
        /// Returns the Id of the current HTTP request
        /// </summary>
        static public string RequestId
        {
            get
            {
                string Result = Activity.Current?.Id ?? GetHttpContext().TraceIdentifier;
                if (!string.IsNullOrWhiteSpace(Result) && Result.StartsWith('|'))
                    Result = Result.Remove(0, 1);
                return Result;

            }
        }
    }
}
