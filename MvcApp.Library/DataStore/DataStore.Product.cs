namespace MvcApp.Library
{
    static public partial class DataStore
    {
        static public ListDataResult<Product> GetProducts()
        {
            ListDataResult<Product> Result = new ();

            using (AppDbContext context = GetDbContext())
            {
                Result.List = context.Products.ToList();
            }
                
            return Result;
        }
    }
}
