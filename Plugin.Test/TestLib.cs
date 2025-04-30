

namespace Plugin.Test
{
    static public partial class TestLib
    {

        /// <summary>
        /// Returns the url to a static file in the plugin folder.
        /// <para>e.g. <c>~/Plugins/PLUGIN_NAME/wwwroot/css/plugin.css</c></para>
        /// <para>where FilePath is <c>css/plugin.css</c></para>
        /// </summary>
        static public string GetStaticFileUrl(string FilePath)
        {
            return Plugin.Descriptor.GetStaticFileUrl(FilePath);
        }
        static public void AddResourceProviders()
        {
            // AppContext.AddResourceProvider()
        }

        static public IMvcAppPlugin Plugin { get; internal set; }
        static public IWebContext AppContext { get; internal set; }
    }
}
