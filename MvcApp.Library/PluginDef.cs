namespace MvcApp.Library
{
    /// <summary>
    /// Plugin Definition
    /// </summary>
    public class PluginDef
    {

        string fContentRootUrl;
        string fWebRootUrl;

        /// <summary>
        /// Constructor
        /// </summary>
        public PluginDef(string PluginFolderPath)
        {
            this.PluginFolderPath = PluginFolderPath;
        }

        /// <summary>
        /// The plugin assembly name, e.g. <c>"Plugin.MyPlugin.dll"</c>.
        /// <para><strong>WARNING:</strong> The assembly name must have the form <c>Plugin.PLUGIN_NAME.dll</c> and must be unique.</para>
        /// </summary>
        public string Id { get; set; } 
        /// <summary>
        /// The group the plugin belongs to. Maybe null.
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// The plugin description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The plugin author
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// The plugin load order
        /// </summary>
        public int LoadOrder { get; set; }

        /// <summary>
        /// The physical "root path", i.e. the root folder of the plugin in the applications output or publish folder.
        /// </summary>
        [JsonIgnore]
        public string PluginFolderPath { get; }
        /// <summary>
        /// Full path to plugin assembly file
        /// </summary>
        [JsonIgnore]
        public string PluginAssemblyFilePath { get; set; }
        /// <summary>
        /// The plugin assembly
        /// </summary>
        [JsonIgnore]
        public Assembly PluginAssembly { get; set; }

 
        /// <summary>
        /// Returns the content root url to the plugin folder.
        /// <para>For example, this path: <c>C:\PROJECT-PATH\bin\Debug\Plugins\Plugin.Test </c></para>
        /// <para>becomes: <c>/Plugins/Plugin.Test</c></para>
        /// </summary>
        [JsonIgnore]
        public string ContentRootUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fContentRootUrl))
                   fContentRootUrl = this.GetContentRootUrl();
     
                return fContentRootUrl;
            }
        }
        /// <summary>
        /// Returns the web root url to the  "wwwroot" plugin folder
        /// <para>e.g. <c>~/Plugins/Plugin.Test/wwwroot</c></para>
        /// </summary>
        [JsonIgnore]
        public string WebRootUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(fWebRootUrl))
                    fWebRootUrl = $"{ContentRootUrl}/wwwroot";

                return fWebRootUrl;
            }
        }
    }




}
