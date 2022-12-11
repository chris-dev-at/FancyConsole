using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FancyConsole
{
    public static class FancyConsole
    {
        //make log to file 

        public static string Title = "";
        public static ConsoleColor ContentBackgroundColor = ConsoleColor.Black;
        public static ConsoleColor InputBackgroundColor = ConsoleColor.Black;
        public static ConsoleColor InputForegroundColor = ConsoleColor.Gray;

        public delegate void FancyChatInput(string text);
        public static event FancyChatInput OnFancyConsoleInput;

        public static string Input_Prefix = "> ";
        public static bool Active { get { return _Active; } }
        private static bool _Active = false;

        public static LinkedList<string> Lines = new LinkedList<string>();
        private static string current_line;


        public static void Activate()
        {
            _Active = true;
            OnFancyConsoleInput += Dummy;
            new Thread(() =>
            {
                int lastWindowHeight = Console.WindowHeight;
                int lastWindowWidth = Console.WindowWidth;
                while (Active)
                {
                    if (Console.WindowHeight < 3) //minimum of 3 Lines are needed
                        Console.WindowHeight = 3;
                    if (lastWindowHeight != Console.WindowHeight || lastWindowWidth != Console.WindowWidth)
                    {
                        lastWindowHeight = Console.WindowHeight;
                        lastWindowWidth = Console.WindowWidth;
                        Display();
                    }
                    Console.SetWindowPosition(0, 0);
                    Thread.Sleep(200);
                }
            }).Start();
            new Thread(() =>
            {
                while (true)
                {
                    OnFancyConsoleInput(ReadLine());
                }
            }).Start();
            Display();
        }
        public static void Format(int x, int y, int cursor_size)
        {
            Console.SetWindowSize(x, y);
            Console.CursorSize = 2;
        }

        public static void Display()
        {
            //presets
            Console.CursorVisible = false;

            DisplayTitle();

            DisplayContent();

            DisplayInput();

            Console.CursorVisible = true;
        }
        public static void DisplayTitle()
        {
            Console.SetCursorPosition(0, 0); //crashes when consoleheigh is 0
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            PadLine((Title.Length + Console.WindowWidth) / 2, ConsoleColor.Gray);
            Console.Write(Title);
            PadLine((Title.Length + Console.WindowWidth) / 2, ConsoleColor.Gray);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void DisplayContent()
        {
            if (Lines.Count == 0) return;

            int content_start = (Title == "") ? 0 : 1; //reserve Space if there is a Title
            Console.BackgroundColor = ContentBackgroundColor;

            LinkedListNode<string> lastLine = Lines.First;
            for (int i = 0; i < Console.WindowHeight - 3; i++)
            {
                if (lastLine.Next != null)
                    lastLine = lastLine.Next;
            }

            for (int i = content_start; i < Console.WindowHeight - 1; i++)
            {
                if (lastLine != null)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(lastLine.Value);
                    PadLine(lastLine.Value, ContentBackgroundColor);
                }
                else
                {
                    PadLine(0, ContentBackgroundColor);
                }
                if (lastLine != null)
                    lastLine = lastLine.Previous;
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void DisplayInput()
        {
            Console.ForegroundColor = InputForegroundColor;
            Console.BackgroundColor = InputBackgroundColor;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            PadLine(0, InputBackgroundColor);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(Input_Prefix);
        }

        private static void PadLine(string line_content, ConsoleColor color = ConsoleColor.Black)
        {
            PadLine(line_content.Length, color);
        }
        private static void PadLine(int charlength, ConsoleColor color = ConsoleColor.Black)
        {
            Console.BackgroundColor = color;
            string tmp = "";
            for (int i = 0; i < Console.WindowWidth - charlength; i++)
            {
                tmp += " ";
            }
            Console.Write(tmp);
            //Console.BackgroundColor = ConsoleColor.Black;
        }

        /*public static void Write(string text="")
        {
            //add splitting when \n contained
            current_line += text;
            Lines.First.Value = current_line;
        }*/

        public static void WriteLine(string text = "")
        {
            current_line += text;
            Lines.AddFirst(current_line);
            current_line = null;
        }
        private static string ReadLine()
        {
            string text = Console.ReadLine();
            Console.SetWindowPosition(0, 0);
            DisplayInput();
            return text;
        }
        private static void Dummy(string x) { } //used to prevent Exception when Empty FancyConsole.Read is used();
    }
}
