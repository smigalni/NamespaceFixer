using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    public class ProjectFolderService
    {   
        public void FilterList(Dictionary<string, NamespaceEntity> namespaceDictionary,
            Dictionary<string, string> usingDictionary,
            string folder, string rootPath)
        {
            var projectFolderName = ProjectFolderNameService.GetProjectFolderName(folder);

            var foldersList = new List<string>();

            var filteredDirectories = SourceFilesService.GetFoldersWithSourceFiles(folder);
            filteredDirectories.Add(folder);            

            foreach (var dir in filteredDirectories)
            {
                var fileList = new List<string>();               

                foldersList.Add(dir);
                var files = Directory.GetFiles(dir).ToList();               

                SourceFilesService.FilterFiles(fileList, files);

                var namespaceList = NamespacesService.GetAllnamespacesFromFiles(rootPath, fileList, folder, dir);

                foreach (var namespaceItem in namespaceList)
                {
                    var correctNamespace = NamespacesService.CheckProjectFile(fileList, folder, dir, rootPath);

                    var dictionaryKey = $"{namespaceItem.NamespaceFromFile}_{namespaceItem.FolderName}";

                    if (!namespaceDictionary.TryGetValue(dictionaryKey, out _))
                    {
                        namespaceItem.CorrectNamespace = correctNamespace;
                        namespaceDictionary.Add(dictionaryKey, namespaceItem);
                    }
                    var namespaceKey = namespaceItem.NamespaceFromFile;
                    if (usingDictionary.TryGetValue(namespaceKey, out _))
                    {
                        Console.WriteLine($"Usings for the namespace {namespaceKey} has to be done manually");
                    }
                    else
                    {
                        usingDictionary.Add(namespaceKey, correctNamespace);
                    }
                }
            }           
        }
    }
}
