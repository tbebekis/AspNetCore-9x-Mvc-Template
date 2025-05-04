namespace MvcApp.Library
{

    /// <summary>
    /// DataStore is the class for database operations.
    /// <para><strong>NOTE: </strong>This implementation does nothing actually related to database operations. It's here just for demonstration purposes.</para>
    /// </summary>
    static public partial class DataStore
    {
        // ● constants
        public const string SDefaultId = "00000000-0000-0000-0000-000000000000";

        // ● private
        /// <summary>
        /// Returns a localized string based on a specified resource key, e.g. Customer, and the current (Session's) culture code, e.g. el-GR
        /// </summary>
        static string L(string Key, params object[] Args)
        {
            string S = Res.GetString(Key);
            if ((Args != null) && (Args.Length > 0))
                S = string.Format(S, Args);
            return S;
        }

        // ● public
        /// <summary>
        /// Initializes the data store
        /// </summary>
        static public void Initialize()
        {
            // nothing
        }

    }
}
