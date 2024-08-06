// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.Json;
using ProgrammingAdvent2021.Solutions;
using ProgrammingAdvent2021.Utilities;

namespace ProgrammingAdvent2021.Common;

internal static class Examples
{
    private static readonly SortedDictionary<int, (string, string?, string?)[]> _examples = [];

    public static void Initialize()
    {
        _examples.Clear();
        if (!SystemIO.TryReadExampleFile(out JsonDocument? jsonDocument))
        {
            return;
        }
        for (int dayNumber = 1; dayNumber <= 25; dayNumber++)
        {
            if (jsonDocument.RootElement.TryGetProperty($"Day{dayNumber:00}", out JsonElement dayProperty))
            {
                List<(string, string?, string?)> dayExamples = [];
                for (int i = 1; i < 100; i++)
                {
                    if (!dayProperty.TryGetProperty($"Example{i:00}", out JsonElement exampleProperty))
                    {
                        break;
                    }
                    if (!exampleProperty.TryGetProperty("Input", out JsonElement inputProperty)
                        || inputProperty.GetString() is null)
                    {
                        break;
                    }
                    string input = inputProperty.GetString() ?? string.Empty;
                    string? partOne = null, partTwo = null;
                    if (exampleProperty.TryGetProperty("PartOne", out JsonElement partOneProperty))
                    {
                        partOne = partOneProperty.GetString();
                    }
                    if (exampleProperty.TryGetProperty("PartTwo", out JsonElement partTwoProperty))
                    {
                        partTwo = partTwoProperty.GetString();
                    }
                    if (partOne is not null || partTwo is not null)
                    {
                        dayExamples.Add((input, partOne, partTwo));
                    }
                    else
                    {
                        break;
                    }
                }
                if (dayExamples.Count > 0)
                {
                    _examples.Add(dayNumber, [.. dayExamples]);
                }
            }
        }
        jsonDocument.Dispose();
    }

    public static void TestExamples()
    {
        for (int dayNumber = 1; dayNumber <= 25; dayNumber++)
        {
            if (!Reflection.DayTypes.ContainsKey(dayNumber))
            {
                Console.WriteLine($"Day {dayNumber:00}: No solution implemented.");
                continue;
            }
            if (_examples.TryGetValue(dayNumber, out (string, string?, string?)[]? dayExamples)
                && dayExamples.Length > 0)
            {
                if (Day.TryGetSolution(dayNumber, out Day? solution))
                {
                    foreach ((string input, string? partOne, string? partTwo) in dayExamples)
                    {
                        (string, string) result = solution.Test(input.ToLines());
                        if (partOne is not null)
                        {
                            if (result.Item1 == partOne)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Day {dayNumber:00} Part One: Success");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Day {dayNumber:00} Part One: FAILURE");
                                Console.WriteLine($"Expected \"{partOne}\", got \"{result.Item1}\"");
                                Console.ResetColor();
                            }
                        }
                        if (partTwo is not null)
                        {
                            if (result.Item2 == partTwo)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Day {dayNumber:00} Part Two: Success");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Day {dayNumber:00} Part Two: FAILURE");
                                Console.WriteLine($"Expected \"{partTwo}\", got \"{result.Item2}\"");
                                Console.ResetColor();
                            }
                        }
                        if (partOne is null && partTwo is null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Day {dayNumber:00}: Example contained no outputs.");
                            Console.ResetColor();
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Day {dayNumber:00}: Unknown error.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine($"Day {dayNumber:00}: No examples.");
            }
        }
    }
}
