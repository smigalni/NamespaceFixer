using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NamespaceFixer
{
    public class ProjectFolderService
    {
        private readonly ProjectFileService _projectFileService;
        private string _rootNamespace = default;
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
        private Dictionary<string, string> _dictionaryWithKeyNotEqualValue = new Dictionary<string, string>();

        public ProjectFolderService()
        {
            _projectFileService = new ProjectFileService();

        }
        public void FilterList(string folder, string rootPath)
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

                var namespaceList = NamespacesService.GetAllnamespacesFromFiles(fileList);

                foreach (var namespaceItem in namespaceList)
                {
                    var nm = CheckProjectFile(files, dir, rootPath);
                    if (!_dictionary.TryGetValue(namespaceItem, out string key))
                    {
                        _dictionary.Add(namespaceItem, nm);
                    }
                }
                //}
            }

            RemoveEqualNamespaces();

            ChangeNamespaceInAllFiles(rootPath);
        }

       

      
        
        private string CheckProjectFile(List<string> files, string dir, string rootPath)
        {
            var projectFile = files.SingleOrDefault(item => item.EndsWith(".csproj"));
            if (projectFile is null)
            {
            }
            else
            {
                _rootNamespace = _projectFileService.GetRootNamespace(projectFile);
            }
            if (_rootNamespace is null)
            {
                return CreateNamespace(dir, rootPath);
            }
            else
            {
                var rootFolder = dir.Remove(0, rootPath.Length + 1);
                //Antar at folders har riktig navn
                var folderArray = rootFolder.Split("\\");
                var nm = rootFolder.Replace(folderArray.First(), _rootNamespace).Replace("\\", ".");

                return nm;
            }
        }
        private string CreateNamespace(string dir, string rootPath)
        {
            //Antar at folders har riktig navn
            var rootFolder = dir.Remove(0, rootPath.Length + 1);
            var nm = rootFolder.Replace("\\", ".");
            return nm;
        }
        private void RemoveEqualNamespaces()
        {
            foreach (var item in _dictionary)
            {
                if (item.Key != item.Value)
                {
                    _dictionaryWithKeyNotEqualValue[item.Key] = item.Value;
                }
            }
        }

        private void ChangeNamespaceInAllFiles(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            var myFiles = files.Where(item => !item.Contains("obj")).ToList();
            var newFileList = new List<string>();
            SourceFilesService.FilterFiles(newFileList, myFiles);

            foreach (var file in newFileList)
            {
                string[] arrLine = File.ReadAllLines(file);
                var fileChanged = false;
                for (int i = 0; i < arrLine.Count(); ++i)
                {
                    if (arrLine[i].Contains("using"))
                    {
                        var usingItem = arrLine[i].Replace("using", string.Empty).Replace(";", string.Empty).Trim();
                        _dictionaryWithKeyNotEqualValue.TryGetValue(usingItem, out string value);
                        if (value is null)
                        {

                        }
                        else
                        {
                            fileChanged = true;
                            arrLine[i] = arrLine[i].Replace(usingItem, _dictionary[usingItem]);
                        }
                    }
                    else if (arrLine[i].Contains("namespace"))
                    {
                        var namespaceItem = arrLine[i].Replace("namespace", "").Trim();
                        _dictionaryWithKeyNotEqualValue.TryGetValue(namespaceItem, out string value);
                        if (value is null)
                        {

                        }
                        else
                        {
                            fileChanged = true;
                            arrLine[i] = arrLine[i].Replace(namespaceItem, _dictionary[namespaceItem]);
                        }
                    }
                }
                if (fileChanged)
                {
                    File.WriteAllLines(file, arrLine, Encoding.UTF8);
                }
            }
        }
    }
}
