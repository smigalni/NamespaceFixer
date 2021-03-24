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

            var paths = _projectFileService.GetAllProjectFiles(rootPath);

            var rootNamespaceList = new List<string>();
            foreach (var path in paths)
            {
                var folderName = ProjectFileFolderShouldBeTheSameWithProjectFileName(projectFolders, projectFiles, path);

                _projectFileService.CheckRootNamespaces(path, rootNamespaceList);

                _projectFileService
                    .CheckThatFolderIncludesCsFiles(folderName);
            }
            if (paths.Count != rootNamespaceList.Count)
            {
                Console.WriteLine("Some project files has RootNamespace, but some doesn't. Check that all project files has RootNamespace or remove RootNamespaces from alle project files.");

            }
            return projectFolders;
        }

        private string ProjectFileFolderShouldBeTheSameWithProjectFileName(
            List<string> projectFolders, 
            List<string> projectFiles, string path)
        {
            var filename = ProjectFileService.GetProjectFileName(path);
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