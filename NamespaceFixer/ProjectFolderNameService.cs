using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    public static class ProjectFolderNameService
    {
        public static string GetProjectFolderName(string path) 
        {
            return path.Split(Path.DirectorySeparatorChar).Last();
        }
    }
}
