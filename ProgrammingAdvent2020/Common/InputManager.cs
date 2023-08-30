// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Diagnostics.CodeAnalysis;
using ProgrammingAdvent2020.Solutions;

namespace ProgrammingAdvent2020.Common;

internal static class InputManager
{
    private static readonly SortedDictionary<int, string[]> _inputs = new();
    private static readonly SortedDictionary<int, string> _errors = new();

    public static void Update()
    {
        _inputs.Clear();
        _errors.Clear();
        for (int dayNumber = 1; dayNumber <= 25; dayNumber++)
        {
            if (!SystemIO.TryReadInputFile(dayNumber, out string[] input))
            {
                _errors[dayNumber] = "Unable to read input file.";
                continue;
            }
            if (!SolutionSource.TryGetSolution(dayNumber, out Day? solution))
            {
                _errors[dayNumber] = "Solution not available.";
                continue;
            }
            if (!solution.ValidateInput(input, out string errorMessage))
            {
                _errors[dayNumber] = $"Input in file is invalid. {errorMessage}";
                continue;
            }
            _inputs[dayNumber] = input;
        }
    }

    public static bool TryGetInput(int dayNumber, [NotNullWhen(true)] out string[]? input, out string errorMessage)
    {
        input = null;
        if (dayNumber < 1 || dayNumber > 25)
        {
            errorMessage = $"{dayNumber} is not a valid day in the calendar.";
            return false;
        }
        if (_inputs.ContainsKey(dayNumber))
        {
            input = _inputs[dayNumber];
            errorMessage = string.Empty;
            return true;
        }
        errorMessage = _errors.TryGetValue(dayNumber, out string? error) ? error : "Unknown error.";
        return false;
    }
}
