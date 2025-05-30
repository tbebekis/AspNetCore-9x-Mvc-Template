namespace MvcApp.Library
{
    [Table(nameof(AppRolePermission))]
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    public class AppRolePermission
    {
        public AppRolePermission() { }
        public AppRolePermission(string RoleId, string PermissionId) 
        {
            this.RoleId = RoleId;
            this.PermissionId = PermissionId;
        }

 
        [MaxLength(40)]
        public string RoleId { get; set; }
        [MaxLength(40)]
        public string PermissionId { get; set; }
    }
}
