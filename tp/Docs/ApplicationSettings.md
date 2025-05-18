# Application Settings

Many applications need one or more configuration files. In the MS Windows era that configuration was done using [ini files](https://en.wikipedia.org/wiki/INI_file).

It was a simple format. Just named sections with Key/Value pair entries.

```
; comment 1
[section1]
Key1 = Value
Key2 = Value

[section2]
; comment 1
Key1 = Value     ; comment 3   
Key2 = Value
Key3 = Value
```

Nowadays applications use `xml` and `json` formats with their configuration files.

## .Net Configuration

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

## The AppSettingsBase class

This library provides the `AppSettingsBase` class for application configuration.

```
public class AppSettingsBase
{
    const string SFileName = "ApplicationSettings.json";

    protected string Folder { get; private set; }
    protected string FileName { get; private set; }
    protected string FilePath { get; private set; }
    protected FileSystemWatcher Watcher { get; private set; }
    protected bool IsSaving { get; private set; }
    protected bool IsReloadable { get; private set; }

    void CreateWatcher();
    void DisposeWatcher();

    protected void Watcher_Changed(object sender, FileSystemEventArgs e);

    protected virtual void BeforeLoad();
    protected virtual void AfterLoad();
    protected virtual void BeforeSave();
    protected virtual void AfterSave();

    private AppSettingsBase();
    protected AppSettingsBase(string Folder = "", string FileName = "", bool IsReloadable = true);

    public virtual void Load();
    public virtual void Save();
}
```

`AppSettingsBase` serves as a base class. The developer derives a class from `AppSettingsBase` and adds whatever needed to be added. Here is an example.

```
public class DatabaseSettings
{
   public string ConnectionString { get; set; } = "...";
    ...
}

public class HttpSettings
{
    public string StaticFilesCacheControl { get; set; } = "no-store,no-cache";
    public List<string> BlackList { get; set; } = new List<string>();
    ...
}

public class ApplicationSettings: ApplicationSettingsBase
{
    public ApplicationSettings();

    public string DefaultCultureCode { get; set; } = "en-US";
    public string DefaultCurrencySymbol { get; set; } = "€";

    public DatabaseSettings Database { get; set; } = new DatabaseSettings();
    public HttpSettings Http { get; set; } = new HttpSettings(); 
}
```

`AppSettingsBase` saves configuration to a `json` file. It watches that `json` file for changes that may happen during application execution, using the [FileSystemWatcher](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher) class. If a change happens the `AppSettingsBase` reloads the file into its properties.

> **NOTE**: If there are list properties in the `AppSettingsBase`, such is the `BlackList` in the above example, the developer has to override the `BeforeLoad()` method and clear those lists.
 
## How to use it

Most of the time application configuration information should be available to any client code.

All that is needed is a `public static class` available to any client code with a relevant property.

```
static public class MyLib
{
    ...

    static public ApplicationSettings Settings { get; } = new ApplicationSettings();
}
```

That's all. And it's easy.