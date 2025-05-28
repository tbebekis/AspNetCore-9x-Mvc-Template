namespace MvcApp.Controllers
{


    /// <summary>
    /// Base MVC controller of this application
    /// <para>NOTE: Rrequires authentication</para>
    /// </summary>
    [Authorize(AuthenticationSchemes = Lib.SCookieAuthScheme)]
    public class AppControllerMvcBaseWithAuthentication : ControllerMvc
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public AppControllerMvcBaseWithAuthentication()
        {
        }

    }
}
