namespace tp.Web
{
    static public partial class WLib
    {
        static public void Initialize(IWebContext WebContext)
        {
            if (WLib.WebContext == null)
            {
                WLib.WebContext = WebContext;
            }
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

        /* query string */

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


        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the culture code of the current request, e.g. el-GR
        /// </summary>
        static public string Localize(string Key)
        {
            return Res.GetString(Key, Key, WebContext.Culture);
        }
        /// <summary>
        /// Returns the <see cref="IWebContext"/>
        /// </summary>
        static public IWebContext WebContext { get; private set; }
    }
}
