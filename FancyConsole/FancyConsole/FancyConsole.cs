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
        public static bool ShowOverflow = true;
        public static bool UpdateContentAfterWrite = true;
        public static string Input_Prefix = "";
        public static ConsoleColor TitleBackgroundColor = ConsoleColor.Gray;
        public static ConsoleColor TitleForegroundColor = ConsoleColor.Black;
        public static ConsoleColor ContentBackgroundColor = ConsoleColor.Black;
        public static ConsoleColor ContentForegroundColor = ConsoleColor.Gray;
        public static ConsoleColor InputBackgroundColor = ConsoleColor.Black;
        public static ConsoleColor InputForegroundColor = ConsoleColor.Gray;

        public delegate void FancyChatInput(string text);
        public static event FancyChatInput OnFancyConsoleInput;

        /// <summary>
        /// When changed automatically Updates Title
        /// </summary>
        public static string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                DisplayTitle();
            }
        }
        private static string _Title = "";

        public static bool Active { get { return _Active; } }
        private static bool _Active = false;

        public static LinkedList<string> Lines = new LinkedList<string>();
        private static string current_line;


        /// <summary>
        /// Activate FancyConsole
        /// </summary>
        public static void Activate()
        {
            if (_Active) return;
            _Active = true; //Stores Activated state
            OnFancyConsoleInput += Dummy; //make sure event is not empty
            new Thread(() => //check for size change and adjust
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
                    try { Console.SetWindowPosition(0, 0);  } catch (Exception) { } //Crashes when Window is rezised aggressivly up and down
                    Thread.Sleep(1);
                }
            }).Start();
            new Thread(() => //constant ReadLine Loop
            {
                while (true)
                {
                    OnFancyConsoleInput(ReadLine());
                }
            }).Start();
            Display(); //initial draw
        }
        /// <summary>
        /// Deactivate FancyConsole
        /// </summary>
        public static void Deactivate()
        {
            _Active = false;
        }

        public static void Format(int x, int y, int cursor_size)
        {
            Console.SetWindowSize(x, y);
            Console.CursorSize = 2;
        }

        /// <summary>
        /// Updates the entire Frame
        /// </summary>
        public static void Display()
        {
            //presets
            Console.CursorVisible = false;

            DisplayTitle();

            DisplayContent();

            DisplayInput();

            Console.CursorVisible = true;
        }

        /// <summary>
        /// Updates the TitleFrame
        /// </summary>
        public static void DisplayTitle()
        {
            Console.SetCursorPosition(0, 0); //crashes when consoleheigh is 0
            Console.BackgroundColor = TitleBackgroundColor;
            Console.ForegroundColor = TitleForegroundColor;
            PadLine((_Title.Length + Console.WindowWidth) / 2, ConsoleColor.Gray);
            Console.Write(_Title);
            PadLine((_Title.Length + Console.WindowWidth) / 2, ConsoleColor.Gray);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Updates the ContentFrame
        /// </summary>
        public static void DisplayContent()
        {
            if (Lines.Count == 0) return;

            int content_start = (_Title == "") ? 0 : 1; //reserve Space if there is a Title
            Console.BackgroundColor = ContentBackgroundColor;
            Console.ForegroundColor = ContentForegroundColor;

            LinkedListNode<string> lastLine = Lines.First;
            for (int i = 0; i < Console.WindowHeight - 2 - content_start; i++) //-1 for update / -1 for Input / -1 for Title(if exists)
            {
                if (lastLine.Next != null)
                    lastLine = lastLine.Next;
            }
            string output = "";
            Console.SetCursorPosition(0, content_start);
            for (int i = content_start; i < Console.WindowHeight - 1; i++)
            {
                //Yes its ugly. also its 4:46am so leave me alone man :(
                if(lastLine != null)
                {
                    if(lastLine.Value.Length <= Console.WindowWidth || ShowOverflow) //normal output with Overflow
                    {
                        output += lastLine.Value;
                        output += PadRight("", (Console.WindowWidth - lastLine.Value.Length), ' ');
                        if(lastLine.Value.Length > Console.WindowWidth) //Remove Red filling when Text automaticly overflows
                            output += PadRight("", Console.WindowWidth-(lastLine.Value.Length%Console.WindowWidth), ' ');
                        output += "\n";
                    }

                    if (lastLine.Value.Length > Console.WindowWidth && !ShowOverflow) //output without Overflow
                        output += lastLine.Value.Substring(0, Console.WindowWidth - 3) + " >>\n"; //-3 for Marking
                    
                    lastLine = lastLine.Previous;
                }
                else
                {
                    output += PadRight("", Console.WindowWidth, ' ') + "\n";
                }
            }
            Console.Write(output);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            DisplayInput(); //needs redraw to setup input again
        }

        /// <summary>
        /// Updates the InputFrame
        /// </summary>
        public static void DisplayInput()
        {
            Console.ForegroundColor = InputForegroundColor;
            Console.BackgroundColor = InputBackgroundColor;
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            PadLine(0, InputBackgroundColor);
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(Input_Prefix);
        }

        /// <summary>
        /// Extension of PadLine(Int32, ConsoleColor) used for padding after finishing writing a string
        /// </summary>
        /// <param name="line_content">Written Content</param>
        /// <param name="color">The Background Color</param>
        private static void PadLine(string line_content, ConsoleColor color = ConsoleColor.Black)
        {
            PadLine(line_content.Length, color);
        }
        /// <summary>
        /// Creates Spaces " " with a given Color
        /// </summary>
        /// <param name="charlength">The Length of Spaces</param>
        /// <param name="color">The Background Color</param>
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
        private static string PadRight(string text, int num, char c) //prevent crashing when num is negative
        {
            if (num < 0) return text;
            return text.PadRight(num, ' ');
        }

            /*public static void Write(string text="") TODO
            {
                //add splitting when \n contained

            }*/

            /// <summary>
            /// Writes a new Line of Text in the ContentWindow. Requires a redraw to Display (FancyConsole.Display())
            /// </summary>
            /// <param name="text"></param>
        public static void WriteLine(string text = "")
        {
            current_line += text;
            Lines.AddFirst(current_line);
            current_line = null;
            if (UpdateContentAfterWrite) DisplayContent();
        }

        /// <summary>
        /// Requesting Input in Input Section
        /// </summary>
        /// <returns>The Input</returns>
        private static string ReadLine() //TODO: replace with ReadKey
        {
            string text = Console.ReadLine();
            Console.SetWindowPosition(0, 0);
            DisplayInput();
            return text;
        }
        private static void Dummy(string x) { } //used to prevent Exception when Empty FancyConsole.Read is used();
    }
}
