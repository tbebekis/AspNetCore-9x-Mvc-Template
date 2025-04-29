namespace MvcApp.Library
{
    /// <summary>
    /// Represents a plugin
    /// </summary>
    public interface IAppPlugin
    {
        /// <summary>
        /// Initializes the plugin
        /// </summary>
        void Initialize();

        /// <summary>
        /// Use the <c>static</c> <see cref="ViewLocationExpander.AddViewLocation(string)"/> method to add locations.
        /// </summary>
        void AddViewLocations();
        /// <summary>
        /// The plugin descriptor
        /// </summary>
        PluginDef Descriptor { get; set; }
    }
}
