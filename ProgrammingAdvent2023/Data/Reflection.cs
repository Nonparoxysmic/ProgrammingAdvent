﻿// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

using System.Reflection;
using System.Text.RegularExpressions;
using ProgrammingAdvent2023.Solutions;

namespace ProgrammingAdvent2023.Common;

internal static partial class Reflection
{
    public static readonly SortedDictionary<int, Type> DayTypes = InitializeDayTypes();

    public static readonly Regex ValidDayName = ValidDayNameRegex();

    [GeneratedRegex("^Day(?<DayNumber>0[1-9]|1[0-9]|2[0-5])$")]
    private static partial Regex ValidDayNameRegex();

    private static SortedDictionary<int, Type> InitializeDayTypes()
    {
        SortedDictionary<int, Type> dictionary = [];
        Type[] dayTypes = GetDayTypes();
        foreach (Type type in dayTypes)
        {
            Match match = ValidDayName.Match(type.Name);
            if (match.Success)
            {
                dictionary[int.Parse(match.Groups["DayNumber"].Value)] = type;
            }
        }
        return dictionary;
    }

    private static Type[] GetDayTypes()
    {
        Assembly? assembly = Assembly.GetAssembly(typeof(Day));
        if (assembly is null)
        {
            return [];
        }
        return assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Day))).ToArray();
    }
}
