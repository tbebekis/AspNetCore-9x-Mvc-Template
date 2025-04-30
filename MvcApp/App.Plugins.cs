namespace MvcApp
{
    static internal partial class App
    { 
        static void CleanPluginFolder(string PluginFolderPath)
        {
            string[] Patterns = {"Plug.WebLib.*", "*.pdb", "*.deps.json" };
            string[] FilePaths;
            foreach (string Pattern in Patterns)
            {
                FilePaths = Directory.GetFiles(PluginFolderPath, Pattern);
                if (FilePaths != null && FilePaths.Length > 0)
                {
                    foreach (string FilePath in FilePaths)
                    {
                        if (File.Exists(FilePath))
                            File.Delete(FilePath);
                    }
                }
            }      
        }
        static void LoadPlugin(ApplicationPartManager PartManager, string PluginFolderPath)
        {
            // load the plugin definition
            CleanPluginFolder(PluginFolderPath);
            string PluginDefFilePath = Path.Combine(PluginFolderPath, "plugin-def.json");
            MvcAppPluginDef Def = new MvcAppPluginDef(PluginFolderPath);
 
            Json.LoadFromFile(Def, PluginDefFilePath);

            // find plugin assembly file path
            string PluginAssemblyFilePath;
            string[] FilePaths = Directory.GetFiles(PluginFolderPath, "Plugin.*.dll");
            if (FilePaths == null || FilePaths.Length == 0)
                Sys.Throw($"No Plugin Assembly found in folder: {PluginFolderPath}");
            PluginAssemblyFilePath = FilePaths[0];

            // load the assembly and the application part for that assembly
            Assembly PluginAssembly = Assembly.LoadFrom(PluginAssemblyFilePath); 
            ApplicationPart Part = new AssemblyPart(PluginAssembly);
            PartManager.ApplicationParts.Add(Part);

            Def.PluginAssembly = PluginAssembly;
            Def.Id = Path.GetFileName(PluginAssemblyFilePath);

            PluginDefList.Add(Def);
        }
        static void LoadPlugins(ApplicationPartManager PartManager)
        { 
            string RootPluginFolder = Path.Combine(App.BinPath, "Plugins");
            string[] PluginFolders = Directory.GetDirectories(RootPluginFolder);

            // load plugin definitions and assemblies
            DirectoryInfo DI;
            string FolderName;
            foreach (string PluginFolderPath in PluginFolders)
            {
                DI = new DirectoryInfo(PluginFolderPath);
                FolderName = DI.Name;
                if (FolderName.StartsWith("Plugin."))
                {
                    //LoadPlugin(PartManager, PluginFolderPath);

                    // load the plugin definition
                    CleanPluginFolder(PluginFolderPath);
                    string PluginDefFilePath = Path.Combine(PluginFolderPath, "plugin-def.json");
                    MvcAppPluginDef Def = new MvcAppPluginDef(PluginFolderPath);

                    Json.LoadFromFile(Def, PluginDefFilePath);

                    // find plugin assembly file path  
                    string[] FilePaths = Directory.GetFiles(PluginFolderPath, "Plugin.*.dll");
                    if (FilePaths == null || FilePaths.Length == 0)
                        Sys.Throw($"No Plugin Assembly found in folder: {PluginFolderPath}");
                    Def.PluginAssemblyFilePath = FilePaths[0];
                    Def.Id = Path.GetFileName(Def.PluginAssemblyFilePath);

                    PluginDefList.Add(Def);
                }
            }

            // sort definition list
            PluginDefList = PluginDefList.OrderBy(item => item.LoadOrder).ToList();

            // create plugins
            List<Type> ImplementorClassTypes;
            foreach (MvcAppPluginDef Def in PluginDefList)
            {
                // load the assembly and the application part for that assembly
                Def.PluginAssembly = Assembly.LoadFrom(Def.PluginAssemblyFilePath);
                ApplicationPart Part = new AssemblyPart(Def.PluginAssembly);
                PartManager.ApplicationParts.Add(Part);

                ImplementorClassTypes = TypeFinder.FindImplementorClasses(typeof(IMvcAppPlugin), Def.PluginAssembly);
                if (ImplementorClassTypes.Count == 0)
                    Sys.Throw($"Plugin: {Def.Id} does not implement IAppPlugin");

                if (ImplementorClassTypes.Count > 1)
                    Sys.Throw($"Plugin: {Def.Id} implements more than one IAppPlugin");

                IMvcAppPlugin Plugin = (IMvcAppPlugin)Activator.CreateInstance(ImplementorClassTypes[0]);
                Plugin.Descriptor = Def;
                PluginList.Add(Plugin);
            }
        }
        static void InitializePlugins()
        {
            foreach (IMvcAppPlugin Plugin in PluginList)
            {
                Plugin.Initialize(AppContext);
                Plugin.AddViewLocations();
            }
        }
 
        static public List<MvcAppPluginDef> PluginDefList { get; private set; } = new List<MvcAppPluginDef>();
        static public List<IMvcAppPlugin> PluginList { get; } = new List<IMvcAppPlugin>();  
    }
}
