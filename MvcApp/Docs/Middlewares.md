# Middlewares and the Request pipeline

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).

A web application handles HTTP requests and returns HTTP responses. When a request arrives it is examined and handled by a number of software components, sequentially.

In Asp.Net Core these components are called `Middlewares` or `Request Delegates`.

A `Middleware`, in Asp.Net Core, is a term referring to a software component that examines and handles a request.

`Pipeline` is a term referring to the sequence of middlewares. 

A middleware may, or may not, pass the request down to the next middleware in the pipeline. It may also do some work with the request before passing it or after passing it to the next middleware.

A middleware may opt to not pass the request to the next middleware in the pipeline, thus terminating the handling or the request and sending a response to the client. That middleware is called `terminal middleware`.

The order a middleware is added to the pipeline is **very important**.

## Request Delegates

Request Delegate is another term for a middleware.

A request delegate can be either an anonymous method or a specialized middleware class. The pipeline is actually a sequence of request delegates.

Request delegates, be it an anonymous method or a middleware class, are added to the pipeline using the `Use`, `Map` and `Run` extension methods. 

## The `Run` request delegate

A `Run` request delegate is a `terminal middleware` and it terminates the pipeline returning a response.

```
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// other middlewares here
// ...

app.Run(async context =>
{
    // do some work here
    // and then terminate the pipeline
    await context.Response.WriteAsync("This is the response.");
});

app.Run();
```
 
## The `Use` request delegate

The `Use` request delegate is used in chaining multiple request delegates, i.e. middlewares.

```
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    // do some work before calling the next middleware
    // ...

    // call next middleware  
    await next.Invoke();

    // do some work after calling the next middleware
    // ...
});

app.Use(async (context, next) =>
{
    // this is the next middleware in the pipeline

    // call next middleware  
    await next.Invoke();
});

// other middlewares here
// ...

app.Run(async context =>
{
    await context.Response.WriteAsync("This is the response.");
});

app.Run();
```

The next parameter represents the next request delegate, i.e. middleware, in the pipeline. If `next` is not called, the pipeline terminates.

The `Run` extension method does not receive a `next` parameter as it is always a `terminal middleware`.

## The `Map` request delegate

The `app` variable is of type [WebApplication](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplication).

The `WebApplication` implements, among others, the following two interfaces.
- [IApplicationBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder)
- [IEndpointRouteBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.routing.iendpointroutebuilder)

There is number of `Map` extension methods on these two interfaces. Here are the links.

- [MapExtensions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.mapextensions)
- [EndpointRouteBuilderExtensions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.endpointroutebuilderextensions)

The `Map` request delegate is used in branching the pipeline based on the request path. If the request path starts with the path of the `Map` delegate, then that branch is executed.

```
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/get_data", (app) => {
    var Data = SomeService.GetData();
    return Results.Ok(Data);
});

app.Map("/get_data_list", (app) => {
    var DataList = SomeService.GetDataList();
    return Results.Ok(DataList);
});

// other middlewares here
// ...

app.Run(async context =>
{
    await context.Response.WriteAsync("This is the response.");
});

app.Run();
```

There are `MapGet()`, `MapPost()`, etc. variations of the `Map` method.

## The `UseWhen` and `MapWhen` request delegates
 