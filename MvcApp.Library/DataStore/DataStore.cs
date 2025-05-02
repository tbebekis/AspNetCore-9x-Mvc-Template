namespace MvcApp.Library
{
    static public partial class DataStore
    {
        // ● constants
        public const string SDefaultId = "00000000-0000-0000-0000-000000000000";

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

        static public void Initialize()
        {

        }



    }
}
