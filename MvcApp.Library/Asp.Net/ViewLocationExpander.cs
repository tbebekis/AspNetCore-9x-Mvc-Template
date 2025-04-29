namespace MvcApp.Library
{
    /// <summary>
    /// A location expander. It adds the views locations in the view search locations of the <see cref="RazorViewEngine"/>.
    /// <para>NOTE: Use the <c>static</c> <see cref="ViewLocationExpander.AddViewLocation(string)"/> method to add locations.</para>
    /// <para>NOTE: The order of view location matters.</para>
    /// <para>SEE: View Discavery at https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-9.0#view-discovery </para>
    /// </summary>
    public class ViewLocationExpander : IViewLocationExpander
    {
        static List<string> LocationList = new List<string>();

        /* public */
        /// <summary>
        /// This is called by the <see cref="RazorViewEngine"/> when it looks to find a view (any kind of view, i.e. layout, normal view, or partial).
        /// <para>Information about the view, action and controller is passed in the <see cref="ViewLocationExpanderContext"/> parameter.</para>
        /// <para>This call gives a chance to search and locate the view and pass back the view location.</para>
        /// <para>NOTE: The order of view location matters.</para>
        /// </summary>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (LocationList.Count > 0)
                viewLocations = LocationList.Union(viewLocations);

            return viewLocations;
        }
        /// <summary>
        /// More or less useless. 
        /// <para>It is used in adding values to the <see cref="ViewLocationExpanderContext.Values"/> dictionary 
        /// in order to be available when the  <see cref="ExpandViewLocations(ViewLocationExpanderContext, IEnumerable{string})"/> method is called. </para>
        /// </summary>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            /*
            if (!IsNonThemeableArea(context.AreaName)) // some areas, such as Admin, may be not themeable
            {
                context.Values[SThemeKey] = Theme;
            } 
             */
        }

        /// <summary>
        /// Adds a location to the internal list.
        /// <para>Example for normal views:
        /// <code>
        ///     // 0 = view file name
        ///     // 1 = controller name
        ///     
        ///     AddLocation("/Views/{1}/{0}.cshtml");   
        ///     AddLocation("/Views/Shared/{0}.cshtml");
        ///     AddLocation("/Views/Ajax/{0}.cshtml");
        /// </code>
        /// </para>
        /// <para>Example for themeable views:
        /// <code>
        ///     // 0 = view file name
        ///     // 1 = controller name
        /// 
        ///    AddLocation("/{ThemesFolder}/{Theme}/Views/{1}/{0}.cshtml");
        ///    AddLocation("/{ThemesFolder}/{Theme}/Views/Shared/{0}.cshtml");
        /// </code>
        /// </para>
        /// </summary>
        static public void AddViewLocation(string Location)
        {
            if (!LocationList.ContainsText(Location))
                LocationList.Add(Location);
        }
        /// <summary>
        /// Adds Plugin view locations to the internal list.
        /// </summary>
        static public void AddPluginViewLocations(PluginDef Def)
        { 
            string PluginFolder =  Def.PluginFolderPath.Replace(Lib.BinPath, string.Empty)
                .TrimStart('\\')
                .TrimEnd('\\')
                .TrimStart('/')
                .TrimEnd('/')
                .Replace('\\', '/');

            PluginFolder = '/' + PluginFolder;

            string Location = "PLUGIN_FOLDER/Views/{1}/{0}.cshtml".Replace("PLUGIN_FOLDER", PluginFolder);
            AddViewLocation(Location);

            Location = "PLUGIN_FOLDER/Views/Shared/{0}.cshtml".Replace("PLUGIN_FOLDER", PluginFolder);
            AddViewLocation(Location);

             
 
        }
    }
}
