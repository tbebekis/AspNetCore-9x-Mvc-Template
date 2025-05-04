namespace MvcApp.Library
{

    /// <summary>
    /// Represents this library
    /// </summary>
    static public partial class Lib
    {
        // ● public
        /// <summary>
        /// Initializes the library
        /// </summary>
        static public void Initialize(IMvcAppContext AppContext)
        {
            if (Lib.AppContext == null)
            {
                Lib.AppContext = AppContext;                 
            }
        }

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
        static public AppSettings AppSettings => AppContext.AppSettings;
    }
}
