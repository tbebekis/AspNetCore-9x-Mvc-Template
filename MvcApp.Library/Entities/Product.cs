namespace MvcApp.Library
{
    public class Product
    {
        public Product(string Name, double Price)
        {
            this.Id = Sys.GenId();
            this.Name = Name;
            this.Price = Price;
        }
        public override string ToString()
        {
            return Name;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
