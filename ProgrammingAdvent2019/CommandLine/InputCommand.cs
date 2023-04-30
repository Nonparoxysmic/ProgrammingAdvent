// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.CommandLine
{
    internal class InputCommand : ICommand
    {
        public void Execute(string[] args)
        {
            Console.WriteLine();
            if (args.Length == 0)
            {
                Console.WriteLine("No parameters provided. Use \"... help\" for usage guide.");
                Console.WriteLine();
                return;
            }
            switch (args[0].ToLower())
            {
                case "help":
                case "?":
                case "/?":
                    PrintHelp();
                    break;
                case "update":
                case "/u":
                    InputManager.Update();
                    Console.WriteLine("Puzzle inputs updated.");
                    break;
                default:
                    Console.WriteLine("Unrecognized subcommand. Use \"... help\" for usage guide.");
                    break;
            }
            Console.WriteLine();
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Use \"... update\" to update puzzle inputs from file.");
        }
    }
}
