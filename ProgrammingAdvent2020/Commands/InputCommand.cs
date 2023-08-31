// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Commands;

internal class InputCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No parameters provided. Use the help command for a guide.");
            return;
        }
        if (args[0] is "update" or "u" or @"/u")
        {
            InputManager.Update();
            Console.WriteLine("Puzzle inputs updated.");
            return;
        }
        Console.WriteLine("Invalid parameters provided. Use the help command for a guide.");
    }

    public override void PrintHelp()
    {
        Console.WriteLine("Reads inputs from InputFiles folder.");
        Console.WriteLine("Usage: input (update|u)");
        Console.WriteLine("Aliases: input, i");
    }
}
