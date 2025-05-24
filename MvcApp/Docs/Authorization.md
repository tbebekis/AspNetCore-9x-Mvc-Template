# Authorization

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).

[**Authorization**](https://en.wikipedia.org/wiki/Authorization) is a process that decides if a user has the required permissions in order to access a certain resource provided by an application. 

## Authorization types

Regarding authorization Asp.Net Core provides a number of options.

- Simple Authorization.
- Policy-based Authorization.
- Scheme-based Authorization.
- Claims-based Authorization.
- Role-based Authorization.
- Resource-based Authorization.
- Custom Authorization. https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iard

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

## Policy-based Authorization.

In Asp.Net Core all authorization methods end up to be implemented as policies.

A `Requirement` is a code entity which represents a condition that the current `Identity`, i.e. user, must meet, in order to access a protected resource.

Common requirements are
- the `Identity` contains a claim of a specified type
- the `Identity` contains a claim of a specified type having a specified value
- the `Identity` contains a role claim
- the `Identity` contains a role claim having as value a particular role

Besides the above nothing prohibits a requirement from specifying a condition that does not involve claims. That is a requirement may specify a condition that comes from any other source and not from a claim.

A `Policy` is a code entity which represents a collection of requirements. For a policy evaluation to succeed all of its requirements must meet.

#### Applying a Policy

Here is how to apply a policy.

```
[Authorize(Policy = "Policy1")]
public class MyController : Controller
{
    public ActionResult Action1() 
    {
    }

    [Authorize(Policy = "Policy2")]
    public ActionResult Action2() 
    {
    }

    [Authorize(Policy = "Policy3")]
    [Authorize(Policy = "Policy4")]
    public ActionResult Action3() 
    {
    }        
}
```
When multiple policies are applied to a controller or action, then all of them are required to be satisfied. Multiple policies work like boolean conditions with the and operator.

In the above code

- `Action1` requires `Policy1`
- `Action2` requires `Policy1` and `Policy2`
- `Action3` requires `Policy1`, `Policy2`, `Policy3` and `Policy4`


Here is how to apply a policy in [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview) end point.

```
app.MapGet("/get-item", () => { ...})
    .RequireAuthorization("Item.View");
```

#### Creating a Policy

Here is, a totally valid but not common way, of how to create a policy that requires users to be authenticated, using the [AuthorizationPolicy](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizationpolicy) class.

```
List<IAuthorizationRequirement> Requirements = new() { new DenyAnonymousAuthorizationRequirement() };
List<string> RequiredAuthenticationSchemes = new();

var MyPolicy = new AuthorizationPolicy(Requirements, RequiredAuthenticationSchemes);
```

In creating a policy a common way is to use the [AuthorizationPolicyBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizationpolicybuilder
) class.

The use of the `AuthorizationPolicyBuilder` class provides properties and methods thus allowing to easily add requirements regarding authentication schemes, claims, roles and even use assertion functions of the form `Func<AuthorizationHandlerContext, bool>`.

```
var policy = new AuthorizationPolicyBuilder()
  .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
  .RequireAuthenticatedUser()
  .RequireRole("Admininstrator")
  .RequireClaim("Department", "IT")  
  .RequireAssertion(ctx =>
  {
    return ctx.User.HasClaim("Specialty", "SystemEngineer");
  })
  .Build();
```

The `Build()` method returns an `AuthorizationPolicy` instance.

#### Registering a Policy

A policy must be registered with the authorization middleware under a unique name.

The `policy` parameter in the following example is an instance of the `AuthorizationPolicyBuilder` class.

```
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("POLIY_NAME", policy => { 
        // code here
    });
});
```

#### Checking a Policy using code

It is possible to check programmatically if a policy is met using the [IAuthorizationService.AuthorizeAsync(ClaimsPrincipal, Object, String)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.iauthorizationservice.authorizeasync) method.

That method gets the following parameters.

- ClaimsPrincipal user. The `Identity` to check.
- object? resource. Optional. A resouce that may required in policy evaluation.
- string policyName. The policy name.
 
Here is how to programmatically check a policy in a controller.

```
public class MyController : Controller
{
    IAuthorizationService AuthService;
    public MyController(IAuthorizationService authorizationService)
    {
        AuthService = authorizationService;
    }

    public async Task<IActionResult> MyAction1() 
    {
        AuthorizationResult AuthResult = await AuthService.AuthorizeAsync(User, null, "MyPolicy");
        if (!AuthResult.Succeeded)
            return new ForbiddenResult();

        // ...
    }  
}
```

Here is how to programmatically check a policy in a view.

```
@inject IAuthorizationService AuthService
@{
    AuthorizationResult AuthResult = await AuthService.AuthorizeAsync(User, null, "MyPolicy");
    if (!AuthResult.Succeeded)
    {
        //...
    }
}
<div>
   <!--  
   ...
   -->
</div>
```

The above example requires a using in the `_ViewImports.cshtml` as 

```
@using Microsoft.AspNetCore.Authorization
```

#### Custom Policy Requirements

A policy requirement is comprised of two components.

- a requirement class that is just data
- an authorization handler that evaluates that requirement data against the current `Identity` 

Here is a custom requirement class.


```
public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string Permission)
    {
        this.Permission = Permission;
    }

    public string Permission { get; }
}
```

And here is its associated `AuthorizationHandler`.

The passing in `context` is a [AuthorizationHandlerContext](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizationhandlercontext) instance.

The `DataStore.GetUserPermissions(Id)` returns a list of permissions, from a database table, associated to a `Identity`, i.e. user or application, `Id` 

```
public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var IdClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Sid);
        if (IdClaim is null)
        {
            return Task.CompletedTask;
        }

        string Id = IdClaim.Value;
        List<string> Permissions = DataStore.GetUserPermissions(Id);

        foreach (var Permission in Permissions)
        {
            if (string.Compare(Permission, requirement.Permission, StringComparison.OrdinalIgnoreCase) == 0)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        } 

        return Task.CompletedTask;
    }
}
```

The `AuthorizationHandler` must be registered with the Dependency Injection container.
 
```
services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
```

Here is the registration of a policy with the above requirement.

```
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Item.Create", policy => { policy.Requirements.Add(new PermissionRequirement("Item.Create")); });
});
```
#### Policy considerations


























 
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

There are two ways to enforce authorization using an `Authentication Scheme`.
- using the `AuthorizeAttribute`
- using a `Policy`

#### Enforce Authentication Scheme using the AuthorizeAttribute attribute

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
#### Enforce Authentication Scheme using Policies 

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

Although the above example uses the `ClaimTypes.Role` claim type, claims-based authorization works with any claim type, as long as a policy is registered expressing a requirement about a claim.

Here an overload of the `RequireClaim(string claimType, params string[] allowedValues)` method is used which accepts both the claim type and its possible values. In this overload both the claim type and its possible values is the requirement.

There is another overload as `RequireClaim(string claimType)` where only the existence of a specified claim type is the requirement. 

Here is how to authorize access based on claims enforced by policies.

When multiple policies are applied to a controller or action, then all of them are required to be satisfied. Multiple policies work like boolean conditions with the **and** operator.

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

Role-based Authorization in Asp.Net Core is very simple. It just requires the existence of a claim type in the authenticated request before access is granted.

 



[Role-based access control (RBAC)](https://en.wikipedia.org/wiki/Role-based_access_control) or `Role-based access security` is a widely used method in allowing access to protected resources to authorized requestors only, based on Roles.
