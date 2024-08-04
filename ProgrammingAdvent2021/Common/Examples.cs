// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.Json;

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
        // TODO: Test examples
        Console.WriteLine("Example testing not yet implemented.");
    }
}
