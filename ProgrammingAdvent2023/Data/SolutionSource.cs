// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

using System.Diagnostics.CodeAnalysis;
using ProgrammingAdvent2023.Common;
using ProgrammingAdvent2023.Solutions;

namespace ProgrammingAdvent2023.Data;

internal static class SolutionSource
{
    private static readonly SortedDictionary<int, Day> _solutions = InitializeDictionary();

    private static SortedDictionary<int, Day> InitializeDictionary()
    {
        SortedDictionary<int, Day> output = [];
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
        if (_solutions.TryGetValue(dayNumber, out Day? value))
        {
            solution = value;
            return true;
        }
        solution = null;
        return false;
    }
}
