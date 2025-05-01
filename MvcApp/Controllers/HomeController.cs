namespace MvcApp.Controllers
{
    public class HomeController : ControllerMvc
    {
        public HomeController()
        {
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/login", Name = "Login"), AllowAnonymous]
        public IActionResult Login()
        {
            if (UserContext.IsAuthenticated)
                return RedirectToRoute("Home");

            CredentialsModel M = new CredentialsModel();

            return View("Login", M);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true), AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
