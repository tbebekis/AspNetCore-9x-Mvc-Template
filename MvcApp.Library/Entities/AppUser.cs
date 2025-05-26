namespace MvcApp.Library
{
    public class AppUser: IRequestor
    {
        static AppUser fDefault;

        public AppUser()
        {
        }
        public AppUser(string Id, string UserName, string Email, string PlainTextPassword)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.Name = Name;
            this.Email = Email;
            this.PasswordSalt = Hasher.GenerateSalt();
            this.Password = Hasher.Hash(PlainTextPassword, this.PasswordSalt);
        }
 
        public override string ToString()
        {
            return Name;
        }

        // ● properties
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Database Id or something similar.</para>
        /// </summary>
        [Key]
        public string Id { get; set; } = "";
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Required. 
        /// <para><strong>Unique.</strong></para>
        /// <para>Email or UserName when this is a person.</para>
        /// <para>ClientId when this is a client application or service.</para>
        /// </summary> 
        public string AccountId { get { return UserName; } set { } }
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
        /// Returns the default user. A fake user.
        /// </summary>
        static public AppUser Default
        {
            get
            {
                if (fDefault == null)
                {
                    fDefault = new AppUser();

                    fDefault.Id = Sys.SDefaultId;

                    fDefault.Name = "Default";
                    fDefault.Email = "";
                    fDefault.IsBlocked = false;
                    fDefault.Password = "password";
                    fDefault.PasswordSalt = "salt";
                }

                return fDefault;
            }
        }
    }
}
