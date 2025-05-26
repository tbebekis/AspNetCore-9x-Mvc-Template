using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApp.Library
{
    static public partial class DataStore
    {
        static public AppUser ValidateCredentials(string UserId, string Password)
        {
            AppUser Result = null;
            using (AppDbContext context = GetDbContext())
            {
                AppUser User = context.Users.FirstOrDefault(x => x.UserName == UserId);
                if (User != null)
                {
                    if (Hasher.Validate(Password, User.Password, User.PasswordSalt))
                        Result = User;
                }
            }

            return Result;
        }

        static public List<AppPermission> GetUserPermissions(string Id)
        {
            List<AppPermission> Result = new List<AppPermission>();

            using (AppDbContext context = GetDbContext())
            {
                Result = context.GetUserPermissions(Id);
            }

            return Result;
        }
    }
}
