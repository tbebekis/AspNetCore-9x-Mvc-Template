namespace tp.Logging
{
    /// <summary>
    /// A log listener that writes <see cref="LogEntry"/> log info to file(s).
    /// <para><strong>NOTE: </strong> Applies retain policy. By default keeps log files 7 days old.</para>
    /// </summary>
    public class LogListenerFile : LogListener
    {
        // ● private
        int Counter = 0;
        WriteLineFile LogFile;
 
        void ApplyRetainPolicy()
        {
            if (Counter > RetainPolicyCounter)
            {
                Counter = 0;
                LogFile.DeleteFilesOlderThan(RetainDays);
            }
        }
 
        // ● construction
        /// <summary>
        /// Constructor
        /// </summary>
        public LogListenerFile(string Folder = "", int MaxSizeKiloBytes = 512)
                : base()
        {
            this.MaxSizeKiloBytes = MaxSizeKiloBytes;

            string AppPath = Environment.GetCommandLineArgs()[0];

            if (string.IsNullOrWhiteSpace(Folder))
            {
                Folder = Path.GetDirectoryName(AppPath);
                Folder = Path.Combine(Folder, "Logs");
            }                

            string DefaultFileName = Path.GetFileName(AppPath);
            DefaultFileName = Path.ChangeExtension(DefaultFileName, ".log");
 
            string ColumnLine = Logger.GetLineCaptions();
            LogFile = new WriteLineFile(Folder, DefaultFileName, ColumnLine, MaxSizeKiloBytes);
        }

        // ● public
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        public override void ProcessLog(LogEntry Entry)
        {
            string Line = Logger.GetAsLine(Entry);
            LogFile.WriteLine(Line);

            Counter++;
            ApplyRetainPolicy();
        }

        // ● properties
        /// <summary>
        /// The folder where log files are placed. Defaults to Sys.AppRootDataFolder/Logs
        /// </summary>
        public string Folder { get; private set; }
 
    }


}
