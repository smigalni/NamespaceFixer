using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    public class FolderService
    {
        private readonly ProjectFileService _projectFileService;
        public FolderService()
        {
            _projectFileService = new ProjectFileService();
        }
        public void CompareProjectFolderWithCsprojFile(string rootPath)
        {
            var paths = Directory.GetFiles(rootPath, "*.csproj", SearchOption.AllDirectories).ToList();

            var csprojList = new List<string>();
            var rootNamespaceList = new List<string>();

            foreach (var path in paths)
            {
                var filename = Path.GetFileName(path).Replace(".csproj", string.Empty);
                csprojList.Add(filename);
                var dirName = Path.GetDirectoryName(path);
                var lastDirName = dirName.Split(Path.DirectorySeparatorChar).Last();

                if (filename != lastDirName)
                {
                    Console.WriteLine($"Project file is {filename}.csproj but project file folder is {lastDirName}. Should be the same. Check the folder {dirName}");

                    throw new Exception($"Project file is {filename}.csproj but project file folder is {lastDirName}. Should be the same. Check the folder {dirName}");
                }
                rootNamespaceList.Add(_projectFileService.GetRootNamespace(path));

                CheckRootNamespaces(csprojList,rootNamespaceList);
            }

        }

        private void CheckRootNamespaces(List<string> csprojList, List<string> rootNamespaceList)
        {
            if (!rootNamespaceList.Any())
            {
                return;
            }
            if (csprojList.Count != rootNamespaceList.Count)
            {
                Console.WriteLine("Check that all project files has RootNamespace");

            }
        }
    }
}