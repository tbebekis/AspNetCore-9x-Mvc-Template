namespace tp.Logging
{
    /// <summary>
    /// Event arguments used in conveying log entries.
    /// </summary>
    public class LogEntryArgs : EventArgs
    {
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public LogEntryArgs(LogEntry Entry)
        {
            this.Entry = Entry;
        }

        // ● properties
        /// <summary>
        /// The log entry
        /// </summary>
        public LogEntry Entry { get; }
    }
}
