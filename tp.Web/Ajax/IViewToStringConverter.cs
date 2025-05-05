namespace tp.Web.Ajax
{
    /// <summary>
    /// Represents an object that processes a razor view and returns the result HTML.
    /// </summary>
    public interface IViewToStringConverter
    {
        /// <summary>
        /// Renders a partial view to a string.
        /// </summary>
        string ViewToString(string ViewName, object Model, IDictionary<string, object> PlusViewData = null);
        /// <summary>
        /// Renders a partial view to a string.
        /// </summary>
        string ViewToString(string ViewName, IDictionary<string, object> PlusViewData = null);
    }
}
