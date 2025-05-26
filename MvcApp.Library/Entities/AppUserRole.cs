namespace MvcApp.Library
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class AppUserRole
    {
        public AppUserRole() { }
        public AppUserRole(string UserId, string RoleId) 
        { 
            this.UserId = UserId;
            this.RoleId = RoleId;
        }
 
        public string UserId { get; set; } 
        public string RoleId { get; set; }
    }
}
