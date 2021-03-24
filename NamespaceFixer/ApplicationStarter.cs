using System.Collections.Generic;

namespace NamespaceFixer
{
    public class ApplicationStarter
    {
        private readonly FolderService _folderSerivce;
        private readonly ProjectFolderService _projectFolderService;
       

        public ApplicationStarter()
        {
            _folderSerivce = new FolderService();
            _projectFolderService = new ProjectFolderService();
            
        }  
      
        public void Start(string rootPath)
        {
            var namespaceDictionary = new Dictionary<string, NamespaceEntity>();
            var usingDictionary = new Dictionary<string, string>();

            var projectFolders = _folderSerivce.RunDifferentChecks(rootPath);

            foreach (var folder in projectFolders)
            {
                _projectFolderService.FilterList(namespaceDictionary, usingDictionary, folder, rootPath);
            }
            var namespaceDictionaryUnique = NamespacesService.RemoveEqualNamespaces(namespaceDictionary);

            ChangeNamespacesAndUsingService.Change(rootPath, namespaceDictionaryUnique, usingDictionary);
        }   
    }
}