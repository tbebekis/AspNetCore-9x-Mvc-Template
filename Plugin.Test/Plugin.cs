using tp.Web;

namespace Plugin.Test
{
    /// <summary>
    /// Represents this plugin
    /// </summary>
    public class Plugin: IMvcAppPlugin
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Plugin()
        {
            TestLib.Plugin = this;
            
        }

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public void Initialize(IWebContext AppContext)
        {
            TestLib.AppContext = AppContext;
        }
        /// <summary>
        /// Use the <c>static</c> <see cref="ViewLocationExpander.AddViewLocation(string)"/> method to add locations.
        /// </summary>
        public void AddViewLocations()
        {
            Descriptor.AddPluginViewLocations();
        }

        /// <summary>
        /// The plugin descriptor
        /// </summary>
        public MvcAppPluginDef Descriptor { get; set; }
    }
}
