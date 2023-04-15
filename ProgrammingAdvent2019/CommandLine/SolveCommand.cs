// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

namespace ProgrammingAdvent2019.CommandLine;

internal class SolveCommand : ICommand
{
    public void Execute(string[] args)
    {
        // TODO: Implement this command.
        if (args.Length == 0)
        {
            Console.WriteLine("No parameters.");
        }
        else
        {
            Console.WriteLine("Parameters: " + string.Join(", ", args));
        }
    }
}
