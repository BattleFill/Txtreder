using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Txtred
{
    class FileManager
    {
        private string filePath;

        public FileManager(string path)
        {
            filePath = path;
        }

        public string[] LoadFile()
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".txt":
                    return File.ReadAllLines(filePath);
                case ".json":
                    return JsonSerializer.Deserialize<string[]>(File.ReadAllText(filePath));
                case ".xml":
                    var doc = new XmlDocument();
                    doc.Load(filePath);
                    var lines = new string[doc.DocumentElement.ChildNodes.Count];
                    for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
                    {
                        lines[i] = doc.DocumentElement.ChildNodes[i].InnerText;
                    }
                    return lines;
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }

        public void SaveFile(string[] data)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".txt":
                    File.WriteAllLines(filePath, data);
                    break;
                case ".json":
                    File.WriteAllText(filePath, JsonSerializer.Serialize(data));
                    break;
                case ".xml":
                    var doc = new XmlDocument();
                    var root = doc.CreateElement("root");
                    foreach (var line in data)
                    {
                        var node = doc.CreateElement("line");
                        node.InnerText = line;
                        root.AppendChild(node);
                    }
                    doc.AppendChild(root);
                    doc.Save(filePath);
                    break;
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }
    }
}
