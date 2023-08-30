// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;
using ProgrammingAdvent2020.Solutions;

namespace ProgrammingAdvent2020.Commands;

internal class SolveCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No parameters provided. Use the help command for a guide.");
            return;
        }
        if (int.TryParse(args[0], out int numArg))
        {
            SolveDay(numArg);
            return;
        }
        Match rangeMatch = _numberRange.Match(args[0]);
        if (rangeMatch.Success)
        {
            SolveRange(int.Parse(rangeMatch.Groups["first"].Value), int.Parse(rangeMatch.Groups["last"].Value));
        }
        // TODO: Implement more options
    }

    public override void PrintHelp()
    {
        Console.WriteLine("Solves puzzles.");
        Console.WriteLine("Aliases: solve, s");
    }

    private static void SolveDay(int dayNumber)
    {
        Console.WriteLine();
        if (!SolutionSource.TryGetSolution(dayNumber, out Day? solution))
        {
            Console.WriteLine($"Solution not available for Day {dayNumber}.");
            return;
        }
        if (!InputManager.TryGetInput(dayNumber, out string[]? input, out string errorMessage))
        {
            Console.WriteLine($"Input not available for Day {dayNumber}.");
            Console.WriteLine(errorMessage);
            return;
        }
        PuzzleAnswers answers = solution.Solve(input);
        Console.WriteLine(answers);
    }

    private static void SolveRange(int first, int last)
    {
        if (last < first)
        {
            (first, last) = (last, first);
        }
        for (int i = first; i <= last; i++)
        {
            SolveDay(i);
        }
    }
}
