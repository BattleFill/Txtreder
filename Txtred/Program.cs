using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using Txtred;

namespace ConsoleTextEditor
{
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