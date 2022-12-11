using FancyConsole;

namespace DummyProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FancyConsole.FancyConsole.OnFancyConsoleInput += Input; // This has to be added before Activate. If added after you need to enter once before this method gets alerted!
            FancyConsole.FancyConsole.Activate();
            FancyConsole.FancyConsole.ContentBackgroundColor = ConsoleColor.Black;
            FancyConsole.FancyConsole.InputBackgroundColor = ConsoleColor.Red;
            FancyConsole.FancyConsole.InputForegroundColor = ConsoleColor.Black;
            FancyConsole.FancyConsole.ShowOverflow = false;
            FancyConsole.FancyConsole.Title = "--[FancyConsole]--";
            FancyConsole.FancyConsole.WriteLine("For multiple line outputs");
            FancyConsole.FancyConsole.WriteLine("Use the WriteLine Command");
            FancyConsole.FancyConsole.WriteLine("After you are finished");
            FancyConsole.FancyConsole.WriteLine("Execute the Display Command");
            FancyConsole.FancyConsole.WriteLine("Like this");
            FancyConsole.FancyConsole.WriteLine("--TESTING--");
            FancyConsole.FancyConsole.WriteLine("OVERFLOW TEST: igdfjbnafgsdijp oajsfdoja nfsdojnags fdagdf fgd dgf dfg dfg as rsojbojbsdf onjfds ojnüfa dsonjü fosdnajüa soedfjnü");

            FancyConsole.FancyConsole.Display();
            while (true)
            {
                Thread.Sleep(10000);
            }
            void Input(string text)
            {
                if (text == "exit") Environment.Exit(0);
                FancyConsole.FancyConsole.WriteLine(text);
                FancyConsole.FancyConsole.Display();
            }
        }
    }
}