# Claims and Identity

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).
 
[**Authentication**](https://en.wikipedia.org/wiki/Authentication) is a term denoting a process that verifies the identity of an application user.

[**Authorization**](https://en.wikipedia.org/wiki/Authorization) is a term denoting a process that decides if a user has the required permissions in order to access a certain resource provided by an application.

[**Principal**](https://en.wikipedia.org/wiki/Principal_(computer_security)) is a software entity that can be authenticated by an application, computer or network and it may represent a user, an application, a computer and the like.

[**Identity Provider**](https://en.wikipedia.org/wiki/Identity_provider) is a term denoting a software component that issues, maintains and manages identity information for `Principals`.

## .Net Core Identity

`Identity Information` is information, i.e. data, that identifies an entity. That entity could be a person or an application. `Authentication` is the verification of an `Identity`.

As `Identity Information`, the .Net Core and Asp.Net Core, uses [Claims-based Identity](https://en.wikipedia.org/wiki/Claims-based_identity). The relevant .Net Core classes are part of the [System.Security.Claims](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims) namespace.

- [ClaimsPrincipal](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimsprincipal) class. Implements the [IPrincipal](https://learn.microsoft.com/en-us/dotnet/api/system.security.principal.iprincipal) interface. 
- [ClaimsIdentity](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimsidentity) class. Implements the [IIdentity](https://learn.microsoft.com/en-us/dotnet/api/system.security.principal.iidentity) interface. 
- [Claim](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claim) class.

> In Asp.Net Core the [HttpContext.User](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext.user) property is a `ClaimsPrincipal`.

## Claim
The `Claim` class provides the `Type` and the `Value` properties both of the string type. 

The `Type` property contains the *name* of the claim, such as `Role`, where a possible `Value` could be `Administrator`.  

A `Claim` is actually a `Key-Value` pair. The `Claim.Type` property is the `Key` where the `Claim.Value` property is the `Value`.

A claim is a statement that an entity, such as a user or an application, makes about itself. Claims are used in both `Authentication` and `Authorization`.

## ClaimsIdentity

The `ClaimsIdentity` class is a **Claims-based Identity** and provides a collection of claims.

The `ClaimsIdentity.Claims` property is where these claims are kept.

One or more `Claim` instances may be assigned to a `ClaimsIdentity`. 

## ClaimsPrincipal

The `ClaimsPrincipal` class is an authentication and authorization entity.

The `ClaimsPrincipal` class is actually a collection of `ClaimsIdentity` identities. One or more `ClaimsIdentity` instances may be assigned to a `ClaimsPrincipal`. The `ClaimsPrincipal.Identities` property is where these identities are kept. 

One of these identities is considered the **primary Identity** and is accessible through the `ClaimsPrincipal.Identity` property.

The `ClaimsPrincipal` class also provides a collection of all the claims of all of its Identities. The `ClaimsPrincipal.Claims` property is a consolidation of all the claims of all the identities of a principal.

## A claims example


A `using System.Security.Claims;` directive is required when working with claims.

Consider the following `Main()` method of a console application.

```
internal class Program
{
    static void Main(string[] args)
    {
        // ● create a claims list
        List<Claim> ClaimList = new List<Claim>();

        ClaimList.Add(new Claim(ClaimTypes.PrimarySid, @"E7807E26-E5D2-4698-8E96-3F0C5B9BBBFE"));
        ClaimList.Add(new Claim(ClaimTypes.Name, "John Doe"));
        ClaimList.Add(new Claim(ClaimTypes.Email, "jdoe@company.com"));
        ClaimList.Add(new Claim(ClaimTypes.Role, "Admin"));

        // ● private claims
        ClaimList.Add(new Claim("FavoriteColor", "Blue"));

        // ● create the identity
        string AuthenticationType = "MyAuthType";
        ClaimsIdentity Identity = new ClaimsIdentity(ClaimList, AuthenticationType);   

        Console.WriteLine($"IsAuthenticated: {Identity.IsAuthenticated}");

        // ● create the principal
        ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);

        // ● make it the current principal in the current thread
        Thread.CurrentPrincipal = Principal;

        Console.WriteLine($"Thread.CurrentPrincipal.Identity.Name: {Thread.CurrentPrincipal.Identity.Name}");
        Console.ReadLine();
    }
}
```
The code first creates a claim list and adds claims to it.

The [ClaimTypes](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes) is a .Net built-in class which defines constants for the *well-known* claim `Types`. 

Besides these *well-known* claim types an application is free to use its own claim types, as is the `FavoriteColor` in the above example.

The `ClaimsIdentity` class provides a number of constructors. Some of these constructors accept a string parameter under the name `authenticationType`.

```
public ClaimsIdentity(IEnumerable<Claim>? claims, string? authenticationType)
```

If an `authenticationType` is passed when a `ClaimsIdentity` is constructed then the `ClaimsIdentity.IsAuthenticated` property returns `true`. 

> In Asp.Net Core passing an `authenticationType` is not enough for authentication. Also in Asp.Net Core an [Authentication Scheme](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/#authentication-scheme) is passed to these constructors where an `authenticationType` parameter is expected.

The list of claims is passed to the `ClaimsIdentity` constructor. Also there are constructors where the application may define what is the `Name` and the `Role` claims.

```
string AuthenticationType = "MyAuthType";
ClaimsIdentity Identity = new ClaimsIdentity(ClaimList, AuthenticationType, ClaimTypes.Email, ClaimTypes.Role);  
```

The `ClaimsPrincipal` class implements the `IPrincipal` interface. Because of that a `ClaimsPrincipal` instance may assigned to `Thread.CurrentPrincipal` property.

```
Thread.CurrentPrincipal = Principal;
```

After that the Principal is available on the current thread.

```
Console.WriteLine($"Thread.CurrentPrincipal.Identity.Name: {Thread.CurrentPrincipal.Identity.Name}");
```

The `Identity.Name` in the above, returns the value of the claim that is defined as the name claim.

## Useful method and properties

### ClaimsPrincipal
- `ClaimsPrincipal.Current`.
- `ClaimsPrincipal.AddIdentity()`.
- `ClaimsPrincipal.IsInRole()`. 
- `ClaimsPrincipal.HasClaim()`. 
- `ClaimsPrincipal.FindFirst()`. 
- `ClaimsPrincipal.FindAll()`. 

### ClaimsIdentity
- `ClaimsIdentity.IsAuthenticated`.
- `ClaimsIdentity.AuthenticationType`.
- `ClaimsIdentity.AddClaim()`.
- `ClaimsIdentity.AddClaims()`.
- `ClaimsIdentity.RemoveClaim()`.
- `ClaimsIdentity.HasClaim()`. 
- `ClaimsIdentity.FindFirst()`. 
- `ClaimsIdentity.FindAll()`. 

 


