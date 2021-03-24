using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    public class ProjectFolderService
    {
        private readonly ProjectFileService _projectFileService;
        private string _rootNamespace = default;
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
        //private Dictionary<string, NamespaceEntity> _namespaceDictionaryUnique = new Dictionary<string, NamespaceEntity>();
        private Dictionary<string, string> _dictionaryWithKeyNotEqualValue = new Dictionary<string, string>();

        public ProjectFolderService()
        {
            _projectFileService = new ProjectFileService();

        }
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

                //case when only one project file
                //if (files.Count == 1 && files.First().EndsWith(".csproj"))
                //{
                //    var projectFile = files.First(item => item.EndsWith(".csproj"));
                //    _rootNamespace = _projectFileService.GetRootNamespace(projectFile);
                //}

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
                //}
            }           
        }
    }
}
