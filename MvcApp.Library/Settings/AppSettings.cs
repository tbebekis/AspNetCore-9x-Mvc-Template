namespace MvcApp.Library
{

    /// <summary>
    /// Application settings.
    /// <para>Watches for changes in the settings file and reloads this instance.</para>
    /// <para><strong>NOTE: </strong> If this class contains lists, 
    /// then override the <see cref="BeforeLoad()"/> method and clean those lists.</para>
    /// </summary>
    public class AppSettings : AppSettingsBase
    {
        const string SFileName = "MvcAppSettings.json";
 

        protected override void BeforeLoad()
        {
            base.BeforeLoad();
            SupportedCultures.Clear();
        }
        protected override void AfterLoad()
        {
            base.AfterLoad();
            if (Loaded != null)
                Loaded(this, EventArgs.Empty);
        }

        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public AppSettings(string Folder = "", string FileName = SFileName, bool IsReloadable = true)
            : base(Folder, FileName, IsReloadable)
        {
        }

        // ● properties
        /// <summary>
        /// Whether or not to use Cookie authentication
        /// </summary>
        public bool UseAuthentication { get; set; } = true;
        /// <summary>
        /// The theme currently in use. Defaults to null or empty string, meaning no themes at all.
        /// </summary>
        public string Theme { get; set; }
        /// <summary>
        /// List of supported cultures
        /// </summary>
        public List<string> SupportedCultures { get; set; } = new List<string>() { "en-US", "el-GR" };
 
        /// <summary>
        /// Default settings
        /// </summary>
        public DefaultSettingsBase Defaults { get; set; } = new DefaultSettingsBase();
        /// <summary>
        /// User cookie settings
        /// </summary>
        public UserCookieSettingsBase UserCookie { get; set; } = new UserCookieSettingsBase();
        /// <summary>
        /// Http related settings
        /// </summary>
        public HttpSettingsBase Http { get; set; } = new HttpSettingsBase();
        /// <summary>
        /// HSTS settings
        /// <para>SEE: https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security </para>
        /// </summary>
        public HSTSSettingsBase HSTS { get; set; } = new HSTSSettingsBase();
        /// <summary>
        /// SEO related settings
        /// </summary>
        public SeoSettingsBase Seo { get; set; } = new SeoSettingsBase();

        // ● events
        /// <summary>
        /// Called when the settings are loaded
        /// </summary>
        public event EventHandler Loaded;
    }
}
