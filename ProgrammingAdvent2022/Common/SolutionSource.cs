// ProgrammingAdvent2022 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Diagnostics.CodeAnalysis;
using ProgrammingAdvent2022.Solutions;

namespace ProgrammingAdvent2022.Common;

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
