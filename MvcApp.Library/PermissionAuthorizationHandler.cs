namespace MvcApp.Library
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAttribute>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAttribute requirement)
        {
            var User = context.User;
            bool IsAuthenticated = User != null && User.Identity != null ? User.Identity.IsAuthenticated : false;

            if (IsAuthenticated)
            {
                var IdClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Sid);
                if (IdClaim != null)
                {
                    string UserId = IdClaim.Value;
                    List<AppPermission> Permissions = DataStore.GetUserPermissions(UserId);

                    foreach (var Permission in Permissions)
                    {
                        if (string.Compare(Permission.Name, requirement.PermissionName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            context.Succeed(requirement);
                            break;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }

}
