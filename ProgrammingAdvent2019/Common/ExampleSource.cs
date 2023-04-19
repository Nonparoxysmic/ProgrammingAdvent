// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.Json;

namespace ProgrammingAdvent2019.Common;

internal static class ExampleSource
{
    private static readonly SortedDictionary<int, (string, string?, string?)[]> _examples = new();

    public static void Initialize()
    {
        _examples.Clear();
        if (!SystemIO.TryReadExampleFile(out JsonDocument? jsonDocument, out string errorMessage)
            || jsonDocument is null)
        {
            Console.WriteLine($"Examples not available. {errorMessage}");
            return;
        }
        for (int dayNumber = 1; dayNumber <= 25; dayNumber++)
        {
            if (jsonDocument.RootElement.TryGetProperty($"Day{dayNumber:00}", out JsonElement dayProperty))
            {
                List<(string, string?, string?)> dayExamples = new();
                for (int i = 1; i < 100; i++)
                {
                    if (!dayProperty.TryGetProperty($"Example{i:00}", out JsonElement exampleProperty))
                    {
                        break;
                    }
                    if (!exampleProperty.TryGetProperty("Input", out JsonElement inputProperty)
                        || inputProperty.GetString() == null)
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
                    _examples.Add(dayNumber, dayExamples.ToArray());
                }
            }
        }
        jsonDocument.Dispose();
    }

    public static bool TryGetExamples(int dayNumber, out (string, string?, string?)[] examples)
    {
        if (_examples.ContainsKey(dayNumber))
        {
            examples = _examples[dayNumber];
            return true;
        }
        examples = Array.Empty<(string, string?, string?)>();
        return false;
    }

    public static int[] AvailableExampleDays()
    {
        List<int> output = new();
        foreach (var kvp in _examples)
        {
            output.Add(kvp.Key);
        }
        return output.ToArray();
    }
}
