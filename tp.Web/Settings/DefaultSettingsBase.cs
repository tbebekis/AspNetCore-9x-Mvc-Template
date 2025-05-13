namespace tp.Web
{

    /// <summary>
    /// Default settings
    /// </summary>
    public class DefaultSettingsBase
    {
        string fCultureCode;
        string fCurrencyCode;
        string fCurrencySymbol;
        string fMoneyFormat;

        /// <summary>
        /// The default culture, i.e. el-GR
        /// <para>NOTE: This setting is assigned initially by default to any new visitor.</para>
        /// </summary>
        public string CultureCode
        {
            get => !string.IsNullOrWhiteSpace(fCultureCode) ? fCultureCode : "en-US";           // "en-US";  
            set => fCultureCode = value;
        }
        /// <summary>
        /// Default currency code, e.g. EUR, USD, etc.
        /// <para>NOTE: This setting is assigned initially by default to any new visitor.</para>
        /// </summary>
        public string CurrencyCode
        {
            get => !string.IsNullOrWhiteSpace(fCurrencyCode) ? fCurrencyCode : "EUR";       // "EUR";
            set => fCurrencyCode = value;
        }
        /// <summary>
        /// Returns the currency symbol. Used in formatting prices
        /// </summary>
        public string CurrencySymbol
        {
            get => !string.IsNullOrWhiteSpace(fCurrencySymbol) ? fCurrencySymbol : "€";  // "€";
            set => fCurrencySymbol = value;
        }
        /// <summary>
        /// Format string for formatting money values
        /// </summary>
        public string MoneyFormat
        {
            get => !string.IsNullOrWhiteSpace(fMoneyFormat) ? fMoneyFormat : $"{CurrencySymbol} 0.00";
            set => fMoneyFormat = value;
        }
        /// <summary>
        /// The eviction timeout of an entry from the cache, in minutes. 
        /// <para>Defaults to 0 which means "use the timeouts of the internal implementation".</para>
        /// </summary>
        public int CacheTimeoutMinutes { get; set; }
    }
}
