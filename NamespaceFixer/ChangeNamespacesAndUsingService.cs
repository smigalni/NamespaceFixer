using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NamespaceFixer
{
    public static class ChangeNamespacesAndUsingService
    {
        private static int _numberOfCodeLinesTotal;
        public static void Change(string rootPath,
            Dictionary<string, NamespaceEntity> namespaceDictionary,
            Dictionary<string, string> usingDictionary)
        {
            var files = Directory
                .GetFiles(rootPath, "*.cs", SearchOption.AllDirectories)
                .Where(item => !item.Contains("obj"))
                .ToList();

            foreach (var file in files)
            {
                var directory = Path.GetDirectoryName(file);
                var folderName = directory.Remove(0, rootPath.Length + 1);

                var allLinesFromFile = File.ReadAllLines(file);
                _numberOfCodeLinesTotal += allLinesFromFile.Count();

                var fileChanged = false;

                for (int i = 0; i < allLinesFromFile.Count(); i++)
                {
                    if (allLinesFromFile[i].StartsWith("using"))
                    {
                        var lineWithoutUsing = allLinesFromFile[i].Replace("using", string.Empty).Replace(";", string.Empty).Trim();
                        var key = lineWithoutUsing;
                        usingDictionary.TryGetValue(key, out var entity);
                        if (entity is null)
                        {
                            continue;
                        }
                        else
                        {
                            fileChanged = true;
                            allLinesFromFile[i] = allLinesFromFile[i].Replace(lineWithoutUsing, entity);
                        }

                    }
                    if (allLinesFromFile[i].StartsWith("namespace"))
                    {
                        var namespaceItem = allLinesFromFile[i].Replace("namespace", "").Trim();
                        var key = $"{namespaceItem}_{folderName}";
                        namespaceDictionary.TryGetValue(key, out var entity);
                        if (entity is null)
                        {
                            continue;
                        }
                        else
                        {
                            fileChanged = true;
                            allLinesFromFile[i] = allLinesFromFile[i].Replace(namespaceItem, entity.CorrectNamespace);
                        }
                    }
                }

                if (fileChanged)
                {
                    File.WriteAllLines(file, allLinesFromFile, Encoding.UTF8);
                }
            }
            Console.WriteLine($"The total number of code lines in your solution is {_numberOfCodeLinesTotal}. Migrations included.");
            Console.WriteLine($"The total number of source files in your solution is {files.Count}.  Migrations included.");
        }       
    }
}
