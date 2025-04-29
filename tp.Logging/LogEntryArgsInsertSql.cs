namespace tp.Logging
{
 
    /// <summary>
    /// Event arguments used in inserting log entries to a database table.
    /// </summary>
    public class LogEntryArgsInsertSql : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogEntryArgsInsertSql(LogEntry Entry, Dictionary<string, object> SqlParams)
        {
            this.Entry = Entry;
            this.SqlParams = SqlParams;
        }

        /// <summary>
        /// The log entry
        /// </summary>
        public LogEntry Entry { get; }
        /// <summary>
        /// The Sql parameters along with their values.
        /// </summary>
        public Dictionary<string, object> SqlParams { get; }  
    }

}
