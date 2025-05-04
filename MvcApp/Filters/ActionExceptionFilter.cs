namespace MvcApp.Filters
{

    /// <summary>
    /// Global exception filter for controller actions. Use this instead of try-catch blocks inside action methods.    
    /// <para>
    /// Exception filters: <br />
    ///  ● Handle unhandled exceptions that occur in Razor Page or controller creation, model binding, action filters, or action methods. <br />
    ///  ● Do not catch exceptions that occur in resource filters, result filters, or MVC result execution.  
    /// </para>
    /// <para></para>
    /// <para>
    /// SEE: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1#exception-filters
    /// </para>
    /// <para>To register</para>
    /// <para><code> services.AddControllersWithViews(o =&gt; { o.Filters.Add&lt;ActionExceptionFilter&gt;(); })
    /// </code></para>
    /// </summary>
    internal class ActionExceptionFilter : IExceptionFilter
    {
        Type ControllerBaseType = typeof(ControllerBase);
        Type ControllerType = typeof(Controller);

        IWebHostEnvironment HostEnvironment;
        IModelMetadataProvider ModelMetadataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionExceptionFilter(IWebHostEnvironment HostEnvironment, IModelMetadataProvider ModelMetadataProvider)
        {
            this.HostEnvironment = HostEnvironment;
            this.ModelMetadataProvider = ModelMetadataProvider;
        }


        /// <summary>
        /// Called after an action has thrown a <see cref="Exception"/> 
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            ControllerActionDescriptor ActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            TypeInfo ControllerTypeInfo = ActionDescriptor.ControllerTypeInfo;

            bool IsApi = ControllerTypeInfo.IsSubclassOf(ControllerBaseType) && !ControllerTypeInfo.IsSubclassOf(ControllerType);
            bool IsMvc = ControllerTypeInfo.IsSubclassOf(ControllerBaseType) && ControllerTypeInfo.IsSubclassOf(ControllerType);

            string RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            if (!string.IsNullOrWhiteSpace(RequestId) && RequestId.StartsWith('|'))
                RequestId = RequestId.Remove(0, 1);

            ActionExceptionFilterContext FilterContext = new ActionExceptionFilterContext(IsApi, RequestId, context, ActionDescriptor, ControllerTypeInfo, ModelMetadataProvider, HostEnvironment.IsDevelopment());

            HandlerFunc?.Invoke(FilterContext);

            context.ExceptionHandled = true;
            Logger.Error(ActionDescriptor.ControllerName, ActionDescriptor.ActionName, RequestId, context.Exception);
        }

        /* properties */
        /// <summary>
        /// A replacable static handler function for global exceptions. It offers a default error handling.
        /// </summary>
        static public Action<ActionExceptionFilterContext> HandlerFunc = (Context) =>
        {
            // it is an Api controller or an AJAX request
            /*
            if (Context.IsWebApi || WSys.IsAjax(Context.ExceptionContext.HttpContext.Request))
            {
                HttpPacketResult ActionResult = new HttpPacketResult();
                ActionResult.IsSuccess = false;
                ActionResult.ErrorText = Context.ExceptionContext.Exception.Message;

                // NO, we do NOT want an invalid HTTP StatusCode. It is a valid HTTP Response.
                // We just have an action result with errors, so any error should be recorded by our HttpActionResult and delivered to the client.
                //context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; 
                Context.ExceptionContext.HttpContext.Response.ContentType = "application/json";
                Context.ExceptionContext.Result = new JsonResult(ActionResult);
            }
            // handle errors when it's an MVC Controller
            else // IsMvc controller
            */
            {
                /* SEE: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1#exception-filters */
                var Result = new ViewResult();
                Result.ViewName = "Error";
                Result.ViewData = new ViewDataDictionary(Context.ModelMetadataProvider, Context.ExceptionContext.ModelState);
                Result.ViewData.Add("Exception", Context.ExceptionContext.Exception);
                Result.ViewData.Add("RequestId", Context.RequestId);
                Context.ExceptionContext.Result = Result;
            }

        };
    }

}
