namespace tp.Web
{
    /// <summary>
    /// Allows access for Ajax requests only
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeAjaxAttribute : Attribute, IAuthorizationFilter
    { 
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null && !WLib.IsAjaxRequest(context.HttpContext.Request))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}