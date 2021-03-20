using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    public static class SourceFilesService
    {
        public static List<string> GetFoldersWithSourceFiles(string rootPath)
        {
            var directories = Directory
                .GetDirectories(rootPath, "*", SearchOption.AllDirectories)
                .ToList();
            var list = new List<string>();

            foreach (var directory in directories)
            {
                var folderName = directory.Remove(0, rootPath.Length + 1);

                if (FoldersToIgnore(folderName))
                {
                    continue;
                }
                else
                {
                    list.Add(directory);
                }
            }

            return list;
        }

        private static bool FoldersToIgnore(string folderName)
        {
            //TODO check that I have to ignore more folders
            return folderName.StartsWith(".") || folderName.Contains("bin") || folderName.Contains("obj") || folderName.Contains("Properties") || folderName.Contains("Migrations");
        }

        public static void FilterFiles(List<string> fileList, List<string> files)
        {
            var cSharpfiles = files.Where(item => item.EndsWith(".cs"));
            fileList.AddRange(cSharpfiles);
        }
    }
}
