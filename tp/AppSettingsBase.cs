namespace tp
{
    /// <summary>
    /// Base class for application settings.
    /// <para>Watches for changes in the settings file and reloads this instance.</para>
    /// <para><strong>NOTE: </strong> If this class contains lists, 
    /// then override the <see cref="BeforeLoad()"/> method and clean those lists.</para>
    /// </summary>
    public class AppSettingsBase
    {
        const string SFileName = "ApplicationSettings.json";

        // ● protected 
        protected string Folder { get; private set; }
        protected string FileName { get; private set; }
        protected string FilePath { get; private set; }
        protected FileSystemWatcher Watcher { get; private set; }
        protected bool IsSaving { get; private set; }
        protected bool IsReloadable { get; private set; }

        void CreateWatcher()
        {
            DisposeWatcher();

            Watcher = new FileSystemWatcher(this.Folder);
            Watcher.NotifyFilter = NotifyFilters.LastWrite;
            Watcher.Filter = this.FileName;
            Watcher.EnableRaisingEvents = true;
            Watcher.Changed += Watcher_Changed;
        }
        void DisposeWatcher()
        {
           if (Watcher != null)
            {
                Watcher.EnableRaisingEvents = false;
                Watcher.Changed -= Watcher_Changed;
                Watcher.Dispose();
            }
        }

        /// <summary>
        /// event handler
        /// </summary>
        protected void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!IsSaving)
            {
                string S = e.FullPath.ToLower();
                string S2 = FilePath.ToLower();

                CreateWatcher();

                if (S == S2)
                {
                    // directly executing Load() here, a second time, will throw
                    // System.IO.IOException: 'The process cannot access the file because it is being used by another process' 
                    // so we need to wait, inside a thread
                    Sys.TaskFactory.StartNew(() =>
                    {
                        Thread.Sleep(1000);
                        try
                        {
                            Load();
                        }
                        catch 
                        {
                        }                             
                    });
                }
                 
            }
        }

        // ● overridables
        /// <summary>
        /// Called just before <see cref="Load()"/> method. Default implementation does nothing.
        /// <para><strong>NOTE: </strong> If this class contains lists, 
        /// then override the <see cref="BeforeLoad()"/> method and clean those lists.</para>
        /// <para></para>
        /// </summary>
        protected virtual void BeforeLoad() { }
        /// <summary>
        /// Called just after <see cref="Load()"/> method. Default implementation does nothing.
        /// </summary>
        protected virtual void AfterLoad() { }
        /// <summary>
        /// Called just before <see cref="Save()"/> method. Default implementation does nothing.
        /// <para><strong>NOTE: </strong> The <see cref="IsSaving"/> protected property is set to true when this method is called.</para>
        /// </summary>
        protected virtual void BeforeSave() { }
        /// <summary>
        /// Called just after <see cref="Save()"/> method. Default implementation does nothing.
        /// <para><strong>NOTE: </strong> The <see cref="IsSaving"/> protected property is set to true when this method is called.</para>
        /// </summary>
        protected virtual void AfterSave() { }

        // ● construction 
        /// <summary>
        /// Private constructor
        /// </summary>
        private AppSettingsBase()
        {
        }
        /// <summary>
        /// Constructor.
        /// <para>Writes the file if not exists, and loads the file, so settings are available from the creation of this instance.</para>
        /// <para>Also it sets up a <see cref="FileSystemWatcher"/> to detect changes in the specified file and re-load it.</para>
        /// </summary>
        protected AppSettingsBase(string Folder = "", string FileName = "", bool IsReloadable = true)
        {
            if (string.IsNullOrWhiteSpace(Folder))
                Folder = System.AppContext.BaseDirectory; // Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            if (string.IsNullOrWhiteSpace(FileName))
                FileName = SFileName;

            this.Folder = Folder;
            this.FileName = FileName;
            this.IsReloadable = IsReloadable;

            this.FilePath = Path.Combine(this.Folder, this.FileName);

            if (!File.Exists(this.FilePath))
            {
                if (!Directory.Exists(this.Folder))
                    Directory.CreateDirectory(this.Folder);
                Save();
            }

            Load();

            if (IsReloadable)
            {
                CreateWatcher();
            }

        }

        // ● public
        /// <summary>
        /// Loads this instance's properties from the disk file
        /// </summary>
        public virtual void Load()
        {
            BeforeLoad();
            Json.LoadFromFile(this, FilePath);
            AfterLoad();
        }
        /// <summary>
        /// Saves this instance to a disk file
        /// </summary>
        public virtual void Save()
        {
            IsSaving = true;
            try
            {
                BeforeSave();
                Json.SaveToFile(this, FilePath);
                AfterSave();
            }
            finally
            {
                IsSaving = false;
            }
        }
    }
}
