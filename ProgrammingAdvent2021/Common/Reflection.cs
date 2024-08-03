// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Reflection;
using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Solutions;

namespace ProgrammingAdvent2021.Common;

internal static class Reflection
{
    public static readonly SortedDictionary<int, Type> DayTypes = InitializeDayTypes();

    private static SortedDictionary<int, Type> InitializeDayTypes()
    {
        SortedDictionary<int, Type> dictionary = [];
        Type[] dayTypes = GetDayTypes();
        Regex validDayName = GeneratedRegex.ValidDayName;
        foreach (Type type in dayTypes)
        {
            Match match = validDayName.Match(type.Name);
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
