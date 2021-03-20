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
        public List<string> RunDifferentChecks(string rootPath)
        {
            var projectFolders = new List<string>();
            var projectFiles = new List<string>();

            var paths = Directory.GetFiles(rootPath, "*.csproj", SearchOption.AllDirectories).ToList();          

            foreach (var path in paths)
            {
                var folderName = ProjectFileFolderShouldBeTheSameWithProjectFileName(projectFolders, projectFiles, path);

                _projectFileService.CheckRootNamespaces(projectFiles, path);

                _projectFileService
                    .CheckThatFolderIncludesCsFiles(folderName);
            }
            return projectFolders;
        }

        private string ProjectFileFolderShouldBeTheSameWithProjectFileName(
            List<string> projectFolders, 
            List<string> projectFiles, string path)
        {
            var filename = Path.GetFileName(path).Replace(".csproj", string.Empty);
            projectFiles.Add(filename);

            var folderName = Path.GetDirectoryName(path);
            projectFolders.Add(folderName);
           
            var projectFolderName = ProjectFolderNameService.GetProjectFolderName(folderName);

            if (filename != projectFolderName)
            {
                LogAndThrowException(filename, folderName, projectFolderName);
            }

            return folderName;
        }

        private static void LogAndThrowException(string filename, string folderName, string projectFolderName)
        {
            Console.WriteLine($"Project file is {filename}.csproj " +
                $"but project file folder is {projectFolderName}. " +
                $"Should be the same. Check the folder {folderName}");

            throw new Exception($"Project file is {filename}.csproj " +
                $"but project file folder is {projectFolderName}. " +
                $"" +
                $"Should be the same. Check the folder {folderName}");
        }
    }
}