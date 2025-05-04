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

        string fDefaultCultureCode;
        string fDefaultCurrencyCode;
        string fDefaultCurrencySymbol;
        string fMoneyFormat;

        protected override void BeforeLoad()
        {
            base.BeforeLoad();
            SupportedCultures.Clear();
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
        /// The default culture, i.e. el-GR
        /// <para>NOTE: This setting is assigned initially by default to any new visitor.</para>
        /// </summary>
        public string DefaultCultureCode
        {
            get => !string.IsNullOrWhiteSpace(fDefaultCultureCode) ? fDefaultCultureCode : Lib.SDefaultCultureCode;           // en-US
            set => fDefaultCultureCode = value;
        }
        /// <summary>
        /// Default currency code, e.g. EUR, USD, etc.
        /// <para>NOTE: This setting is assigned initially by default to any new visitor.</para>
        /// </summary>
        public string DefaultCurrencyCode
        {
            get => !string.IsNullOrWhiteSpace(fDefaultCurrencyCode) ? fDefaultCurrencyCode : Lib.SDefaultCurrencyCode;       // "EUR";
            set => fDefaultCurrencyCode = value;
        }
        /// <summary>
        /// Returns the currency symbol. Used in formatting prices
        /// </summary>
        public string DefaultCurrencySymbol
        {
            get => !string.IsNullOrWhiteSpace(fDefaultCurrencySymbol) ? fDefaultCurrencySymbol : Lib.SDefaultCurrencySymbol;  // "€";
            set => fDefaultCurrencySymbol = value;
        }
        /// <summary>
        /// Format string for formatting money values
        /// </summary>
        public string MoneyFormat
        {
            get => !string.IsNullOrWhiteSpace(fMoneyFormat) ? fMoneyFormat : $"{DefaultCurrencySymbol} 0.00";
            set => fMoneyFormat = value;
        }
        /// <summary>
        /// List of supported cultures
        /// </summary>
        public List<string> SupportedCultures { get; set; } = new List<string>() { "en-US", "el-GR" };
 
        /// <summary>
        /// User cookie settings
        /// </summary>
        public UserCookieSettings UserCookie { get; set; } = new UserCookieSettings();

        /// <summary>
        /// Http related settings
        /// </summary>
        public HttpSettings Http { get; set; } = new HttpSettings();
        /// <summary>
        /// HSTS settings
        /// <para>SEE: https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security </para>
        /// </summary>
        public HSTSSettings HSTS { get; set; } = new HSTSSettings();
    }
}
