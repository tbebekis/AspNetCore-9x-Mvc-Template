using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata;

namespace MvcApp.Library
{
    public class AppDbContext : DbContext
    {
        public const string SMemoryDatabase = "MemoryDatabase";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(SMemoryDatabase);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Product> XXX = modelBuilder.Entity<Product>();
 
        }

        /// <summary>
        /// If the DbContext subtype is itself intended to be inherited from, then it should expose a protected constructor taking a non-generic DbContextOptions
        /// SEE: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#dbcontextoptions-versus-dbcontextoptionstcontext
        /// </summary>
        protected AppDbContext(DbContextOptions contextOptions)
        : base(contextOptions)
        {
        }

        public AppDbContext()
            : this(new DbContextOptions<AppDbContext>())
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        static public AppDbContext Create()
        {
            // SEE: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#basic-dbcontext-initialization-with-new
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                //.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test;ConnectRetryCount=0")
                                 .UseInMemoryDatabase(AppDbContext.SMemoryDatabase) 
                                 .Options;

            return new AppDbContext(contextOptions);
        }

        static public List<Product> CreateProductList()
        {
            Random R = new Random();

            decimal GetPrice()
            {
                decimal Result = Convert.ToDecimal(R.NextDouble());
                Result = Math.Round(Result, 2);
                Result += R.Next(2, 40);
                return Result;
            }
            List<Product> List = new List<Product>()
            {
                new Product("Absorbent cotton", GetPrice()),
                new Product("Alfalfa pellets", GetPrice()),
                new Product("Allspice", GetPrice()),
                new Product("Almonds", GetPrice()),
                new Product("Aniseed", GetPrice()),
                new Product("Apples", GetPrice()),
                new Product("Apples, dried", GetPrice()),
                new Product("Apricot kernels", GetPrice()),
                new Product("Apricots, dried", GetPrice()),
                new Product("Artichokes", GetPrice()),
                new Product("Asparagus", GetPrice()),
                new Product("Automobiles", GetPrice()),
                new Product("Avocados", GetPrice()),
                new Product("Bananas", GetPrice()),
                new Product("Barley", GetPrice()),
                new Product("Bay leaves", GetPrice()),
                new Product("Beans, dried", GetPrice()),
                new Product("Beer", GetPrice()),
                new Product("Blueberries", GetPrice()),
                new Product("Brazil nuts", GetPrice()),
                new Product("Brewer’s grain pellets", GetPrice()),
                new Product("Butter", GetPrice()),
                new Product("Camelina oil", GetPrice()),
                new Product("Candy sugar", GetPrice()),
                new Product("Capsicum", GetPrice()),
                new Product("Caraway", GetPrice()),
                new Product("Cardamom", GetPrice()),
                new Product("Cardboard", GetPrice()),
                new Product("Carpets", GetPrice()),
                new Product("Carrots", GetPrice()),
                new Product("Cashew nuts", GetPrice()),
                new Product("Castor oil", GetPrice()),
                new Product("Cellulose/chemical pulp", GetPrice()),
                new Product("Chair cane", GetPrice()),
                new Product("Cheese", GetPrice()),
                new Product("Cherries", GetPrice()),
                new Product("Chestnuts", GetPrice()),
                new Product("Chilies", GetPrice()),
                new Product("Chilled meat", GetPrice()),
                new Product("Chocolate, solid", GetPrice()),
                new Product("Cinnamon", GetPrice()),
                new Product("Citrus pellets", GetPrice()),
                new Product("Clementines", GetPrice()),
                new Product("Clothing/ready-made garmen", GetPrice()),
                new Product("Cloves", GetPrice()),
                new Product("Cocoa beans, raw cocoa", GetPrice()),
                new Product("Coconut fiber", GetPrice()),
                new Product("Coconut oil", GetPrice()),
                new Product("Coconuts", GetPrice()),
                new Product("Coffee, green coffee beans", GetPrice()),
                new Product("Combed top", GetPrice()),
                new Product("Copra", GetPrice()),
                new Product("Copra expeller", GetPrice()),
                new Product("Copra extraction meal", GetPrice()),
                new Product("Coriander", GetPrice()),
                new Product("Corn", GetPrice()),
                new Product("Corn gluten pellets", GetPrice()),
                new Product("Corn pellets", GetPrice()),
                new Product("Corrugated board", GetPrice()),
                new Product("Cotton", GetPrice()),
                new Product("Cottonseed", GetPrice()),
                new Product("Cottonseed expeller", GetPrice()),
                new Product("Cottonseed extraction meal", GetPrice()),
                new Product("Cottonseed oil", GetPrice()),
                new Product("Cottonseed pellets", GetPrice()),
                new Product("Cucumbers", GetPrice()),
                new Product("Currants", GetPrice()),
                new Product("Cut lumber", GetPrice()),
                new Product("Garlic", GetPrice()),
                new Product("Ginger, fresh", GetPrice()),
                new Product("Ginger, dried", GetPrice()),
                new Product("Grapefruit", GetPrice()),
                new Product("Grapes", GetPrice()),
                new Product("Lemons", GetPrice()),
                new Product("Lentils, dried", GetPrice()),
                new Product("Limes", GetPrice()),
                new Product("Linseed expeller", GetPrice()),
                new Product("Linseed/flaxseed", GetPrice()),
                new Product("Linseed oil", GetPrice()),
                new Product("Lump sugar", GetPrice()),
                new Product("Mace", GetPrice()),
                new Product("Machinery, machine parts", GetPrice()),
                new Product("Mandarins", GetPrice()),
                new Product("Mangos", GetPrice()),
                new Product("Manila hemp", GetPrice()),
                new Product("Millboard", GetPrice()),
                new Product("Mustard", GetPrice()),
                new Product("Mustard oil", GetPrice()),
                new Product("Natural rubber", GetPrice()),
                new Product("Newsprint", GetPrice()),
                new Product("Nutmegs", GetPrice()),
                new Product("Oats", GetPrice()),
                new Product("Olive oil", GetPrice()),
                new Product("Onions", GetPrice()),
                new Product("Oranges", GetPrice()),
                new Product("Packing paper", GetPrice()),
                new Product("Palm fiber", GetPrice()),
                new Product("Palm kernel oil", GetPrice()),
                new Product("Palm oil", GetPrice()),
                new Product("Paper bales", GetPrice()),
                new Product("Peaches/nectarines", GetPrice()),
                new Product("Peanut expeller", GetPrice()),
                new Product("Peanut extraction meal", GetPrice()),
                new Product("Peanut oil", GetPrice()),
                new Product("Peanut pellets", GetPrice()),
                new Product("Peanuts", GetPrice()),
                new Product("Pears", GetPrice()),
                new Product("Peas, dried", GetPrice()),
                new Product("Pepper", GetPrice()),
                new Product("Photographic paper", GetPrice()),
                new Product("Piassava", GetPrice()),
                new Product("Pineapple", GetPrice()),
                new Product("Pipes", GetPrice()),
                new Product("Pistachio nuts", GetPrice()),
                new Product("Poppy seed", GetPrice()),
                new Product("Potatoes", GetPrice()),
                new Product("Preserved foods, general", GetPrice()),
                new Product("Profiles", GetPrice()),
                new Product("Prunes", GetPrice()),
                new Product("Raffia", GetPrice()),
                new Product("Raisins", GetPrice()),
                new Product("Ramie", GetPrice()),
                new Product("Rapeseed oil", GetPrice()),
                new Product("Raw sugar", GetPrice()),
                new Product("Rice", GetPrice()),
                new Product("Rice bran pellets", GetPrice()),
                new Product("Roofing felt", GetPrice()),
                new Product("Roundwood", GetPrice()),
                new Product("Rum", GetPrice()),
                new Product("Rye", GetPrice()),
                new Product("Saffron", GetPrice()),
                new Product("Sage leaves", GetPrice()),
                new Product("Salt", GetPrice()),
                new Product("Seal oil", GetPrice()),
                new Product("Sesame oil", GetPrice()),
                new Product("Sesame seed expeller", GetPrice()),
                new Product("Silk", GetPrice()),
                new Product("Sisal hemp", GetPrice()),
                new Product("Soybean extraction meal", GetPrice()),
                new Product("Soybean meal pellets", GetPrice()),
                new Product("Soybeans", GetPrice()),
                new Product("Soy oil", GetPrice()),
                new Product("Star anise", GetPrice()),
                new Product("Steel sheet in coils", GetPrice()),
                new Product("Steel sheet in sheets", GetPrice()),
                new Product("Sultanas", GetPrice()),
                new Product("Sunflower expeller", GetPrice()),
                new Product("Sunflower oil", GetPrice()),
                new Product("Sunflower pellets", GetPrice()),
                new Product("Sunflower seeds", GetPrice()),
                new Product("Sweet peppers", GetPrice()),
                new Product("Switch cabinets/switchgear", GetPrice()),
                new Product("Synthetic rubber", GetPrice()),
                new Product("Tea", GetPrice()),
                new Product("Teaseed oil", GetPrice()),
                new Product("Tobacco, leaf tobacco", GetPrice()),
                new Product("Tomatoes", GetPrice()),
                new Product("Trucks", GetPrice()),
                new Product("Vanilla", GetPrice()),
                new Product("Veneer", GetPrice()),
                new Product("Walnuts", GetPrice()),
                new Product("Waste paper", GetPrice()),
                new Product("Waxed paper", GetPrice()),
                new Product("Wheat", GetPrice()),
                new Product("Wheat bran pellets", GetPrice()),
                new Product("Wheat mill run pellets", GetPrice()),
                new Product("White sugar", GetPrice()),
                new Product("Wire rod", GetPrice()),
                new Product("Wool", GetPrice())
            };

            

            return List;
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

            List<Product> ProductList = CreateProductList();


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

        // ● data handling
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


    // Principal (parent)
    public class Blog
    {
        public int Id { get; set; }
        public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
    }

    // Dependent (child)
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // Required foreign key property
        public Blog Blog { get; set; } = null!; // Required reference navigation to principal
    }
}
