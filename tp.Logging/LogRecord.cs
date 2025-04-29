namespace tp.Logging
{
    /// <summary>
    /// Used in convertint a <see cref="LogEntry"/> to a json serializable object.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LogRecord
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public LogRecord(LogEntry Entry)
        {
            Id = Entry.Id;
            TimeStamp = Entry.TimeStamp;
            Date = Entry.Date;
            Time = Entry.Time;
            User = Entry.User;
            Host = Entry.Host;
            Level = Entry.LevelText;
            Source = Entry.Source;
            Scope = Entry.ScopeId;
            EventId = Entry.EventId;
            Message = Entry.Text;

            if (Entry.Properties != null && Entry.Properties.Count > 0)
                Properties = Entry.GetPropertiesAsSingleLine();

            if (!string.IsNullOrWhiteSpace(Entry.ExceptionData))
                Stack = Entry.ExceptionData;
        }

        // ● properties
        /// <summary>
        /// The Id of this entry. A guid with surrounding brackets
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Returns the UTC date-time this info created
        /// </summary>
        public DateTime TimeStamp { get; }
        /// <summary>
        /// Returns the UTC date this info created.
        /// </summary>
        public string Date { get; }
        /// <summary>
        /// Returns the UTC time this info created.
        /// </summary>
        public string Time { get; }
        /// <summary>
        /// The username of the current user of this application or the local computer
        /// </summary>
        public string User { get; }
        /// <summary>
        /// The name of the local computer
        /// </summary>
        public string Host { get; }
        /// <summary>
        /// The log level
        /// </summary>
        public string Level { get; }
        /// <summary>
        /// The source of this log, if any
        /// </summary>
        public string Source { get; }
        /// <summary>
        /// The scope if any
        /// </summary>
        public string Scope { get; }
        /// <summary>
        /// The event Id, if any
        /// </summary>
        public string EventId { get; }
        /// <summary>
        /// The text message to log 
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// <see cref="LogEntry.Properties"/> is a dictionary with params passed when the log message was formatted. For use by structured log listeners.
        /// <para>These properties become a single text line here.</para>
        /// </summary>
        public string Properties { get; }
        /// <summary>
        /// The exception data, if any.
        /// </summary>
        public string Stack { get; }
 
    }
}
