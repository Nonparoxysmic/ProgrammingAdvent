// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

namespace ProgrammingAdvent2020.Commands;

internal class HelpCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length > 0)
        {
            Command command = ParseCommand(args[0]);
            if (command != Unrecognized)
            {
                command.PrintHelp();
                return;
            }
        }
        Console.WriteLine("Available commands: clear, help, input, solve, test");
        Console.WriteLine("Type \"help <commandName>\" for information about a command.");
    }

    public override void PrintHelp()
    {
        Console.WriteLine("Provides information about commands.");
        Console.WriteLine("Usage: help <commandName>");
        Console.WriteLine("Aliases: help, h, ?");
    }
}
