namespace MvcApp.Controllers
{
    public class AjaxController: ControllerMvcAjax
    {

        [HttpPost("/Ajax/Execute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;
            HttpPacketResult Result = ProcessRequest(R); new HttpPacketResult();
            return Json(Result);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxController()
        {
        }
    }
}
