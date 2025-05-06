namespace tp.Web.Controllers
{

    /// <summary>
    /// Base Ajax controller.
    /// </summary>
    public class ControllerMvcAjax : ControllerMvc, IViewToStringConverter
    { 
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IViewToStringConverter.ViewToString(string ViewName, object Model, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, Model, PlusViewData);
        }
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IViewToStringConverter.ViewToString(string ViewName, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, PlusViewData);
        }

        /// <summary>
        /// Returns the exception text
        /// </summary>
        protected virtual string GetExceptionText(Exception e)
        {
            string Result = WLib.InDevMode ? Sys.GetExceptionText(e) : e.Message;
            return Result;
        }

        /// <summary>
        /// Processes the request.
        /// <para>Sets itself, i.e. this Controller, as the <see cref="AjaxRequest.ViewToStringConverter"/> which is used when processing a Ui request by converting a razor view to HTML.</para>
        /// <para>Calls the static method <see cref="AjaxRequest.Process(AjaxRequest)"/> 
        /// which processes the request by calling all registered <see cref="IAjaxRequestHandler"/> handlers.</para>
        /// <para>And finally assigns the result to the <see cref="HttpPacketResult"/>.</para>
        /// </summary>
        protected virtual HttpPacketResult ProcessRequest(AjaxRequest R)
        {
            R.ViewToStringConverter = this;

            HttpPacketResult Result = new HttpPacketResult();
            try
            {
                AjaxRequestResult RequestResult = AjaxRequest.Process(R);

                Result = HttpPacketResult.SetPacket(RequestResult.GetPacketObject(), true);  
                Result.IsSuccess = RequestResult.IsSuccess;
            }
            catch (Exception e)
            {
                Result.ErrorText = GetExceptionText(e);
            }

            return Result;
        }

        /// <summary>
        /// This is the default execution method.
        /// <para>It just calls <see cref="ProcessRequest(AjaxRequest)"/> 
        /// which in turn uses the static <see cref="AjaxRequest.Process(AjaxRequest)"/> method
        /// delegating the execution of the request to a registered <see cref="IAjaxRequestHandler"/> handler.</para>
        /// </summary>
        protected virtual async Task<JsonResult> DefaultExecute(AjaxRequest R)
        {
            await Task.CompletedTask;
            HttpPacketResult Result = ProcessRequest(R); 
            return Json(Result);
        }
    }

}
