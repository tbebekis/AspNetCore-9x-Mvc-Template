namespace MvcApp.Library
{
    /// <summary>
    /// Represents a user of this application
    /// </summary>
    public class AppUser: IRequestor
    {
        static AppUser fDefault;
 
        // ● properties
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Database Id or something similar.</para>
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// The level of a user, i.e. Guest, Admin, User, etc.
        /// </summary>
        public UserLevel Level { get; set; }
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para><c>Email</c> or <c>UserName</c>, when <see cref="Level"/> is <see cref="UserLevel.User"/>, <see cref="UserLevel.Admin"/> or <see cref="UserLevel.Guest"/>.</para>
        /// <para><c>ClientId</c> when <see cref="Level"/> is <see cref="UserLevel.ClientApp"/> or <see cref="UserLevel.Service"/>.</para>
        /// </summary> 
        public string AccountId { get; set; }
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para><c>Email</c> or <c>UserName</c>, when <see cref="Level"/> is <see cref="UserLevel.User"/>, <see cref="UserLevel.Admin"/> or <see cref="UserLevel.Guest"/>.</para>
        /// <para><c>ClientId</c> when <see cref="Level"/> is <see cref="UserLevel.ClientApp"/> or <see cref="UserLevel.Service"/>.</para>
        /// </summary> 
        public string UserId => AccountId;
        /// <summary>
        /// The user password encrypted
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The user password salt
        /// </summary>
        public string PasswordSalt { get; set; }
        /// <summary>
        /// Optional. The requestor name
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        /// Optional. The requestor email
        /// </summary> 
        public string Email { get; set; }
        /// <summary>
        /// True when requestor is blocked by admins
        /// </summary>
        public bool IsBlocked { get; set; }
        /// <summary>
        /// When true the user is activated
        /// </summary>
        public bool IsActivated { get; set; }

        /// <summary>
        /// Returns the default user. A fake user.
        /// </summary>
        static public AppUser Default
        {
            get
            {
                if (fDefault == null)
                {
                    fDefault = new AppUser();

                    fDefault.Id = DataStore.SDefaultId;
                    fDefault.Level = UserLevel.Guest;

                    fDefault.AccountId = "Default";
                    fDefault.Name = "Default";
                    fDefault.Email = "";
                    fDefault.IsBlocked = false;
                    fDefault.IsActivated = true;
                    fDefault.Password = "password";
                    fDefault.PasswordSalt = "salt";
                }
                    
                return fDefault;
            }             
        }
    }
}
