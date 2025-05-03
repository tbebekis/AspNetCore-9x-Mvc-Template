namespace MvcApp.Controllers
{
    public class HomeController : ControllerMvc
    {
        public HomeController()
        {
        }
 
        [HttpGet("/", Name = "Home"), AllowAnonymous]
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
        [HttpPost("/login", Name = "Login"), AllowAnonymous]
        public async Task<IActionResult> Login(CredentialsModel M, string ReturnUrl = "")
        {
 
            if (UserContext.IsAuthenticated)
                return RedirectToRoute("Home");

            if (ValidateModel(M))
            {
                ItemResponse<IRequestor> Response = DataStore.ValidateRequestor(M.UserId, M.Password);
                IRequestor User = Response.Item;

                if (Response.Succeeded && User != null)
                {       
                    bool IsImpersonation = DataStore.GetIsImpersonation(M.Password);

                    await UserContext.SignInAsync(User, true, IsImpersonation);

                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        return HandleReturnUrl(ReturnUrl);

                    return RedirectToRoute("Home");
                }
                else if (!string.IsNullOrWhiteSpace(Response.Error))
                {
                    Session.AddToErrorList(L(Response.Error));
                }
                else
                {
                    Session.AddToErrorList(L("LoginFailed"));
                }

            }

            return View("Login", M); // something went wrong 
        }
        [Route("/logout", Name = "Logout"), AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            if (UserContext.IsAuthenticated)
                await UserContext.SignOutAsync();

            return RedirectToRoute("Home");
        }

        [Route("/set-language", Name = "SetLanguage"), AllowAnonymous]
        public IActionResult SetLanguage(string CultureCode, string ReturnUrl = "")
        {
 
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureCode)),
                new CookieOptions 
                {
                    Secure = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddYears(1) 
                }
            );

            if (!string.IsNullOrWhiteSpace(ReturnUrl))
                return HandleReturnUrl(ReturnUrl);

            return RedirectToRoute("Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true), AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
