using System;
using System.IO;
using System.Text.Json;
using System.Xml;

namespace ConsoleTextEditor
{
    class Figure
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

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

    class TextEditor
    {
        private string[] data;
        private int cursorPosition;

        public TextEditor(string[] textData)
        {
            data = textData;
            cursorPosition = 0;
        }

        public void EditText()
        {
            Console.CursorVisible = false;
            Console.Clear();

            PrintText();

            while (true)
            {
                Console.SetCursorPosition(0, cursorPosition);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        MoveCursorUp();
                        break;
                    case ConsoleKey.DownArrow:
                        MoveCursorDown();
                        break;
                    case ConsoleKey.Enter:
                        EditLine();
                        PrintText();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private void PrintText()
        {
            Console.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
        }

        private void MoveCursorUp()
        {
            if (cursorPosition > 0)
            {
                cursorPosition--;
            }
        }

        private void MoveCursorDown()
        {
            if (cursorPosition < data.Length - 1)
            {
                cursorPosition++;
            }
        }

        private void EditLine()
        {
            Console.SetCursorPosition(0, cursorPosition);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, cursorPosition);
            data[cursorPosition] = Console.ReadLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the file path: ");
            string filePath = Console.ReadLine();

            FileManager fileManager = new FileManager(filePath);
            string[] data = fileManager.LoadFile();

            TextEditor textEditor = new TextEditor(data);
            textEditor.EditText();

            Console.WriteLine("Do you want to save changes? (Y/N)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                fileManager.SaveFile(data);
                Console.WriteLine("File saved successfully.");
            }
            else
            {
                Console.WriteLine("Changes not saved.");
            }
        }
    }
}