using System.Xml;

namespace NamespaceFixer
{
    public class ProjectFileService
    {
        public string GetRootNamespace(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.DocumentElement.SelectSingleNode("//RootNamespace");
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
    }
}
