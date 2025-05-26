namespace MvcApp.Library
{
    public class AppPermission
    {
        public override string ToString()
        {
            return Name;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
