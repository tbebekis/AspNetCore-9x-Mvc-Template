# A Logger for Asp.Net and Windows.Forms

This text describes a .Net [Logging](https://en.wikipedia.org/wiki/Logging_(computing)) library that can be used with [Asp.Net Core](https://github.com/dotnet/aspnetcore) or [Windows.Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms). 

Apart from [NewtonsoftJson](https://www.newtonsoft.com/json) this library has no other dependencies.

The idea that the use of [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection) should be universal and everything must be converted into service instances that are injected into a constructor and that the use of static classes must completely disappear, it resembles a cult and it is responsible for added complexity.

And if the above is true then we are facing an ideological issue. Not a technical one. I hope that’s not true.
 
## Logger

A logging framework can be implemented around a static `Logger` class.

```
static public class Logger
{
    static public void Log(LogEntry Info);
    static public void Log(string Source, string ScopeId, string EventId, LogLevel Level, Exception Exception, string Text, Dictionary<string, object> Params);
 
    static public void Info(string Text, ...);
    static public void Warn(string Text, ...);
    static public void Error(Exception Ex, ...);
 
    static public void Add(LogListener Listener);
    static public void Remove(LogListener Listener);

    static public bool Active { get; set; }
    static public LogLevel MinLevel { get; set; }
}
```

When the `Active` is false no logs are recorded. Defaults to true.

The `MinLevel` determines the level of the accepted log. For a `LogEntry` log information to be recorded its log level must be greater or equal to this `MinLevel` level. Defaults to `Info`.

```
    public enum LogLevel
    {
        None = 0,
        Trace = 1,
        Debug = 2,
        Info = 4,
        Warning = 8,
        Error = 0x10,
        Fatal = 0x20,
    }
```

That static `Logger` class provides a set of static log methods, such as `Log()`, `Warn()`, `Error()`, etc.

A log method examines the passed parameters and generates a unit of log information, an instance of the `LogEntry` class.

That `LogEntry` instance is then passed, asynchronously, meaning using a thread, to any registered log listener for further processing. One listener may display the log information to a console while another listener may save that information to a text file or a database table.

That same static `Logger` class provides methods such as `Add(LogListner Listener)` and `Remove(LogListner Listener)`, that is it is a log listeners registry.

## LogEntry

The `LogEntry` represents a log message, a unit of log information.

```
public class LogEntry
{
    public LogEntry(string Source, string ScopeId, string EventId, LogLevel Level, Exception Exception, string Text, Dictionary<string, object> Params = null);

    public string AsLine();
    public string AsList();    
    public string AsJson();

    public string Id { get; }
    public DateTime TimeStamp { get; }
    public string Date { get; }
    public string Time { get; }
    public string User { get; set; }
    public string Host { get; }
    public LogLevel Level { get; }
    public string EventId { get; }
    public string Text { get; }
    public string ScopeId { get; }
    public string Source { get; }
    public Dictionary<string, object> Properties { get; }
    public Exception Exception { get; }
}
```

A `LogEntry` is created using its constructor. 

The `AsLine()`, `AsList()` and `AsJson()` methods return the log information as a single text line, as a string list or as `json` text, respectively.

A `LogEntry` is also created by the 
- `Logger.Log(...)`, 
- `Logger.Info()`, 
- `Logger.Warn()`, 
- `Logger.Error()`, 
- etc. 
methods and then it is passed automatically to the `Logger.Log(LogEntry Info)` which then calls all registered `LogListener` objects passing that `LogEntry`.
 

## LogListener

The `Logger` has no idea of what to do with a `LogEntry`. It has no means to display the information to the user or to store it to a file or database table. This is the job of a `LogListener`.

A `LogListener` registers itself automatically with `Logger`, upon construction.

Whenever a `LogEntry` is passed to `Logger.Log(LogEntry Info)` the `Logger` calls the `ProcessLog(LogEntry Entry)` of all registered listeners, from inside a secondary thread, for further processing. 

```
public abstract class LogListener
{
    public LogListener();

    public abstract void ProcessLog(LogEntry Entry);

    public void Register();
    public void Unregister();

    public int RetainPolicyCounter { get; set; }
    public int RetainDays { get; set; }
    public int MaxSizeKiloBytes { get; set; }
}
```

The `LogListener` serves as the base class for all log listener classes. It also provides retain policy related properties for use by inheritors.

This library provides the following `LogListener` derived classes

- `LogListenerFile`. Writes log information to text files. Its `MaxSizeKiloBytes` constructor parameter defines the max size of a log file in KB. When a log file reaches that size, a new one is created. Applies retain policy. By default keeps log files 7 days old. 
- `LogListenerSynced`. Synchronizes its `ProcessLog(LogEntry Entry)` call to any context. It provides an `EntryEvent` which is safe to attach to any GUI element such as a Form or a TextBox. Client code has just to link to that `EntryEvent`  and then process the passed `LogEntry` safely.
- `LogListenerSqlSynced`. It is also a synchronized log listener. Writes log info to a database table using events. Client code links to `InsertEntryEvent` in order to insert log information to a database table. There is also a `ApplyRetainPolicyEvent` for executing an Sql statement that applies the retain policy. By default keeps entries 7 days old. 

## LogSource

```
public class LogSource
{
    class LogScope
    {
        public LogScope(LogSource LogSource, string Id = "", Dictionary<string, object> ScopeParams = null);
        public string Id { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    public LogSource(string Name);
    
    static public LogSource Create(string Name);

    public void EnterScope(string ScopeName, Dictionary<string, object> ScopeParams = null);
    public void ExitScope();

    public void Log(string EventId, LogLevel Level, Exception Ex, string Text, Dictionary<string, object> Params);
    public void Info(string Text);
    public void Warn(string Text);
    public void Error(Exception Ex);

    public string Name { get; set; }
    public bool Active { get; set; }
}
```
The `Logger` class also provides the `LogSource CreateSource(string Name)` method to create a Source.

The `LogSource` represents a named source in the log system, a source that produces log information.

The `LogEntry.Source` and the `LogEntry.ScopeId` are just names devised by the developer.

A `Source` could be the full name of a class, such as a MainForm, an action url or any other suitable string and it marks all the log messages produced by this source.

A `ScopeId` could be the name of a method or the name of a group of methods, or whatever.

The `LogSource` creates a Default `Scope` when it is created. The developer never gets a reference to a `Scope`. The `LogScope` class is a private type.

The developer may call `EnterScope(...)` passing it a Scope name.

The `ExitScope()` exits the last entered Scope. The Default Scope cannot exited. It is even safe to call `ExitScope()` more times than the existing Scopes.
 
Any call to `LogSource`  log methods, such as  `Log()`,  `Debug()`,  `Error()`, etc.  produces a `LogEntry` marked with its `Source` name and the name of the latest `Scope`.
 
 