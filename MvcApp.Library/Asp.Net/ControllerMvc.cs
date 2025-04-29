namespace MvcApp.Library
{


    /// <summary>
    /// Base MVC controller
    /// </summary>
    [Authorize(AuthenticationSchemes = Lib.SCookieAuthScheme)]
    public class ControllerMvc : Controller
    {

        /* methods */
        /// <summary>
        /// Handles the ReturnUrl parameter
        /// </summary>
        protected IActionResult HandleReturnUrl(string ReturnUrl = "")
        {
            // https://github.com/dotnet/aspnetcore/issues/4919
            // https://stackoverflow.com/questions/54979168/invalid-non-ascii-or-control-character-in-header-on-redirect

            // home  
            if (string.IsNullOrWhiteSpace(ReturnUrl))
                ReturnUrl = Url.RouteUrl("Home");

            bool IsEncoded = ReturnUrl.StartsWith('%');

            if (IsEncoded)
                ReturnUrl = ReturnUrl.UrlDecode();

            // prevent open redirection attack
            if (!Url.IsLocalUrl(ReturnUrl))
                ReturnUrl = Url.RouteUrl("Home");

            return Redirect(ReturnUrl);
        }
        /// <summary>
        /// Validates a model and returns the result.
        /// <para>Returns true if the specified model is ok and has no errors.</para>
        /// <para>Returns false if the specified model has errors, collects the error messages and creates the ErrorList entry in the ViewData.</para>
        /// </summary>
        protected bool ValidateModel(object Model, bool EnhanceModelState = false, Func<Dictionary<string, ModelStateEntry>> InvalidEntriesFunc = null)
        {
            if (this.GetErrorList(Model, out var ErrorList, EnhanceModelState, InvalidEntriesFunc))
            {
                Session.AddToErrorList(ErrorList);
                return false;
            }

            return true;
        }

        /* actions */
        /// <summary>
        /// Returns the "not found" view
        /// </summary>
        protected IActionResult NotFoundInternal(string Text = "No Message")
        {
            return View("_NotFound", Text);
        }
        /// <summary>
        /// Returns the "not yet" view
        /// </summary>
        protected IActionResult NotYetInternal(string Text = "No Message")
        {
            return View("_NotYet", Text);
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ControllerMvc()
        {
        }

    }
}
