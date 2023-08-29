// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

namespace ProgrammingAdvent2020.Commands;

internal class ClearCommand : Command
{
    public override void Execute(string[] args)
    {
        Console.Clear();
    }

    public override void PrintHelp()
    {
        Console.WriteLine("Clears the console.");
        Console.WriteLine("Aliases: clear, c");
    }
}
