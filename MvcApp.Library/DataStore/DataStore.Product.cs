 

namespace MvcApp.Library
{
    static public partial class DataStore
    {
        static List<Product> GetPageChunk(List<Product> SourceList, int PageIndex, int PageSize)
        {
            if (SourceList.Count > 0)
            {
                List<Product> Result;

                List<Product[]> Chunks = SourceList.Chunk(PageSize).ToList();
                Result = Chunks[PageIndex].ToList();

                // or
                // Result = SourceList.Skip(PageIndex * PageSize).Take(PageSize).ToList();

                return Result;
            }

            return new List<Product>();
        }

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

            Result.List = GetPageChunk(List, PageIndex, PageSize);


            return Result;
        }
        static public ListDataResult<Product> GetProducts(ProductListFilter Filter)
        {
            ListDataResult<Product> Result = new();
            List<Product> List;

            using (AppDbContext context = GetDbContext())
            {
                List = context.Products.ToList();

                // filter
                List = List.Where(x => x.Name.Contains(Filter.Term, StringComparison.OrdinalIgnoreCase)).ToList();

                Result.TotalItems = List.Count;
            }            

            Result.List = GetPageChunk(List, Filter.PageIndex, Filter.PageSize);


            return Result;
        }
    }
}

 
