namespace MvcApp.Library
{
    /// <summary>
    /// A <see cref="DataStoreResponse"/> response for lists of items.
    /// </summary>
    public class ListResponse<T> : DataStoreResponse
    {
        /// <summary>
        /// The number of total items when this is a paged response. Used only with paged responses.
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// The list of items
        /// </summary>
        public List<T> List { get; set; } = new List<T>();
    }
}
