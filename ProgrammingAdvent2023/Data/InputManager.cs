// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

using System.Diagnostics.CodeAnalysis;

namespace ProgrammingAdvent2023.Data;

internal static class InputManager
{
    private static readonly SortedDictionary<int, string[]> _inputs = [];

    public static void Update()
    {
        _inputs.Clear();
        for (int dayNumber = 1; dayNumber <= 25; dayNumber++)
        {
            if (SystemIO.TryReadInputFile(dayNumber, out string[] input))
            {
                _inputs[dayNumber] = input;
            }
        }
    }

    public static bool TryGetInput(int dayNumber, [NotNullWhen(true)] out string[]? input)
    {
        input = null;
        if (dayNumber < 1 || dayNumber > 25)
        {
            return false;
        }
        if (_inputs.TryGetValue(dayNumber, out string[]? value))
        {
            input = value;
            return true;
        }
        return false;
    }
}
