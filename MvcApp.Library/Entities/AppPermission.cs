namespace MvcApp.Entities
{

 
    [Table(nameof(AppPermission))]
    public class AppPermission
    {
        public override string ToString()
        {
            return Name;
        }

        [Key]
        public string Id { get; set; }
        [Required, MaxLength(96)]
        public string Name { get; set; }
    }
}
