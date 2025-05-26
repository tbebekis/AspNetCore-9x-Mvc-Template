namespace MvcApp.Library
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    {
        public PermissionAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; set; }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }
    }
}
