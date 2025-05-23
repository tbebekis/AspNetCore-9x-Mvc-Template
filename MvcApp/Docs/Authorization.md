# Authorization

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).

[**Authorization**](https://en.wikipedia.org/wiki/Authorization) is a process that decides if a user has the required permissions in order to access a certain resource provided by an application. 

## Authorization types

Regarding authorization Asp.Net Core provides a number of options.

- Simple Authorization.
- Scheme-based Authorization.
- Claims-based Authorization.
- Role-based Authorization.
- Policy-base Authorization.
- Resource-based Authorization.
- Custom Authorization.

## Simple Authorization

The [AuthorizeAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizeattribute) attribute controlls authorization.

When placed on a controller then all controller actions required an authenticated request.

```
[Authorize]
public class MyController : Controller
{
    public ActionResult Action1() 
    {
    }

    public ActionResult Action2()
    {
    }
}
```

All the actions of the above controller, and its descendants, require authenticated requests.

The `AuthorizeAttribute` can be placed on an `Action` too.

```
public class MyController : Controller
{
    public ActionResult Action1() 
    {
    }

    [Authorize]
    public ActionResult Action2()
    {
    }
}
```

Only the `Action2` of the above controller require authenticated requests.

The [AllowAnonymousAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.allowanonymousattribute) attribute allows access to an `Action` of a protected controller to a non-authenticated request.

```
[Authorize]
public class MyController : Controller
{
    public ActionResult Action1() 
    {
    }

    [AllowAnonymous]
    public ActionResult Action2()
    {
    }
}
```

All the actions of the above controller, and its descendants, require authenticated requests, except of the `Action2`.

The `AuthorizeAttribute` provides the following properties.

- `AuthenticationSchemes`. A comma-delimited list of strings of the authentication schemes that required in order to gain access.
- `Policy`. A string with the name of the policy that required in order to gain access.
- `Roles`. A comma-delimited list of strings of the roles that required in order to gain access.

The `Simple Authentication` does not uses these properties.
 
## Scheme-based Authorization

There are cases where an application has to use multiple authentication methods, i.e. [Authentication Schemes](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/#authentication-scheme).

An application may need
- a cookie-base authentication with a basic identity
- a cookie-base authentication with [Multi-Factor Authentication](https://en.wikipedia.org/wiki/Multi-factor_authentication)
- a [JWT bearer authentication](https://en.wikipedia.org/wiki/JSON_Web_Token).

Consider the following.

```
var builder = WebApplication.CreateBuilder(args);

// some services here
// ...

// ● Authentication
AuthenticationBuilder AuthBuilder = services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = options.DefaultScheme;
    options.DefaultChallengeScheme = options.DefaultScheme;
});

// ● Cookie authentication
AuthBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
    // ...
});

// ● JWT authentication
AuthBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
    // ...
});

// ...

var app = builder.Build();

// some middlewares here
// ...

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(...);

app.Run();
```

Every authentication scheme added to configuration service has its own discrete name.
 
The above code adds two authentication schemes under specific names.

- [CookieAuthenticationDefaults.AuthenticationScheme](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationdefaults)
- [JwtBearerDefaults.AuthenticationScheme](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbearerdefaults)

### Enforce Authentication Scheme using the AuthorizeAttribute attribute

Here is how to authorize access based on an authentication scheme.

```
[Authorize(AuthenticationSchemes = $"{CookieAuthenticationDefaults.AuthenticationScheme}, {JwtBearerDefaults.AuthenticationScheme}")]
public class AllSchemesController : Controller
{
    //...
}

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class CookieOnlyController : Controller
{
    //...
}

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class JwtOnlyController : Controller
{
    //...
}
```
### Enforce Authentication Scheme using Policies 

Policies may added using the `AddAuthorization()`.

```
var builder = WebApplication.CreateBuilder(args);

// some services here
// ...

// ● Authentication
AuthenticationBuilder AuthBuilder = services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = options.DefaultScheme;
    options.DefaultChallengeScheme = options.DefaultScheme;
});

// ● Cookie authentication
AuthBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
    // ...
});

// ● JWT authentication
AuthBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
    // ...
});

// ● Authorization
builder.Services.AddAuthorization(options =>
{
    var DefaultAuthPolicyBuilder = new AuthorizationPolicyBuilder(
        CookieAuthenticationDefaults.AuthenticationScheme,
        JwtBearerDefaults.AuthenticationScheme);

    DefaultAuthPolicyBuilder = DefaultAuthPolicyBuilder.RequireAuthenticatedUser();

    options.DefaultPolicy = DefaultAuthPolicyBuilder.Build();

    var CookieOnlyPolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
    options.AddPolicy("CookieOnlyPolicy", CookieOnlyPolicyBuilder
        .RequireAuthenticatedUser()
        .Build());    

    var JwtOnlyPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
    options.AddPolicy("JwtOnlyPolicy", JwtOnlyPolicyBuilder
        .RequireAuthenticatedUser()
        .Build());
});

//...

var app = builder.Build();

// some middlewares here
// ...

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(...);

app.Run();
```

Here is how to authorize access based on authentication schemes enforced by policies.

```
[Authorize(Policy = "CookieOnlyPolicy")]
public class CookieOnlyController : Controller
{
    //...
}

[Authorize(Policy = "JwtOnlyPolicy")]
public class JwtOnlyController : Controller
{
    //...
}
```

## Claims-based Authorization

Claims authorization is policy based. The application has to register a policy expressing the claims requirements.

```
var builder = WebApplication.CreateBuilder(args);

// some services here
// ... 

// ● Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));

    options.AddPolicy("Manager", policy => policy.RequireClaim(ClaimTypes.Role, "Manager"));
});

//...

var app = builder.Build();

// some middlewares here
// ...

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(...);

app.Run();
```

Here an overload of the `policy.RequireClaim()` method is used which accepts both the cookie type and its possible values.

Here is how to authorize access based on claims enforced by policies.

```
[Authorize(Policy = "Administrator")]
[Authorize(Policy = "Manager")]
public class AllController : Controller
{
    //...
}

[Authorize(Policy = "Administrator")]
public class AdminOnlyController : Controller
{
    //...
} 
```

## Role-based Authorization.

[Role-based access control (RBAC)](https://en.wikipedia.org/wiki/Role-based_access_control) or `Role-based access security` is a widely used method in allowing access to protected resources to authorized requestors only, based on Roles.
