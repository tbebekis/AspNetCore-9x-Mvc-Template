namespace MvcApp.Library
{
    public class AppDbContext : DbContext
    {
        public const string SMemoryDatabase = "MemoryDatabase";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(SMemoryDatabase);
        }



        public AppDbContext()
            : this(new DbContextOptions<AppDbContext>())
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        static public void AddDemoData()
        {
            AppPermission ProductCreate = new AppPermission() { Id = "433CAEBF-4F89-4392-BDC6-B4D3311F8E4D", Name = "Product.Create" };
            AppPermission ProductView = new AppPermission() { Id = "B8714AB8-ED20-4E1A-A251-5F0D6DE2429C", Name = "Product.View" };
            AppPermission ProductEdit = new AppPermission() { Id = "7F3289A3-F02B-4DF6-9949-9E86249841AF", Name = "Product.Edit" };
            AppPermission ProductDelete = new AppPermission() { Id = "EBB67955-C7C2-40D1-A7A6-BF0B2C5BBC2E", Name = "Product.Delete" };

            AppRole Admin = new AppRole("9522D4F4-1F88-4766-B15D-0CB714CD7C93", "Admin");
            AppRole Manager = new AppRole("B58E0CB0-E5A4-4641-B66A-E9792F0CC07F", "Manager");
            AppRole User = new AppRole("5CEF8CCE-A1B8-46F5-A471-3DB89EA376EA", "User");

            AppUser User1 = new AppUser("EFDFE472-FDA7-4B85-A31B-EDBF1E3A91CA", "user1", "john1.doe@email.com", "password");
            AppUser User2 = new AppUser("02C89661-B4D8-454D-ACBB-577471B0FB40", "user2", "john2.doe@email.com", "password");
            AppUser User3 = new AppUser("D108ABCC-E95E-492D-9F9C-AAFB876CEEB4", "user3", "john3.doe@email.com", "password");


            List<AppPermission> PermissionList = new List<AppPermission>() { ProductCreate, ProductView, ProductEdit, ProductDelete };
            List<AppRole> RoleList = new List<AppRole>() { Admin, Manager, User };
            List<AppUser> UserList = new List<AppUser>() { User1, User2, User3 };

            List<AppRolePermission> RolePermissions = new List<AppRolePermission>()
            {
                new AppRolePermission(Admin.Id, ProductCreate.Id),
                new AppRolePermission(Admin.Id, ProductView.Id),
                new AppRolePermission(Admin.Id, ProductEdit.Id),
                new AppRolePermission(Admin.Id, ProductDelete.Id),

                new AppRolePermission(Manager.Id, ProductView.Id),
                new AppRolePermission(Manager.Id, ProductEdit.Id),

                new AppRolePermission(User.Id, ProductView.Id),
            };

            List<AppUserRole> UserRoles = new List<AppUserRole>()
            {
                new AppUserRole(User1.Id, Admin.Id),
                new AppUserRole(User2.Id, Manager.Id),
                new AppUserRole(User3.Id, User.Id),
            };

            List<Product> ProductList = new List<Product>() {
                new Product("Potatoe", 1.5),
                new Product("Bean", 3.5),
                new Product("Onion", 0.5),
                new Product("Carrot", 1),
                new Product("Eggplant", 2.7),
            };


            using (var context = new AppDbContext())
            {
                context.Permissions.AddRange(PermissionList);
                context.Roles.AddRange(RoleList);
                context.Users.AddRange(UserList);

                context.RolePermissions.AddRange(RolePermissions);
                context.UserRoles.AddRange(UserRoles);

                context.Products.AddRange(ProductList);

                context.SaveChanges();
            }
        }

        public List<AppRole> GetUserRoles(string UserId)
        {
            List<AppRole> Result = new List<AppRole>();

            AppUser User = Users.FirstOrDefault(x => x.Id == UserId);
            if (User != null)
            {
                var RoleIdList = UserRoles
                                    .Where(r => r.UserId == UserId)
                                    .Select(x => x.RoleId)
                                    .ToList();

                Result = Roles.Where(r => RoleIdList.Contains(r.Id))
                                    .Select(r => r)
                                    .ToList();
            }

            return Result;
        }
        public List<AppPermission> GetUserPermissions(string UserId)
        {
            List<AppPermission> Result = new List<AppPermission>();
            List<AppRole> RoleList = GetUserRoles(UserId);

            foreach (var Role in RoleList)
            {
                var PermissionIdList = RolePermissions
                                    .Where(p => p.RoleId == Role.Id)
                                    .Select(x => x.PermissionId)
                                    .ToList();

                var List = Permissions.Where(p => PermissionIdList.Contains(p.Id))
                                    .Select(p => p)
                                    .ToList();

                foreach (var Permission in List)
                {
                    if (Result.FirstOrDefault(x => x.Id == Permission.Id) == null)
                        Result.Add(Permission);
                }
            }

            return Result;

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppPermission> Permissions { get; set; }

        public DbSet<AppRolePermission> RolePermissions { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
