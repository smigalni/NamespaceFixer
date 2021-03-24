using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NamespaceFixer
{
    public static class ChangeNamespacesAndUsingService
    {
        private static int _numberOfFilesTotal;
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
                _numberOfFilesTotal += allLinesFromFile.Count();

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
        }

        //private static void Change1(string rootPath)
        //{
        //    var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
        //    var myFiles = files.Where(item => !item.Contains("obj")).ToList();
        //    var newFileList = new List<string>();
        //    SourceFilesService.FilterFiles(newFileList, myFiles);

        //    foreach (var file in newFileList)
        //    {
        //        string[] arrLine = File.ReadAllLines(file);
        //        var fileChanged = false;
        //        for (int i = 0; i < arrLine.Count(); ++i)
        //        {
        //            if (arrLine[i].Contains("using"))
        //            {
        //                var usingItem = arrLine[i].Replace("using", string.Empty).Replace(";", string.Empty).Trim();
        //                _dictionaryWithKeyNotEqualValue.TryGetValue(usingItem, out string value);
        //                if (value is null)
        //                {

        //                }
        //                else
        //                {
        //                    fileChanged = true;
        //                    arrLine[i] = arrLine[i].Replace(usingItem, _dictionary[usingItem]);
        //                }
        //            }
        //            else if (arrLine[i].Contains("namespace"))
        //            {
        //                var namespaceItem = arrLine[i].Replace("namespace", "").Trim();
        //                _dictionaryWithKeyNotEqualValue.TryGetValue(namespaceItem, out string value);
        //                if (value is null)
        //                {

        //                }
        //                else
        //                {
        //                    fileChanged = true;
        //                    arrLine[i] = arrLine[i].Replace(namespaceItem, _dictionary[namespaceItem]);
        //                }
        //            }
        //        }
        //        if (fileChanged)
        //        {
        //            File.WriteAllLines(file, arrLine, Encoding.UTF8);
        //        }
        //    }
        //}
    }
}
