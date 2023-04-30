// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;
using ProgrammingAdvent2019.Solutions;

namespace ProgrammingAdvent2019.CommandLine;

internal class TestCommand : ICommand
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
            case "day":
            case "/d":
                if (args.Length < 2 || !int.TryParse(args[1], out int dayNumber)
                    || dayNumber < 1 || dayNumber > 25)
                {
                    Console.WriteLine("No valid day number specified. Use \"... help\" for usage guide.");
                    break;
                }
                TestDay(dayNumber);
                break;
            case "all":
            case "/a":
                TestAll();
                break;
            default:
                Console.WriteLine("Unrecognized subcommand. Use \"... help\" for usage guide.");
                break;
        }
        Console.WriteLine();
    }

    public static void PrintHelp()
    {
        Console.WriteLine("Use \"... day {dayNumber}\" to test one day.");
        Console.WriteLine("Use \"... all\" to test all available days.");
    }

    public static void TestDay(int dayNumber)
    {
        if (!SolutionSource.TryGetSolution(dayNumber, out Day? solution) || solution is null)
        {
            Console.WriteLine($"Solution not available for Day {dayNumber}.");
            return;
        }
        if (!ExampleSource.TryGetExamples(dayNumber,
            out (string Input, string? PartOne, string? PartTwo)[] examples)
            || examples.Length == 0)
        {
            Console.WriteLine($"No examples available for Day {dayNumber}.");
            return;
        }
        for (int i = 0; i < examples.Length; i++)
        {
            PuzzleAnswers calculated = solution.Solve(examples[i].Input.ToLines());
            if (calculated.IsError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Day {dayNumber} Example {i + 1} {calculated.ErrorMessage}");
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

    public static void TestAll()
    {
        int[] dayNumbers = SolutionSource.AvailableSolutions();
        for (int i = 0; i < dayNumbers.Length; i++)
        {
            TestDay(dayNumbers[i]);
            if (i < dayNumbers.Length - 1)
            {
                Console.WriteLine();
            }
        }
    }
}
