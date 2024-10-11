// ProgrammingAdvent2022 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ProgrammingAdvent2022.Common;

internal static class ExampleSource
{
    private static readonly SortedDictionary<int, (string, string?, string?, string?)[]> _examples = [];

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
                List<(string, string?, string?, string?)> dayExamples = [];
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
                    string? partOne = null, partTwo = null, modifier = null;
                    if (exampleProperty.TryGetProperty("PartOne", out JsonElement partOneProperty))
                    {
                        partOne = partOneProperty.GetString();
                    }
                    if (exampleProperty.TryGetProperty("PartTwo", out JsonElement partTwoProperty))
                    {
                        partTwo = partTwoProperty.GetString();
                    }
                    if (exampleProperty.TryGetProperty("ExampleModifier", out JsonElement modifierProperty))
                    {
                        modifier = modifierProperty.GetString();
                    }
                    if (partOne is not null || partTwo is not null)
                    {
                        dayExamples.Add((input, partOne, partTwo, modifier));
                    }
                    else
                    {
                        break;
                    }
                }
                _examples.Add(dayNumber, [.. dayExamples]);
            }
        }
        jsonDocument.Dispose();
    }

    public static bool TryGetExamples(int dayNumber,
        [NotNullWhen(true)] out (string, string?, string?, string?)[]? examples)
    {
        if (_examples.TryGetValue(dayNumber, out (string, string?, string?, string?)[]? value))
        {
            examples = value;
            return true;
        }
        examples = null;
        return false;
    }
}
