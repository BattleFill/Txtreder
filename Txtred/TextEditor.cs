using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Txtred
{
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
}
