namespace MvcApp.Library
{
    /// <summary>
    /// Represents a plugin
    /// </summary>
    public interface IMvcAppPlugin
    {
 
        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public void Initialize(IWebContext AppContext)

        /// <summary>
        /// Use the <c>static</c> <see cref="ViewLocationExpander.AddViewLocation(string)"/> method to add locations.
        /// </summary>
        void AddViewLocations();
        /// <summary>
        /// The plugin descriptor
        /// </summary>
        MvcAppPluginDef Descriptor { get; set; }
    }
}
