// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Diagnostics.CodeAnalysis;
using ProgrammingAdvent2020.Solutions;

namespace ProgrammingAdvent2020.Common;

internal static class SolutionSource
{
    private static readonly SortedDictionary<int, Day> _solutions = InitializeDictionary();

    private static SortedDictionary<int, Day> InitializeDictionary()
    {
        SortedDictionary<int, Day> output = new();
        foreach (KeyValuePair<int, Type> kvp in Reflection.DayTypes)
        {
            Day? day = (Day?)Activator.CreateInstance(kvp.Value);
            if (day is null) { continue; }
            output.Add(kvp.Key, day);
        }
        return output;
    }

    public static bool TryGetSolution(int dayNumber, [NotNullWhen(true)] out Day? solution)
    {
        if (_solutions.ContainsKey(dayNumber))
        {
            solution = _solutions[dayNumber];
            return true;
        }
        solution = null;
        return false;
    }

    public static int[] AvailableSolutions()
    {
        List<int> output = new();
        foreach (var kvp in _solutions)
        {
            output.Add(kvp.Key);
        }
        return output.ToArray();
    }
}
