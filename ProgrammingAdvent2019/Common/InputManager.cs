// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Solutions;

namespace ProgrammingAdvent2019.Common;

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
            if (!SolutionSource.TryGetSolution(dayNumber, out Day? solution) || solution == null)
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

    public static bool TryGetInput(int dayNumber, out string[] input, out string errorMessage)
    {
        if (dayNumber < 1 || dayNumber > 25)
        {
            input = Array.Empty<string>();
            errorMessage = $"{dayNumber} is not a valid day in the calendar.";
            return false;
        }
        if (_inputs.ContainsKey(dayNumber))
        {
            input = _inputs[dayNumber];
            errorMessage = string.Empty;
            return true;
        }
        else if (_errors.ContainsKey(dayNumber))
        {
            errorMessage = _errors[dayNumber];
        }
        else
        {
            errorMessage = "Unknown error.";
        }
        input = Array.Empty<string>();
        return false;
    }

    public static int[] AvailableInputs()
    {
        List<int> output = new();
        foreach (var kvp in _inputs)
        {
            output.Add(kvp.Key);
        }
        return output.ToArray();
    }
}
