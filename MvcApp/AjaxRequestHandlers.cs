namespace MvcApp
{
    /// <summary>
    /// Helper just to demonstrate the use of <see cref="AjaxRequest"/> with <see cref="IAjaxRequestHandler"/>.
    /// </summary>
    static internal class AjaxRequestHandlers
    {

        /// <summary>
        /// Represents an object that processes an <see cref="AjaxRequest"/> and returns an <see cref="AjaxRequestResult"/>
        /// </summary>
        public class AjaxHandler : IAjaxRequestHandler
        {

            /// <summary>
            /// Processes an <see cref="AjaxRequest"/> and if it handles the request returns an <see cref="AjaxRequestResult"/>. Else returns null.
            /// </summary>
            public AjaxRequestResult Process(AjaxRequest Request)
            {
                if (Sys.IsSameText(Request.OperationName, "Operation1"))
                    return Operation1(Request);
                return null;
            }
        }

        static AjaxRequestResult Operation1(AjaxRequest Request)
        {
            AjaxRequestResult Result = new AjaxRequestResult(Request.OperationName);
            Result["RequestId"] = Request.Id;
            Result["AppName"] = "MvcApp";
            Result["Html"] = false;
            Result["Number"] = 123.45;
            Result["Date"] = DateTime.Now;

            Result.IsSuccess = true;

            return Result;
        }

        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        static AjaxRequestHandlers()
        {
        }

        static public void Initialize()
        {
            AjaxRequest.Register(new AjaxHandler());
        }
    }
}
