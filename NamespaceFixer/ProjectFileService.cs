using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace NamespaceFixer
{
    public class ProjectFileService
    {      
        public string GetRootNamespace(string file)
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
        public void CheckRootNamespaces(List<string> projectFiles,
            string path)
        {
            var rootNamespaceList = new List<string> {
                GetRootNamespace(path)
            };

            if (!rootNamespaceList.Any())
            {
                return;
            }
            if (projectFiles.Count != rootNamespaceList.Count)
            {
                Console.WriteLine("Some project files has RootNamespace, but some doesn't. Check that all project files has RootNamespace or remove RootNamespaces from alle project files.");

            }
        }
        public void CheckThatFolderIncludesCsFiles(string folderName)
        {
            var csFiles = Directory.GetFiles(folderName, "*.cs", SearchOption.TopDirectoryOnly).ToList();
            if (!csFiles.Any())
            {
                Console.WriteLine($"There is no cs files in this project folder. Probably delete this folder.");
            }
        }
    }
}
