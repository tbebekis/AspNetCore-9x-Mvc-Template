namespace MvcApp.Library
{
    /// <summary>
    /// A <see cref="DataResult"/> response for a single item
    /// </summary>
    public class ItemDataResult<T> : DataResult
    {
        /// <summary>
        /// The item
        /// </summary>
        public T Item { get; set; }
    }
}
