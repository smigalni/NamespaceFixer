using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NamespaceFixer
{
    public class ApplicationStarter
    {
        private readonly FolderService _folderSerivce;
        private readonly ProjectFileService _projectFileService;

        public ApplicationStarter()
        {
            _folderSerivce = new FolderService();
            _projectFileService = new ProjectFileService();
        }
        
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
        private Dictionary<string, string> _dictionaryWithKeyNotEqualValue = new Dictionary<string, string>();

        private string _rootNamespace = default;
        public void Start(string rootPath)
        {
            var dirList = _folderSerivce.CompareProjectFolderWithCsprojFile(rootPath);

            var foldersList = new List<string>();
            foreach (var folder in dirList)
            {
                var name = folder.Remove(0, rootPath.Length);
                FilterList(foldersList, name, folder, rootPath);
            }

            RemoveEqualNamespaces();
            ChangeNamespaceInAllFiles(rootPath);

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
            FilterFiles(newFileList, myFiles);

            foreach (var file in newFileList)
            {
                string[] arrLine = File.ReadAllLines(file);
                var fileChanged = false;
                for (int i = 0; i < arrLine.Count(); ++i)
                {
                    if (arrLine[i].Contains("using"))
                    {
                        var usingItem = arrLine[i].Replace("using", string.Empty).Replace(";",string.Empty).Trim();
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

        private void GetAllnamespacesFromFiles(List<string> namespaceList, List<string> fileList)
        {      
            foreach (var file in fileList)
            {              
                var arrLine = File.ReadAllLines(file).ToList();

                string nm;
                try
                {
                    nm = arrLine.Single(item => item.Contains("namespace")).Replace("namespace", "").Trim();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Could not find namespace in the file: {file}");
                    throw;
                }
                

                var tets = namespaceList.Where(item => item == nm);

                if (!tets.Any())
                {
                    namespaceList.Add(nm);
                }
            }
        }

        

        private void FilterFiles(List<string> fileList, List<string> files)
        {
            var cSharpfiles = files.Where(item => item.EndsWith(".cs"));
            fileList.AddRange(cSharpfiles);
        }

        private void FilterList(List<string> foldersList, string name, string folder, string rootPath)
        {
            if (name.StartsWith("\\.") || name.Contains("bin") || name.Contains("obj") || name.Contains("Properties") || name.Contains("Pipelines") || name.Contains("Git") || name.Contains("Migrations"))
            {
            }
            else
            {
                string[] paths = {rootPath, folder};
                string fullPath = Path.Combine(paths);

                var dirs = Directory.GetDirectories(fullPath, "*", SearchOption.AllDirectories).ToList();

                var filteredDirs = new List<string>();
                filteredDirs.Add(folder);
                filteredDirs.AddRange(FilteredDirs(dirs, fullPath));
                

                foreach (var dir in filteredDirs)
                {
                    var fileList = new List<string>();
                    var namespaceList = new List<string>();

                    foldersList.Add(dir);
                    var files = Directory.GetFiles(dir).ToList();

                    //case when only one project file
                    if (files.Count == 1 && files.First().EndsWith(".csproj"))
                    {
                        var projectFile = files.First(item => item.EndsWith(".csproj"));
                        _rootNamespace = _projectFileService.GetRootNamespace(projectFile);
                    }

                    FilterFiles(fileList, files);

                    GetAllnamespacesFromFiles(namespaceList, fileList);

                    foreach (var namespaceItem in namespaceList)
                    {
                        var nm = CheckProjectFile(files, dir, rootPath);
                        if (!_dictionary.TryGetValue(namespaceItem, out string key))
                        {
                            _dictionary.Add(namespaceItem, nm);
                        }
                    }
                }
            }
        }

        private string CreateNamespace(string dir, string rootPath)
        {
            //Antar at folders har riktig navn
            var rootFolder = dir.Remove(0, rootPath.Length+1);
            var nm = rootFolder.Replace("\\",".");
            return nm;
        }

        private List<string> FilteredDirs(List<string> dirs, string fullPath)
        {
            var list = new List<string>();
            
            foreach (var dir in dirs)
            {
                var name = dir.Remove(0, fullPath.Length);
                if (name.StartsWith("\\.") || name.Contains("bin") || name.Contains("obj") || name.Contains("Properties") || name.Contains("Pipelines") || name.Contains("Git") || name.Contains("Migrations"))
                {
                }
                else
                {
                    list.Add(dir);
                }               
            }
            return list;
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
    }
}