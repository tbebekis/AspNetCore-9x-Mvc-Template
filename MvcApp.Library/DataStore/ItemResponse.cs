namespace MvcApp.Library
{
    /// <summary>
    /// Data service response for a single item
    /// </summary>
    public class ItemResponse<T> : DataStoreResponse
    {
        /// <summary>
        /// The item
        /// </summary>
        public T Item { get; set; }
    }
}
