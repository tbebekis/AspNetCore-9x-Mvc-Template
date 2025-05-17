# Cookie Authentication

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).

[**Authentication**](https://en.wikipedia.org/wiki/Authentication) is a term denoting a process that verifies the identity of an application user.

[**Authorization**](https://en.wikipedia.org/wiki/Authorization) is a term denoting a process that decides if a user has the required permissions in order to access a certain resource provided by an application.

[**Principal**](https://en.wikipedia.org/wiki/Principal_(computer_security)) is a software entity that can be authenticated by an application, computer or network and it may represent a person, an application, a computer and the like.

[**Identity Provider**](https://en.wikipedia.org/wiki/Identity_provider) is a term denoting a software component that issues, maintains and manages identity information for `Principals`.

Asp.Net Core provides a number of ways to deal with [Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication) and a list of [Identity solutions](https://learn.microsoft.com/en-us/aspnet/core/security/how-to-choose-identity-solution) to choose from.

This text describes a solution using [Cookie Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie)

## Configuring Gookie Authentication

```
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// some services here
// ...

AuthenticationBuilder AuthBuilder = builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

AuthBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.ReturnUrlParameter = "ReturnUrl";
    options.EventsType = typeof(UserCookieAuthEvents);
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    //options.SlidingExpiration = true;

    options.Cookie.Name = "MyAuthCookie";       
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()? CookieSecurePolicy.None : CookieSecurePolicy.Always;
});

builder.Services.AddScoped<UserCookieAuthEvents>();

// ● authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(App.PolicyAuthenticated, policy => { policy.RequireAuthenticatedUser(); });
});

var app = builder.Build();

// some middlewares here
// ...

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(...);

app.Run();
```

The order of authentication and authorization middlewares is important. Is should be before `MapControllerRoute()` or `MapRazorPages()`.

The `options` parameter in `AddAuthentication()` is of type [AuthenticationOptions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationoptions).
 
The `options` parameter in `AddCookie()` is of type [CookieAuthenticationOptions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions).

The developer has to carefully review the properties of these `options` objects and decide what is the best configuration for an application.

## Authentication Schemes

An `Authentication Scheme` is a specific way of authenticating users and client applications. 

An `Authentication Scheme` consists of the following:

- an authenction scheme name, which is just a string
- an authentication handler, which should inherit from [AuthenticationHandler](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1) class or implement the [IAuthenticationHandler](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.iauthenticationhandler) interface
- an authentication options class, which should inherit from [AuthenticationSchemeOptions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationschemeoptions) class.

An Asp.Net Core application should have set the application's **default** authentication scheme. All the `AddAuthentication()` does is just this, it adds a scheme name as the application's **default** authentication scheme.

> **NOTE**: always set a default authentication scheme, especially if working with Asp.Net Core 6 and earlier.

An application may implement **more than one** authentication types, each one with its own authentication scheme.

```
AuthenticationBuilder AuthBuilder = builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

AuthBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
    // ...
});

AuthBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
    // ...
});
```

### Custom Authentication Scheme

A Custom Authentication Scheme has to fullfil the three requirements of an authentication scheme as described above:

- an authentication scheme name
- an authentication handler class
- an authentication options class.

## Login

A model such as the following is needed.

```
public class CredentialsModel
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
```

Then a `Login` action in a controller. Something like the following.

```
[HttpPost("/login", Name = "Login")]
public async Task<IActionResult> Login(CredentialsModel Model, string ReturnUrl = "")
{ 
    if (HttpContext.User.Identity.IsAuthenticated)
        return RedirectToRoute("Home");
 
    // ValidateCredentials() is a fictional method that goes to an Identity store, 
    // validates credentials and returns a fictional Account class instance on success
    Account account = ValidateCredentials(Model);

    if (account != null)
    {
        List<Claim> ClaimList = new List<Claim>();

        ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, account.Id));
        ClaimList.Add(new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(account.Name) ? account.Name : "no name"));
        ClaimList.Add(new Claim(ClaimTypes.Email, !string.IsNullOrWhiteSpace(account.Email) ? account.Email : "no email"));
 
        AuthenticationProperties AuthProperties = new AuthenticationProperties();
        AuthProperties.AllowRefresh = true;
        AuthProperties.IssuedUtc = DateTime.UtcNow;
        //AuthProperties.ExpiresUtc = DateTime.UtcNow.AddDays(30); // overrides the ExpireTimeSpan option of CookieAuthenticationOptions set with AddCookie
        AuthProperties.IsPersistent = Model.RememberMe;

        // authenticate the principal under the scheme
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal, AuthProperties);

        if (!string.IsNullOrWhiteSpace(ReturnUrl))
            return Redirect(ReturnUrl);

        return RedirectToRoute("Home");
    } 

    return View("Login", Model); // something went wrong 
}
```

The `RememberMe` boolean property of the `CredentialsModel` is used in making the cookie **persistent**, which means that the cookie [remains alive](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie#persistent-cookies) across browser sessions. This persistency is not for the application to decide. User consent is required for that. The user has to check the corresponding check-box in the login screen.

You may want the cookie to persist across browser sessions. This persistence should only be enabled with explicit user consent with a "Remember Me" checkbox on sign in or a similar mechanism.

Claims are stored in the authentication cookie and cookies have a strict size limit. Ideally only the `Account.Id` claim is needed. All other user information may come from the Identity store.

## Logout

The `Logout` action is very simple.

```
[Route("/logout", Name = "Logout")]
public async Task<IActionResult> Logout()
{
    if (HttpContext.User.Identity.IsAuthenticated)
       await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return RedirectToRoute("Home");
}
```

## Cookie is valid, but the user is not

The user is considered valid as long as the authentication cookie is valid. And the application will continue to accept and process requests based on that valid cookie.

But although a request comes with a valid cookie, there are situations where in the back-end system an administrator may have set the user account invalid or blocked. 

To deal with [situations like the above](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-9.0#react-to-back-end-changes) an application needs to check the validity of a cookie on every request. This is done by using a class inheriting from [CookieAuthenticationEvents](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationevents) in the `ValidatePrincipal()` method.

That event class must be set in the cookie options as is done in a previous example.

```
options.EventsType = typeof(UserCookieAuthEvents);
```

Here is that custom `UserCookieAuthEvents` class.

```
    internal class UserCookieAuthEvents : CookieAuthenticationEvents
    {
        public UserCookieAuthEvents()
        {           
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            try
            {
                if (context.Principal.Identity.IsAuthenticated)
                {
                    // we have Account.Id stored in ClaimTypes.NameIdentifier claim
                    Claim Claim = context.Principal.FindFirst(ClaimTypes.NameIdentifier); 

                    string AccountId = Convert.ChangeType(Claim.Value, typeof(string));

                    // the fictional DataStore.GetAccountById()
                    // returns an Account instance or null
                    Account account = DataStore.GetAccountById(AccountId); 

                    if (account != null)
                    {
                        // Account is blocked  but we still have a logged-in user
                        // so we must sign-out and return
                        if (account.IsBlocked)
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return;
                        }

                    }
                }
            }
            catch
            {
                // do nothing
            }
        }

    }
```

## Cookie Policy

[Cookie Policy](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie#cookie-policy-middleware) middleware is used in configuring global characteristics of cookies and even to define event handlers to take actions when cookies are appended or deleted. 

This is how the template application uses the cookie policy middleware.

```
app.UseCookiePolicy(new CookiePolicyOptions {
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    // If CheckConsentNeeded is set to true, then the IsEssential should be also set to true, for any Cookie's CookieOptions setting.
    // SEE: https://stackoverflow.com/questions/52456388/net-core-cookie-will-not-be-set
    CheckConsentNeeded = context => true,

    // Set the secure flag, which Chrome's changes will require for SameSite none.
    // Note this will also require you to be running on HTTPS.
    MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None,

    // Set the cookie to HTTP only which is good practice unless you really do need
    // to access it client side in scripts.
    HttpOnly = HttpOnlyPolicy.Always,

    // Add the SameSite attribute, this will emit the attribute with a value of none.
    Secure = app.Environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always
});
```


 