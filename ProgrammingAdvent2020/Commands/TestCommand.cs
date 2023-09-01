// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;
using ProgrammingAdvent2020.Solutions;

namespace ProgrammingAdvent2020.Commands;

internal class TestCommand : Command
{
    public override void Execute(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No parameters provided. Use the help command for a guide.");
            return;
        }
        if (args[0].ToLower() is "all" or "a" or @"/a")
        {
            TestAll();
            return;
        }
        if (int.TryParse(args[0], out int numArg0))
        {
            TestDay(numArg0);
            return;
        }
        Match rangeMatch = _numberRange.Match(args[0]);
        if (rangeMatch.Success)
        {
            TestRange(int.Parse(rangeMatch.Groups["first"].Value), int.Parse(rangeMatch.Groups["last"].Value));
            return;
        }
        if (args.Length == 1 || args[0] is not ("day" or "range"))
        {
            Console.WriteLine("Invalid parameters provided. Use the help command for a guide.");
            return;
        }
        if (int.TryParse(args[1], out int numArg1))
        {
            if (args[0] == "day")
            {
                TestDay(numArg1);
                return;
            }
            if (args.Length < 3 || !int.TryParse(args[2], out int numArg2))
            {
                Console.WriteLine("Invalid parameters provided. Use the help command for a guide.");
                return;
            }
            TestRange(numArg1, numArg2);
            return;
        }
        rangeMatch = _numberRange.Match(args[1]);
        if (rangeMatch.Success)
        {
            TestRange(int.Parse(rangeMatch.Groups["first"].Value), int.Parse(rangeMatch.Groups["last"].Value));
            return;
        }
        Console.WriteLine("Invalid parameters provided. Use the help command for a guide.");
    }

    public override void PrintHelp()
    {
        Console.WriteLine("Tests puzzle solutions.");
        Console.WriteLine("Usage: test day <dayNumber>");
        Console.WriteLine("Usage: test range <firstDay>-<lastDay>");
        Console.WriteLine("Usage: test all");
        Console.WriteLine("Aliases: test, t");
    }

    private static void TestDay(int dayNumber)
    {
        Console.WriteLine();
        if (!SolutionSource.TryGetSolution(dayNumber, out Day? solution))
        {
            Console.WriteLine($"Solution not available for Day {dayNumber}.");
            return;
        }
        if (!ExampleSource.TryGetExamples(dayNumber,
            out (string Input, string? PartOne, string? PartTwo, string? Modifier)[]? examples))
        {
            Console.WriteLine($"No examples available for Day {dayNumber}.");
            return;
        }
        for (int i = 0; i < examples.Length; i++)
        {
            string input = examples[i].Input;
            string? modifier = examples[i].Modifier;
            PuzzleAnswers calculated = solution.Solve(input.ToLines(), modifier);
            if (calculated.IsError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Day {dayNumber} Example {i + 1} ERROR: {calculated.ErrorMessage}");
                Console.ResetColor();
                continue;
            }
            if (examples[i].PartOne is not null)
            {
                if (calculated.PartOneAnswer == examples[i].PartOne)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Day {dayNumber} Example {i + 1} Part One: Pass");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Day {dayNumber} Example {i + 1} Part One: FAILED");
                    Console.ResetColor();
                    Console.WriteLine($"Should be \"{examples[i].PartOne}\", got \"{calculated.PartOneAnswer}\".");
                }
            }
            if (examples[i].PartTwo is not null)
            {
                if (calculated.PartTwoAnswer == examples[i].PartTwo)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Day {dayNumber} Example {i + 1} Part Two: Pass");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Day {dayNumber} Example {i + 1} Part Two: FAILED");
                    Console.ResetColor();
                    Console.WriteLine($"Should be \"{examples[i].PartTwo}\", got \"{calculated.PartTwoAnswer}\".");
                }
            }
        }
    }

    private static void TestRange(int first, int last)
    {
        if (last < first)
        {
            (first, last) = (last, first);
        }
        for (int i = first; i <= last; i++)
        {
            TestDay(i);
        }
    }

    private static void TestAll()
    {
        int[] dayNumbers = SolutionSource.AvailableSolutions();
        for (int i = 0; i < dayNumbers.Length; i++)
        {
            TestDay(dayNumbers[i]);
        }
    }
}
