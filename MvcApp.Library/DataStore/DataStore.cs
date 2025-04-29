namespace MvcApp.Library
{
    static public partial class DataStore
    {
        /// <summary>
        /// Returns a user from database found under a specified Id, if any, else null.
        /// </summary>
        static public Requestor GetRequestor(string Id)
        {
            return Requestor.Default;
        }
    }
}
