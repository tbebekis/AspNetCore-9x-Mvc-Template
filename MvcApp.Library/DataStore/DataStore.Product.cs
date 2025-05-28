namespace MvcApp.Library
{
    static public partial class DataStore
    {
        static public ListDataResult<Product> GetAllProducts()
        {
            string CultureCode = DataStore.Culture.Name;
            string CacheKey = $"{nameof(GetAllProducts)}-{CultureCode}";

            // get products from cache
            // NOTE: in a real-world application most probably information such as products is NOT cached. 
            // This code is here just for demonstration purposes
            List<Product> ResultList = Cache.Get<List<Product>>(CacheKey, () => {

                List<Product> List;
                using (AppDbContext context = GetDbContext())
                {
                    List = context.Products.ToList();
                }

                int TimeoutMinutes = CacheTimeoutMinutes;
                return (TimeoutMinutes, List);
            });


            ListDataResult<Product> Result = new();
            Result.List = ResultList;
            Result.TotalItems = ResultList.Count;
 
            return Result;
        }
        static public ListDataResult<Product> GetAllProductsWithPaging(int PageIndex, int PageSize)
        {
            ListDataResult<Product> Result = new();
            List<Product> List;

            using (AppDbContext context = GetDbContext())
            {
                List = context.Products.ToList();
                Result.TotalItems = List.Count;
            } 

            List<Product[]> Chunks = List.Chunk(PageSize).ToList();
            Result.List = Chunks[PageIndex].ToList();

            // or
            Result.List = List.Skip(PageIndex).Take(PageSize).ToList();
 
            return Result;
        }
    }
}

 
