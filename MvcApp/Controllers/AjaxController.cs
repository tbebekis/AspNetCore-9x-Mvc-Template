namespace MvcApp.Controllers
{
    public class AjaxController: ControllerMvcAjax
    {

        [HttpPost("/Ajax/Execute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            //await Task.CompletedTask;
            //HttpPacketResult Result = ProcessRequest(R);
            //return Json(Result);
            return await DefaultExecute(R);
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxController()
        {
        }
    }
}
