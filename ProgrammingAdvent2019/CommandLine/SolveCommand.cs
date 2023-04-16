// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;
using ProgrammingAdvent2019.Solutions;

namespace ProgrammingAdvent2019.CommandLine;

internal class SolveCommand : ICommand
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
                SolveDay(dayNumber);
                break;
            case "all":
            case "/a":
                SolveAll();
                break;
            default:
                Console.WriteLine("Unrecognized subcommand. Use \"... help\" for usage guide.");
                break;
        }
        Console.WriteLine();
    }

    public static void PrintHelp()
    {
        Console.WriteLine("Use \"... day {dayNumber}\" to solve one day.");
        Console.WriteLine("Use \"... all\" to solve all available days.");
    }

    public static void SolveDay(int dayNumber)
    {
        if (!SolutionSource.TryGetSolution(dayNumber, out Day? solution) || solution == null)
        {
            Console.WriteLine($"Solution not available for Day {dayNumber}.");
            return;
        }
        if (!SystemIO.TryReadInputFile(dayNumber, out string[] input))
        {
            Console.WriteLine($"Unable to read input file InputDay{dayNumber:00}.txt.");
            return;
        }
        if (!solution.ValidateInput(input, out string errorMessage))
        {
            Console.WriteLine($"Input error: {errorMessage}");
            return;
        }
        PuzzleAnswers answers = solution.Solve(input);
        Console.WriteLine(answers);
    }

    public static void SolveAll()
    {
        int[] dayNumbers = SolutionSource.AvailableSolutions();
        for (int i = 0; i < dayNumbers.Length; i++)
        {
            SolveDay(dayNumbers[i]);
            if (i < dayNumbers.Length - 1)
            {
                Console.WriteLine();
            }
        }
    }
}
