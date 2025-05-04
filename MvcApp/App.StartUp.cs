 

namespace MvcApp
{
    /*
    Service Lifetime:           
        ● Singleton : once per application
        ● Scoped    : once per HTTP Request
        ● Transient : each time is requested     
     */


    static internal partial class App
    {
 
        /// <summary>
        /// Static file response callback.
        /// </summary>
        static void StaticFileResponseProc(StaticFileResponseContext StaticFilesContext)
        {
            StaticFilesContext.Context.Response.Headers.Append(HeaderNames.CacheControl, AppSettings.Http.StaticFilesCacheControl);
        }

        // ● public
        /// <summary>
        /// Initializes other libraries and plugins
        /// </summary>
        static public void Initialize()
        {
            if (App.AppContext == null)
            {
                App.AppContext = new AppContext();
                WLib.Initialize(App.AppContext);
                Lib.Initialize(App.AppContext);                
                App.InitializePlugins();
            }
        }
 

        /// <summary>
        /// Add services to the container.
        /// </summary>
        static public void AddServices(WebApplicationBuilder builder)
        {
            App.AppSettings = new AppSettings();

            // ● custom services 
            builder.Services.AddScoped<IUserRequestContext, UserRequestContext>();
 
            // ● HttpContext
            builder.Services.AddHttpContextAccessor();      // singleton

            // ● Cookie Authentication  
            AuthenticationBuilder AuthBuilder = builder.Services.AddAuthentication(options => {
                options.DefaultScheme = Lib.SCookieAuthScheme;
                options.DefaultAuthenticateScheme = options.DefaultScheme;
                options.DefaultChallengeScheme = options.DefaultScheme;
            });

            AuthBuilder.AddCookie(Lib.SCookieAuthScheme, options => {
 
                TimeSpan Expiration = App.AppSettings.UserCookie.ExpirationHours <= 0 ? TimeSpan.FromDays(365) : TimeSpan.FromHours(App.AppSettings.UserCookie.ExpirationHours);

                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.ReturnUrlParameter = "ReturnUrl";
                options.EventsType = typeof(UserCookieAuthEvents);
                options.ExpireTimeSpan = Expiration;
                //options.SlidingExpiration = true;

                options.Cookie.Name = App.SAuthCookieName;       // cookie name


                /*
                options.Cookie.Name = $"{Assembly.GetEntryAssembly().GetName().Name}.UserCookie";       // cookie name
                options.Cookie.IsEssential = App.AppSettings.UserCookie.IsEssential;
                options.Cookie.HttpOnly = App.AppSettings.UserCookie.HttpOnly;
                options.Cookie.SameSite = App.AppSettings.UserCookie.SameSite;
                //options.Cookie.Expiration = Expiration;   // Exception: OptionsValidationException: Cookie.Expiration is ignored, use ExpireTimeSpan instead
                */
            });

            builder.Services.AddScoped<UserCookieAuthEvents>();

            // ● Session
            builder.Services.AddSession(options => {
                options.Cookie.Name = App.SSessionCookieName;    // cookie name
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;  // Make the session cookie essential
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
            });

            // ● Cookie Policy
            builder.Services.Configure<CookiePolicyOptions>(o =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // If CheckConsentNeeded is set to true, then the IsEssential should be also set to true, for any Cookie's CookieOptions setting.
                // SEE: https://stackoverflow.com/questions/52456388/net-core-cookie-will-not-be-set
                o.CheckConsentNeeded = context => true;

                // Set the secure flag, which Chrome's changes will require for SameSite none.
                // Note this will also require you to be running on HTTPS.
                o.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;

                // Set the cookie to HTTP only which is good practice unless you really do need
                // to access it client side in scripts.
                o.HttpOnly = HttpOnlyPolicy.Always;

                // Add the SameSite attribute, this will emit the attribute with a value of none.
                o.Secure = CookieSecurePolicy.Always;
            });

            // ● Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            /*
            // ● Request Localization
            // https://www.codemag.com/Article/2009081/A-Deep-Dive-into-ASP.NET-Core-Localization
            builder.Services.Configure((RequestLocalizationOptions options) => {

                var Provider = new CustomRequestCultureProvider(async (HttpContext) => {
                    await Task.Yield();

                    // retrieve the ClaimsPrincipal from the cookies
                    // note: this is dependent on how your authentication is set up
                    var authResult = await HttpContext.AuthenticateAsync(Lib.SCookieAuthScheme);
                    if (!authResult.Succeeded)
                        return null;

                    IRequestContext RequestContext = GetService<IUserRequestContext>();
                    return new ProviderCultureResult(RequestContext.CultureCode);
                });

                var Cultures = Lib.GetSupportedCultures();
                options.DefaultRequestCulture = new RequestCulture(App.AppSettings.DefaultCultureCode);
                options.SupportedCultures = Cultures;
                options.SupportedUICultures = Cultures;
                options.RequestCultureProviders.Insert(0, Provider);
            });
            */



            // ● IHttpClientFactory
            builder.Services.AddHttpClient();



            // ● MVC
            IMvcBuilder MvcBuilder = builder.Services.AddControllersWithViews();
            builder.Services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });
            MvcBuilder.AddRazorRuntimeCompilation();

            MvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new DefaultNamingStrategy() };   // no camelCase
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            // ● Plugins
            LoadPlugins(MvcBuilder.PartManager);

        }
        /// <summary>
        /// Add middlewares the the pipeline
        /// </summary>
        static public void AddMiddlewares(WebApplication app)
        {
            App.RootServiceProvider = (app as IApplicationBuilder).ApplicationServices;
            App.HttpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            App.WebHostEnvironment = app.Environment;     
 
            App.Initialize();
            //Test();

            //----------------------------------------------------------------------------------------
            // Middlewares
            //----------------------------------------------------------------------------------------

            


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }
  

            /*
            app.UseHttpsRedirection();
           app.UseStaticFiles();
           // app.UseCookiePolicy();

           app.UseRouting();
           // app.UseRateLimiter();
           // app.UseRequestLocalization();
           // app.UseCors();

           app.UseAuthentication();
           app.UseAuthorization();
           // app.UseSession();
           // app.UseResponseCompression();
           // app.UseResponseCaching(); 
             */

            app.UseHsts();
            app.UseHttpsRedirection();

            // ● static files - wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = StaticFileResponseProc
            });
            // ● static files - OutputPath\Plugins
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(App.BinPath, "Plugins")),
                RequestPath = new PathString("/Plugins"),
                OnPrepareResponse = StaticFileResponseProc
            });

            // ● Cookie Policy
            app.UseCookiePolicy();

            // ● endpoint resolution middlware
            app.UseRouting();
 
            app.UseAuthentication();
            app.UseAuthorization();
 
            // ● Request Localization 
            // UseRequestLocalization initializes a RequestLocalizationOptions object. 
            // On every request the list of RequestCultureProvider in the RequestLocalizationOptions is enumerated 
            // and the first provider that can successfully determine the request culture is used.
            // SEE: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization#localization-middleware
            app.UseRequestLocalization((RequestLocalizationOptions options) =>
            {
                var Cultures = Lib.GetSupportedCultures();
                options.DefaultRequestCulture = new RequestCulture(App.AppSettings.DefaultCultureCode);
                options.SupportedCultures = Cultures;
                options.SupportedUICultures = Cultures;
                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });

            // ● Session
            app.UseSession();

            // ● MVC 
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
        }
    }
}
