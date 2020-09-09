using System;
using System.Collections.Generic;


namespace ClassLibrary
{
    public abstract class Dialog
    {
        public string ConnectionString;
        public List<string> options{get; private set;} = new List<string>();
        private int currentOne = 0;
        private bool inside = true;
        public string ServerType;

        public Dialog(string connectionString, string serverType)
        {
            ConnectionString = connectionString;
            ServerType = serverType;
        }

        public int Control()
        {
            //Start with current option selected as 0, and draw the list of options
            currentOne = 0;
            inside = true;
            Draw();
            while (inside)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (currentOne > 0)
                            {
                                currentOne--;
                                Draw();
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (currentOne < options.Count - 1)
                            {
                                currentOne++;
                                Draw();
                            }
                            break;
                        case ConsoleKey.Enter:
                            inside = false;
                            break;
                        case ConsoleKey.Escape:
                            System.Environment.Exit(0);
                            break;
                    }

                }
            }
            return currentOne;
        }
        public void Add(string content)
        {
            //Add new option to the options list
            options.Add(content);
        }
        private void Draw()
        {
            Console.Clear();
            for (int i = 0; i < options.Count; i++)
            {
                //Draw the list of options, highlighting currently selected one as blue
                if (i == currentOne)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(options[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine(options[i]);
                }
            }
        }

        
    }
}
