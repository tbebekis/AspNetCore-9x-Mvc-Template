namespace tp.Logging
{
    /// <summary>
    /// Event arguments used in applying retain policy to a database log table.
    /// </summary>
    public class LogEntryArgsRetainPolicySql : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogEntryArgsRetainPolicySql(string SqlText)
        {
            this.SqlText = SqlText;
        }

        /// <summary>
        /// SQL statement for deleting old log entries.
        /// <para><c>delete from TABLE_NAME where Stamp &lt; 'SOME_DATE'</c></para>
        /// </summary>
        public string SqlText { get; }

    }
}
