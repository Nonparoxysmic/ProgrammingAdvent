// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

namespace ProgrammingAdvent2019.CommandLine
{
    internal class HelpCommand : ICommand
    {
        public void Execute(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Use \"{command} help\" for usage guide for each command.");
            Console.WriteLine("Available commands: solve, test, input, help.");
            Console.WriteLine("Type 'exit' to terminate the application.");
            Console.WriteLine();
        }
    }
}
