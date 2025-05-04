namespace MvcApp.Filters
{

    /// <summary>
    /// Context for the <see cref="ActionExceptionFilter"/>
    /// </summary>
    internal class ActionExceptionFilterContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionExceptionFilterContext(bool IsWebApi,
            string RequestId,
            ExceptionContext ExceptionContext,
            ControllerActionDescriptor ActionDescriptor,
            TypeInfo ControllerTypeInfo,
            IModelMetadataProvider ModelMetadataProvider,
            bool IsDevelopment)
        {
            this.IsWebApi = IsWebApi;
            this.RequestId = RequestId;
            this.ExceptionContext = ExceptionContext;
            this.ActionDescriptor = ActionDescriptor;
            this.ControllerTypeInfo = ControllerTypeInfo;
            this.ModelMetadataProvider = ModelMetadataProvider;
            this.IsDevelopment = IsDevelopment;
        }

        /* properties */
        /// <summary>
        /// True means the exception thrown in an action of a Web Api controller, else in an Mvc controller.
        /// </summary>
        public bool IsWebApi { get; }
        /// <summary>
        /// The exception context
        /// </summary>
        public ExceptionContext ExceptionContext { get; }
        /// <summary>
        /// The action descriptor
        /// </summary>
        public ControllerActionDescriptor ActionDescriptor { get; }
        /// <summary>
        /// The controller type info. <see cref="TypeInfo"/> is a descendant of the <see cref="Type"/> class.
        /// </summary>
        public TypeInfo ControllerTypeInfo { get; }
        /// <summary>
        /// The model metadata provider
        /// </summary>
        public IModelMetadataProvider ModelMetadataProvider { get; }
        /// <summary>
        /// The Id of the current http request
        /// </summary>
        public string RequestId { get; }
        /// <summary>
        /// True when in development environment
        /// </summary>
        public bool IsDevelopment { get; }
    }

}
