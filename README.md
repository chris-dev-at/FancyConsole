# FancyConsole
A fancier version of the C-Sharp Console <br>

<h2>Example</h2>

![image](https://user-images.githubusercontent.com/111374403/208204278-2bf392da-4e12-4b1a-ab28-84cbf18a4f41.png)


```csharp
void Main(){
FancyConsole.FancyConsole.OnFancyConsoleInput += Input; // This has to be added before Activate. If added after you need to execute input once before this method gets alerted!
            FancyConsole.Activate();
            FancyConsole.ContentBackgroundColor = ConsoleColor.Black;
            FancyConsole.InputBackgroundColor = ConsoleColor.Red;
            FancyConsole.InputForegroundColor = ConsoleColor.Black;
            FancyConsole.ShowOverflow = false;
            FancyConsole.Input_Prefix = "> ";
            FancyConsole.Title = "--[FancyConsole]--";
            FancyConsole.WriteLine("For multiple line outputs");
            FancyConsole.WriteLine("Use the WriteLine Command");
            FancyConsole.WriteLine("After you are finished");
            FancyConsole.WriteLine("Execute the Display Command");
            FancyConsole.WriteLine("Like this");
            FancyConsole.WriteLine("--TESTING--");
            FancyConsole.WriteLine("OVERFLOW TEST: igdfjbnafgsdijp oajsfdoja nfsdojnags fdagdf fgd dgf dfg dfg as rsojbojbsdf onjfds ojnüfa dsonjü fosdnajüa soedfjnü");


            //takes input and displays it
            void Input(string text)
            {
                if (text == "exit") Environment.Exit(0);
                FancyConsole.WriteLine(text);
                //FancyConsole.DisplayContent(); only needed if UpdateContentAfterWrite is false
            }
}
```

<h2>Functions</h2>
<code>WriteLine(string text);</code> => Displays text variable in the Console <br>
<code>Activate();</code> => Starts Displaying in the FanyConsole Format and starts the resize Listener <br>
<code>Deactivate();</code> => Stops the resize Listener and deactivates FancyConsole <br>

<h2>Properties</h2>
<code>Title</code> => If not empty, the title gets displayed at the Top (string, Default: "") <br>
<code>ShowOverflow</code> => Decides if Overflow is shown (bool, Default: true) <br>
<code>UpdateContentAfterWrite</code> => Automatically updates the Interface after WriteLine() (bool, Default: true) <br>
<code>Input_Prefix</code> => Input Prefix is shown in front of the Input-Line (string, Default: "") <br>
<code>TitleBackgroundColor</code> => Background Color of the Title (if there is one) (ConsoleColor, Default: Gray) <br>
<code>TitleForegroundColor</code> => Foreground Color of the Title (if there is one) (ConsoleColor, Default: Black) <br>
<code>ContentBackgroundColor</code> => Background Color of the Content (ConsoleColor, Default: Black) <br>
<code>InputBackgroundColor</code> => Background Color of the Input (ConsoleColor, Default: Black) <br>
<code>InputForegroundColor</code> => Background Color of the Input (ConsoleColor, Default: Gray) <br>

<h2>Events</h2>
> <code>delegate void FancyChatInput</code> => FancyChatInput(string text); <br><br>
<code>OnFancyConsoleInput</code> => Gets Executed when there is an input in the Input-Line<br>

