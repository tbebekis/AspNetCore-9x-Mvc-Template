namespace MvcApp.Library
{
    /// <summary>
    /// Lib.Filea
    /// </summary>
    static public partial class Lib
    {

        static public bool IsDirectory(string PhysicalPath)
        {
            return Directory.Exists(PhysicalPath);
        }

        static public void DeleteFolder(string FolderPath)
        {
            Directory.Delete(FolderPath, true);
            int MaxIterations = 10;
            int Counter = 0;

            // wait until the directory is actually deleted
            // and not just marked as deleted
            while (Directory.Exists(FolderPath))
            {
                Counter += 1;

                if (Counter > MaxIterations)
                    return;

                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Converts a physical path to a virtual path.
        /// <para><c>C:\MyApp\folder</c> becomes <c>~/folder</c></para>
        /// </summary>
        static public string ToVirtualPath(string PhysicalPath)
        {
 
            string Result = "";

            if (!string.IsNullOrWhiteSpace(PhysicalPath) && (File.Exists(PhysicalPath) || Directory.Exists(PhysicalPath)))
            {
                string S = "";

                if (PhysicalPath.Contains(Lib.AppContext.BinPath, StringComparison.OrdinalIgnoreCase))
                    S = PhysicalPath.Replace(Lib.AppContext.BinPath, string.Empty);
                else if(PhysicalPath.Contains(Lib.AppContext.ContentRootPath, StringComparison.OrdinalIgnoreCase))
                    S = PhysicalPath.Replace(Lib.AppContext.ContentRootPath, string.Empty);                   
                else if (PhysicalPath.Contains(Lib.AppContext.WebRootPath, StringComparison.OrdinalIgnoreCase))
                    S = PhysicalPath.Replace(Lib.AppContext.WebRootPath, string.Empty);

                S = S.Replace('\\', '/').Trim('/').TrimStart('~', '/');
                    
                Result = $"~/{S ?? string.Empty}";
            }

            return Result;
        }
        /// <summary>
        /// Converts a virtual path to a physical path assuming that the specified virtual path is relative to the web root, i.e. wwwroot.
        /// <para><c>~/wwwroot</c> becomes <c>C:\MyApp\wwwroot</c></para>
        /// </summary>
        static public string ToPhysicalPath(string VirtualPath)
        {
            string Result = "";

            if (!string.IsNullOrWhiteSpace(VirtualPath))
            {
                string S = VirtualPath.Replace("~/", "wwwroot/").TrimStart('/');

                if (Lib.IsWindows())
                {
                    S = S.Replace('/', '\\');
                }

                // retain the trailing path separator, if any.
                string TrailingPathSeparator = S.EndsWith('/') ? Path.DirectorySeparatorChar.ToString() : string.Empty;

                Result = Path.Combine(Lib.AppContext.ContentRootPath ?? string.Empty, S) + TrailingPathSeparator;
            }

            
            return Result;
        }
    }
}
