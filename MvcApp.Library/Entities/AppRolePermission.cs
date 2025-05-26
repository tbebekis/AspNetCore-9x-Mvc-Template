namespace MvcApp.Library
{ 
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    public class AppRolePermission
    {
        public AppRolePermission() { }
        public AppRolePermission(string RoleId, string PermissionId) 
        {
            this.RoleId = RoleId;
            this.PermissionId = PermissionId;
        }

        public string RoleId { get; set; }
        public string PermissionId { get; set; }
    }
}
