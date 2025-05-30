namespace MvcApp.Library
{
    [Table(nameof(AppUserRole))]
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class AppUserRole
    {
        public AppUserRole() { }
        public AppUserRole(string UserId, string RoleId) 
        { 
            this.UserId = UserId;
            this.RoleId = RoleId;
        }

        [MaxLength(40)]
        public string UserId { get; set; }
        [MaxLength(40)]
        public string RoleId { get; set; }
    }
}
