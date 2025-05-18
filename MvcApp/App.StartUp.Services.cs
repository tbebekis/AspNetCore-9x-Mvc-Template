namespace MvcApp
{
    static public partial class App
    {
        static void AppSettings_Loaded(object sender, EventArgs e)
        {
            ViewLocationExpander.Theme = App.AppSettings.Theme;
            PageBuilderService.Seo = App.AppSettings.Seo;
        }

        // ● public
        /// <summary>
        /// Add services to the container.
        /// </summary>
        static public void AddServices(WebApplicationBuilder builder)
        {
            App.AppSettings = new AppSettings();
            App.AppSettings.Loaded += AppSettings_Loaded;
            AppSettings_Loaded(null, null);

            // ● custom services 
            builder.Services.AddScoped<IUserRequestContext, UserRequestContext>();
            builder.Services.AddScoped<PageBuilderService>();

            // ● HttpContext - NOTE: is singleton
            builder.Services.AddHttpContextAccessor();

            // ● ActionContext - see: https://github.com/aspnet/mvc/issues/3936
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();  // see: https://github.com/aspnet/mvc/issues/3936

            // ● Memory Cache - NOTE: is singleton
            // NOTE: Distributed Cache is required for Session to function properly
            // SEE: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state#configure-session-state
            builder.Services.AddDistributedMemoryCache(); // AddMemoryCache(); 

            // ● Cookie Authentication  
            if (App.AppSettings.UseAuthentication)
            {
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
                    options.Cookie.IsEssential = App.AppSettings.UserCookie.IsEssential;
                    options.Cookie.HttpOnly = App.AppSettings.UserCookie.HttpOnly;
                    options.Cookie.SameSite = App.AppSettings.UserCookie.SameSite;
                    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                });

                // TODO: check if this is needed
                builder.Services.AddScoped<UserCookieAuthEvents>();

                //● authorization
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy(App.PolicyAuthenticated, policy => { policy.RequireAuthenticatedUser(); });
                });
            }

            // ● Session
            builder.Services.AddSession(options => {
                options.Cookie.Name = App.SSessionCookieName;       // cookie name
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;                  // Make the session cookie essential
                options.IdleTimeout = TimeSpan.FromMinutes(App.AppSettings.SessionTimeoutMinutes);
            });

            // ● Localization
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // ● IHttpClientFactory
            builder.Services.AddHttpClient();

            // ● HSTS
            if (!builder.Environment.IsDevelopment())
            {
                HSTSSettingsBase HSTS = App.AppSettings.HSTS;
                builder.Services.AddHsts(options =>
                {
                    options.Preload = HSTS.Preload;
                    options.IncludeSubDomains = HSTS.IncludeSubDomains;
                    options.MaxAge = TimeSpan.FromHours(HSTS.MaxAgeHours >= 1 ? HSTS.MaxAgeHours : 1);
                    if (HSTS.ExcludedHosts != null && HSTS.ExcludedHosts.Count > 0)
                    {
                        foreach (string host in HSTS.ExcludedHosts)
                            options.ExcludedHosts.Add(host);
                    }
                });
            }

            // ● Themes support
            if (ViewLocationExpander.UseThemes)
            {
                builder.Services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });
            }

            // ● MVC
            IMvcBuilder MvcBuilder = builder.Services.AddControllersWithViews(options => {
                options.ModelBinderProviders.Insert(0, new AppModelBinderProvider());
                options.Filters.Add<ActionExceptionFilter>();
            });

            MvcBuilder.AddRazorRuntimeCompilation();

            MvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new DefaultNamingStrategy() };   // no camelCase
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            // ● Plugins
            LoadPluginDefinitions();
            LoadPluginAssemblies();
            AddPluginsToApplicationPartManager(MvcBuilder.PartManager);
        }
    }
}
