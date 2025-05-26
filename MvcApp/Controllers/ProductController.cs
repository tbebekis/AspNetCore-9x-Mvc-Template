namespace MvcApp.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        // GET: ProductController
        [Permission("Product.View")]
        [HttpGet("list")]
        public ActionResult Index()
        {
            ListDataResult<Product> ListResult = DataStore.GetProducts();
            if (ListResult.Succeeded)
            {
                List<ProductModel> ModelList = WLib.ObjectMapper.Map<List<ProductModel>>(ListResult.List);
                return View(ModelList);
            }

            return RedirectToAction("Error", "Home", new { area = "" });
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [Permission("Product.View")]
        [HttpGet("view/{Id}")]
        public ActionResult Details(string Id)
        {
            return View();
        }



        // [HttpGet("/blog/update/{blogpostid}", Name = "UpdateBlogPost")]

        // GET: ProductController/Edit/5

        [Permission("Product.Edit")]
        [HttpGet("edit/{Id}")]
        public ActionResult Edit(string Id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Permission("Product.Delete")]
        [HttpGet("delete/{Id}")]
        public ActionResult Delete(string Id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string Id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
