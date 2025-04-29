namespace MvcApp.Library
{
    static public partial class DataStore
    {

        static public void Initialize()
        {

        }

        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// </summary>
        static public IRequestor GetRequestor(string Id)
        {
            return Lib.AppContext.DefaultRequestor;
        }
    }
}
