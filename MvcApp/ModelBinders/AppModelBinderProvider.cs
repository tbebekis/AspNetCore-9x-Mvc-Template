namespace MvcApp.ModelBinders
{
    /// <summary>
    /// SEE: https://docs.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding
    /// </summary>
    internal class AppModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(decimal))
                return new DecimalModelBinder();

            return null;
        }
    }
}
