namespace tp.Logging
{
    /// <summary>
    /// A log listener that writes log info to a database table using events.
    /// <para><strong>NOTE: </strong> Applies retain policy. By default keeps entries 7 days old.</para>
    /// <para></para>
    /// <para>This log listener synchronizes the <see cref="ProcessLog(LogEntry)"/> call to any context.</para>
    /// <para>That is, it is safe to attach to the <see cref="InsertEntryEvent"/> and the  <see cref="ApplyRetainPolicyEvent"/> even to a GUI element such as a Form or a TextBox.</para>
    /// <para>SEE: <see cref="SynchronizationContext.Post(SendOrPostCallback, object?)"/> which reverses the responsibility of thread synchronization. </para>
    /// <para>Client code has just to link to the events and then process the passed <see cref="LogEntry"/> safely.</para>
    /// <para>The <see cref="LogEntry"/> provides methods such as <see cref="LogEntry.AsJson()"/>, <see cref="LogEntry.AsList()"/>, etc. 
    /// for getting a string representation of the entry.</para>
    /// <para>SEE: https://lostechies.com/gabrielschenker/2009/01/23/synchronizing-calls-to-the-ui-in-a-multi-threaded-application/</para>
    /// </summary>
    public class LogListenerSqlSynced : LogListener
    {
        // ● private
        const string SCreateTableSql = @"
create table {0}  (
   Id                     @NVARCHAR(40)    @NOT_NULL primary key
  ,Stamp                  @DATETIME        @NULL 
  ,LogDate                @NVARCHAR(12)    @NULL
  ,LogTime                @NVARCHAR(12)    @NULL
  ,UserName               @NVARCHAR(96)    @NULL
  ,HostName               @NVARCHAR(64)    @NULL
  ,Level                  @NVARCHAR(24)    @NULL     
  ,Source                 @NVARCHAR(96)    @NULL
  ,Scope                  @NVARCHAR(96)    @NULL
  ,EventId                @NVARCHAR(96)    @NULL
  ,Data                   @NBLOB_TEXT      @NULL
)
";

        int Counter = 0;
        SynchronizationContext fSyncContext;

        void OnEntryEvent(LogEntry Entry)
        {
            Dictionary<string, object> SqlParams = new Dictionary<string, object>();

            SqlParams["Id"] = Entry.Id;
            SqlParams["Stamp"] = Entry.TimeStamp;
            SqlParams["LogDate"] = Entry.Date;
            SqlParams["LogTime"] = Entry.Time;
            SqlParams["UserName"] = !string.IsNullOrWhiteSpace(Entry.User) ? Entry.User : string.Empty;
            SqlParams["HostName"] = !string.IsNullOrWhiteSpace(Entry.Host) ? Entry.Host : string.Empty;
            SqlParams["Level"] = Entry.LevelText;
            SqlParams["Source"] = !string.IsNullOrWhiteSpace(Entry.Source) ? Entry.Source : string.Empty;
            SqlParams["Scope"] = !string.IsNullOrWhiteSpace(Entry.ScopeId) ? Entry.ScopeId : string.Empty;
            SqlParams["EventId"] = !string.IsNullOrWhiteSpace(Entry.EventId) ? Entry.EventId : string.Empty;
            SqlParams["Data"] = Entry.AsJson();

            LogEntryArgsInsertSql Args = new LogEntryArgsInsertSql(Entry, SqlParams);

            InsertEntryEvent(this, Args);

            Counter++;
            ApplyRetainPolicy();
        }

        void ApplyRetainPolicy()
        {
            if (Counter > RetainPolicyCounter)
            {
                Counter = 0;
                DeleteEntriesOlderThan(RetainDays);
            }
        }
        void DeleteEntriesOlderThan(int Days)
        {
            if (ApplyRetainPolicyEvent != null)
            {
                DateTime StartDT = DateTime.UtcNow.AddDays(-Days);
                string sStartDT = StartDT.ToString("yyyy-MM-dd 00:00:00");
                string SqlText = $"delete from {TableName} where Stamp < '{sStartDT}'";

                LogEntryArgsRetainPolicySql Args = new LogEntryArgsRetainPolicySql(SqlText);
                ApplyRetainPolicyEvent(this, Args);
            }  
        }

        // ● construction
        /// <summary>
        /// Constructor.
        /// </summary>
        public LogListenerSqlSynced(string TableName)
            : base()
        {
            fSyncContext = AsyncOperationManager.SynchronizationContext;
            this.TableName = TableName;
        }

        // ● public
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        /// <para>This methods calls an event in order to insert the log entry into the database.</para>
        /// <para>It prepares a <see cref="LogEntryArgsInsertSql"/> object with the needed Sql parameters and then calls the <see cref="InsertEntryEvent"/> event.</para>
        /// <para>The client code event handler should insert the log entry into the database.</para>
        ///<para>
        /// <strong>CAUTION</strong>: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        public override void ProcessLog(LogEntry Entry)
        {
            if (InsertEntryEvent != null)
            {
                fSyncContext.Post(e => OnEntryEvent(e as LogEntry), Entry);
            }    
        }
        /// <summary>
        /// Returns a SQL statement for creating a table for the log listener.
        /// <para>The caller should provide the right data types for the database in use.</para>
        /// </summary>
        public string GetCreateTableSql(string NVarcharType = "nvarchar", string DateTimeType = "datetime", string NTextBlobType = "text", string Null = "null", string NotNull = "not null")
        {
            string Result = string.Format(SCreateTableSql, TableName);
            Result = Result.Replace("@NVARCHAR", NVarcharType);
            Result = Result.Replace("@DATETIME", DateTimeType);
            Result = Result.Replace("@NBLOB_TEXT", NTextBlobType);
            Result = Result.Replace("@NULL", Null);
            Result = Result.Replace("@NOT_NULL", NotNull);
            return Result;
        }

        // ● properties
        /// <summary>
        /// The name of the table for the log listener.
        /// </summary>
        public string TableName { get; }

        // ● events
        /// <summary>
        /// The event for inserting log entries into the database.
        /// <para>The arguments contain the needed Sql parameters along with their values, for the insert statement</para>
        /// </summary>
        public event EventHandler<LogEntryArgsInsertSql> InsertEntryEvent;
        /// <summary>
        /// The event for applying the retain policy.
        /// <para>The arguments contain the Sql statement to be executed, e.g.  </para>
        /// <para><c>delete from TABLE_NAME where Stamp &lt; 'SOME_DATE'</c></para>
        /// </summary>
        public event EventHandler<LogEntryArgsRetainPolicySql> ApplyRetainPolicyEvent;
    }
}
