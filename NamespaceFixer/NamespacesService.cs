using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NamespaceFixer
{
    public static class NamespacesService
    {
       
        public static List<NamespaceEntity> GetAllnamespacesFromFiles(string rootPath,
            List<string> fileList,
            string projecFolder,
            string directory)
        {
            var namespaceEntities = new List<NamespaceEntity>();
            foreach (var file in fileList)
            {
                var allLinesFromFile = File.ReadAllLines(file).ToList();
              

                string namespaceFromFile;
                try
                 {
                    namespaceFromFile = allLinesFromFile
                        .Single(item => item.Contains("namespace"))
                        .Replace("namespace", string.Empty).Trim();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Could not find namespace in the file: {file}");
                    throw;
                }

                CheckIfAlreadyExists(rootPath, namespaceEntities, namespaceFromFile, projecFolder, directory);
            }
            return namespaceEntities;
        }

        private static void CheckIfAlreadyExists(string rootPath,
            List<NamespaceEntity> namespaceEntities,
            string namespaceFromFile,
            string projecFolder,
            string directory)
        {
            var folderName = string.Empty;
            if (directory == projecFolder)
            {
                folderName = ProjectFolderNameService.GetProjectFolderName(directory);
            }
            else
            {
                folderName = directory.Remove(0, rootPath.Length + 1);
            }

            if (!namespaceEntities.Where(item => item.NamespaceFromFile == namespaceFromFile && item.FolderName == folderName).Any())
            {
                namespaceEntities.Add(new NamespaceEntity 
                {
                    FolderName = folderName,
                    NamespaceFromFile = namespaceFromFile
                });
            }
        }
        public static string CheckProjectFile(List<string> files,
            string projecFolder,
            string directory,
            string rootPath)
        {
            var projectFile = ProjectFileService.GetProjectFile(projecFolder);

            var rootNamespace = ProjectFileService.GetRootNamespace(projectFile);
            if (rootNamespace is null)
            {
                var rootNamespaceNew = ProjectFileService.GetProjectFileName(projectFile);
                return CreateNamespace(directory, projecFolder, rootNamespaceNew);
            }
            else
            {
                return CreateNamespace(directory, projecFolder, rootNamespace);
            }
        }

        private static string CreateNamespace(string directory,
             string projecFolder,
            string rootNamespace)
        {
            if (directory == projecFolder)
            {
                return rootNamespace;
            }
            else
            {
                var folderName = directory.Remove(0, projecFolder.Length + 1);
                var foldersArray = folderName.Split(Path.DirectorySeparatorChar);
                var sb = new StringBuilder();
                sb.Append(rootNamespace);
                foreach (var folder in foldersArray)
                {
                    sb.Append($".{folder}");
                }
                return sb.ToString();
            }
        }

        public static Dictionary<string, NamespaceEntity> RemoveEqualNamespaces(Dictionary<string, NamespaceEntity> dictionary)
        {
            var namespaceDictionaryUnique = new Dictionary<string, NamespaceEntity>();
            foreach (var item in dictionary)
            {
                if (item.Value.CorrectNamespace != item.Value.NamespaceFromFile)
                {
                    namespaceDictionaryUnique[item.Key] = item.Value;
                }
            }
            return namespaceDictionaryUnique;
        }
        
    }
}
