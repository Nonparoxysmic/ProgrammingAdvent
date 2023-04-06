// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Reflection;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Solutions;

namespace ProgrammingAdvent2019.Common;

internal static class Reflection
{
    public static readonly SortedDictionary<int, Type> DayTypes = InitializeDayTypes();

    private static SortedDictionary<int, Type> InitializeDayTypes()
    {
        SortedDictionary<int, Type> dictionary = new();
        Type[] dayTypes = GetDayTypes();
        Regex validDayName = new("^Day(?<DayNumber>0[1-9]|1[0-9]|2[0-5])$");
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
        Type day = typeof(Day);
        Assembly? assembly = Assembly.GetAssembly(day);
        if (assembly is null)
        {
            return Array.Empty<Type>();
        }
        return assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(day)).ToArray();
    }
}
