using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace NamespaceFixer
{
    public class ProjectFileService
    {      
        public static string GetRootNamespace(string file)
        {
            var doc = new XmlDocument();
            doc.Load(file);

            var node = doc.DocumentElement.SelectSingleNode("//RootNamespace");
            if (node is null)
            {
                return default;
            }
            else
            {
                var rootNamespace = node.InnerText;
                return rootNamespace;
            }
        }
        public void CheckRootNamespaces(string path,
            List<string> rootNamespaceList)
        {
            rootNamespaceList.Add(GetRootNamespace(path));           
        }

        public void CheckThatFolderIncludesCsFiles(string folderName)
        {
            var csFiles = Directory.GetFiles(folderName, "*.cs", SearchOption.AllDirectories).ToList();
            if (!csFiles.Any())
            {
                Console.WriteLine($"There is no cs files in the project folder {folderName}. Probably delete this folder.");
            }
        }
        public List<string> GetAllProjectFiles(string rootPath)
        {
            return Directory
                .GetFiles(rootPath, "*.csproj", SearchOption.AllDirectories)
                .ToList();
        }
        public static string GetProjectFile(string rootPath)
        {
            return Directory
                .GetFiles(rootPath, "*.csproj", SearchOption.TopDirectoryOnly)
                .Single();
        }
        public static string GetProjectFileName(string path)
        {
            return Path.GetFileName(path).Replace(".csproj", string.Empty);
        }
    }
}
