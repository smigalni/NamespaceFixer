using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NamespaceFixer
{
    public static class NamespacesService
    {
        public static List<string> GetAllnamespacesFromFiles( List<string> fileList)
        {
            var namespaces = new List<string>();
            foreach (var file in fileList)
            {
                var allLinesFromFile = File.ReadAllLines(file).ToList();

                string namespaceFromFile;
                try
                {
                    namespaceFromFile = allLinesFromFile
                        .Single(item => item.Contains("namespace"))
                        .Replace("namespace", string.Empty).Trim();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Could not find namespace in the file: {file}");
                    throw;
                }

                CheckIfAlredyExists(namespaces, namespaceFromFile);
            }
            return namespaces;
        }

        private static void CheckIfAlredyExists(List<string> namespaces, string namespaceFromFile)
        {
            if (!namespaces.Where(item => item == namespaceFromFile).Any())
            {
                namespaces.Add(namespaceFromFile);
            }
        }
    }
}
