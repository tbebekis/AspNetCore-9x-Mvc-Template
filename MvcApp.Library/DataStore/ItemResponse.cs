namespace MvcApp.Library
{
    /// <summary>
    /// A <see cref="DataStoreResponse"/> response for a single item
    /// </summary>
    public class ItemResponse<T> : DataStoreResponse
    {
        /// <summary>
        /// The item
        /// </summary>
        public T Item { get; set; }
    }
}
