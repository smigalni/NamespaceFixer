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
            var projectFolders = _folderSerivce.RunDifferentChecks(rootPath);

            foreach (var folder in projectFolders)
            {
                _projectFolderService.FilterList(folder, rootPath);
            }
        }   
    }
}