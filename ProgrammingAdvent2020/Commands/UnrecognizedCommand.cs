// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

namespace ProgrammingAdvent2020.Commands;

internal class UnrecognizedCommand : Command
{
    public override void Execute(string[] args)
    {
        Console.WriteLine("Unrecognized command. Type \"help\" for a list of commands.");
    }

    public override void PrintHelp()
    {
        Console.WriteLine("Unrecognized command. Type \"help\" for a list of commands.");
    }
}
