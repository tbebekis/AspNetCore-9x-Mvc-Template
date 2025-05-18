# Configuration

> This text is part of a group of texts describing an [Asp.Net Core MVC template project](ReadMe.md).

`Configuration` in Asp.Net Core is a term referring to a sub-system which provides settings, in the form of `Key-Value` pairs, required for application configuration.

`Configuration` in Asp.Net Core is a very complex issue.
 
## .Net and Asp.Net Core Configuration documentation

.Net and especially Asp.Net provide a whole universe of classes and interfaces that comprising its configuration system.

- [ConfigurationBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder)
- [IConfiguration](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration)
- [Configuration Providers](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration#configuration-providers)
- [ConfigurationBinder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder)
- [IOptions<TOptions>](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptions)
- [IOptionsSnapshot<TOptions>](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptionssnapshot-1)
- [IOptionsMonitor<TOptions>](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptionsmonitor-1)
- and more...

Documentation for the above system can be found at
- [Configuration in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration)

## Asp.Net Core Configuration providers

In Asp.Net Core a `Configuration Provider` is a software entity which reads information, in the form of `Key-Value` pairs, stored in a wide number of sources, such as disk files, environment variables, database tables, etc.

In Asp.Net Core there is number of [configuration providers](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#configuration-providers) available.

The most commonly used configuration provider in Asp.Net Core applications is the [JSON configuration provider](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#json-configuration-provider).

## The internals of Asp.Net Core configuration

Consider the following code of an Asp.Net Core startup.

```
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
 
    // ...

    app.Run();
}
```

The `builder` variable here is of type [WebApplicationBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplicationbuilder).

`WebApplicationBuilder` provides the `Configuration` property of type [ConfigurationManager](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationmanager).

`ConfigurationManager` implements an number of interfaces. One of them is the [IConfiguration](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) interface.

Also the `ConfigurationManager.GetSection(string Key)` method returns a [IConfigurationSection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationsection) interface which is a `IConfiguration` interface too.

The `IConfiguration` has a number of extension methods such as `Bind()`, `Get()` and `GetValue()`.

## Accessing Configuration values

Creating an Asp.Net Core application with Visual Studio or VSCode, creates an `appsettings.json` file too, with the following content.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Here is how to access configuration values from that file.

```
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
 
    // access a top level section
    IConfigurationSection LoggingSection = builder.Configuration.GetSection("Logging");

    // using a section variable to access a sub-section
    IConfigurationSection LoggingLevelSection = LoggingSection.GetSection("LogLevel");

    // using a section to read a value
    string DefaultLogLevel = LoggingLevelSection.GetValue<string>("Default");

    // another way in accessing sections is to use a path, i.e. Logging:LogLevel
    IConfigurationSection LoggingLevelSection2 = builder.Configuration.GetSection("Logging:LogLevel");

    // ...

    app.Run();
}
```

## Binding a user defined class to a Configuration Section

Let us say that the `appsettings.json` file has the following content.

```
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",

    "AppSettings": {
        "SessionTimeoutMinutes": 30,
        "SupportedCultures": [ "en-US", "el-GR" ]
    }
}
```

Accordingly the application uses a class like the following.

```
public class AppSettings
{ 
    public int SessionTimeoutMinutes { get; set; }
    public List<string> SupportedCultures { get; set; } = new List<string>();
}
```

Here is how to `bind` an instance of the `AppSettings` class to `appsettings.json` file using Asp.Net Core configuration.

```
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
 
    AppSettings Settings = new AppSettings();
    builder.Configuration.Bind(typeof(AppSettings).Name, Settings);

    // ...

    app.Run();
}
```



https://stackoverflow.com/questions/49454153/cannot-use-ioptionsmonitor-to-detect-changes-in-asp-net-core

https://dev.to/karenpayneoregon/asp-net-core-ioptionsmonitor-onchange-5906

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/change-tokens?view=aspnetcore-9.0#monitor-for-configuration-changes

https://www.endpointdev.com/blog/2021/09/monitoring-settings-changes-in-asp-net-core/