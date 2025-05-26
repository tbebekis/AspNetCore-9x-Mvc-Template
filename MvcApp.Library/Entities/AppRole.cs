namespace MvcApp.Library
{
    public class AppRole
    {

        public AppRole()
        {
        }
        public AppRole(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public override string ToString()
        {
            return Name;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

    }
}
