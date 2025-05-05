namespace MvcApp.Library
{


    /// <summary>
    /// Base MVC controller of this application
    /// </summary>
    [Authorize(AuthenticationSchemes = Lib.SCookieAuthScheme)]
    public class AppControllerMvcBase : ControllerMvc
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public AppControllerMvcBase()
        {
        }

    }
}
