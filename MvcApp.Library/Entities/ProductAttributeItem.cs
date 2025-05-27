namespace MvcApp.Library
{
    /// <summary>
    /// Product attribute along with its value.
    /// <para>CAUTION: Used ONLY in Product Details page</para>
    /// </summary>
    public class ProductAttributeItem
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductAttributeItem()
        {
        }

        /* properties */
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// DisplayOrder
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
