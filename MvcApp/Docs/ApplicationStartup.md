# Application Startup

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).

The startup of an Asp.Net Core application, be it MVC or WebApi, is a complicated subject.

Everything starts in the `Program.cs` file.

In earlier Asp.Net Core versions the `Program.cs` file was something like the following

```
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```

The `Main()` creates a [`host`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/#host) which then is used in creating a `builder` which then is used in calling a `Startup` class.

Here is such a `Startup` class.

```
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // add services to the Dependency Injection container
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // add middlewares into the app pipeline
    }
}
```

In recent Asp.Net Core versions the `Program.cs` file is something like the following

```
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // add services to the Dependency Injection container
        // ...

        var app = builder.Build();

        // add middlewares into the app pipeline
        // ...

        // start the application
        app.Run();
    }
}
```

If the developer leaves un-checked the *Do not use top-level statements*, when creating the project in Visual Studio, then the `Program` class and the `Main()` method is omitted and the content of the `Program.cs` file is just the content of the above `Main()` method.

In any case here is what is going on

- there is a `host`, such as the `WebApplication` above
- the `host` is used in creating a [`builder`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplicationbuilder)
- the `builder` provides the `Services` property which is used in adding services to [Dependency Injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) container
- the `builder` builds the `app` which is a `WebApplication` instance (lol)
- the `app` is used in adding [Middlewares](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware) into the app pipeline to handle requests and responses.

> Nobody said being a developer is an easy or at least rational job and we all understand why.

## The two phases of Application Startup

The above is the history and the evolution.

The summary is that Application Startup has two phases

- add services to Dependency Injection container
- add middlewares into the app pipeline to handle requests and responses.

This template application has a `Program.cs` file with the following class.

```
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseDefaultServiceProvider(options => options.ValidateScopes = false);
        
        App.AddServices(builder);
        
        var app = builder.Build();
        App.AddMiddlewares(app);
        
        app.Run();
    }
}
```

The above code delegates the two phases of Application Startup to two methods of a static `App` class

- `App.AddServices()`, adds services to Dependency Injection container
- `App.AddMiddlewares()`, adds middlewares into the app pipeline to handle requests and responses.

Next we have to understand what Dependency Injection is.
