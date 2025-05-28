namespace MvcApp.Controllers
{


    /// <summary>
    /// Base MVC controller of this application.
    /// <para>NOTE: Does NOT require authentication</para>
    /// </summary>    
    public class AppControllerMvcBase : ControllerMvc
    {
        protected virtual RedirectToActionResult RedirectToErrorPage(string ErrorMessage)
        {
            ErrorViewModel Model = new ErrorViewModel();
            Model.ErrorMessage = ErrorMessage;
            Model.RequestId = WLib.RequestId;

            string JsonText = tp.Json.Serialize(Model);

            TempData["ErrorModel"] = JsonText;

            return RedirectToAction("Error", "Home", new { });
        }



        // ● construction
            /// <summary>
            /// Constructor
            /// </summary>
        public AppControllerMvcBase()
        {
        }

    }
}
