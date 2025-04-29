namespace tp.Web
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
        /// Invoked by a <see cref="RazorViewEngine"/> to determine the values that would be consumed by this instance
        /// of <see cref="IViewLocationExpander"/>. The calculated values are used to determine if the view location
        /// has changed since the last time it was located.
        /// </summary>
        /// <param name="context">The <see cref="ViewLocationExpanderContext"/> for the current view location
        /// expansion operation.</param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
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
            string S = LocationList.FirstOrDefault(s => string.Compare(s, Location, true) == 0);
            if (string.IsNullOrWhiteSpace(S))
                LocationList.Add(Location);
        }
    }
}
