namespace MvcApp.Library
{
    [Table(nameof(Product))]
    public class Product
    {
        public Product(string Name, decimal Price)
        {
            this.Id = Sys.GenId();
            this.Name = Name;
            this.Price = Price;
        }
        public override string ToString()
        {
            return Name;
        }

        [Key, MaxLength(40)]
        public string Id { get; set; }
        [Required, MaxLength(256)]
        public string Name { get; set; }
        [Required, Precision(18, 4)]
        public decimal Price { get; set; }
    }

}
