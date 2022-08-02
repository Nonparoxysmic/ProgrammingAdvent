// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProgrammingAdvent2018.Program
{
    internal static class Reflection
    {
        private static SortedDictionary<int, Type> dayTypes;

        private static Type[] GetDayClassTypes()
        {
            Type t = typeof(Day);
            return Assembly.GetAssembly(t).GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(t)).ToArray();
        }

        internal static void InitializeDictionary()
        {
            dayTypes = new SortedDictionary<int, Type>();
            Type[] types = GetDayClassTypes();
            foreach (Type t in types)
            {
                if (!Day.TryParseDayNumber(t.Name, out int dayNumber))
                {
                    continue;
                }
                if (dayTypes.ContainsKey(dayNumber))
                {
                    throw new ApplicationException();
                }
                else
                {
                    dayTypes.Add(dayNumber, t);
                }
            }
        }

        internal static int[] GetDayNumbers()
        {
            if (dayTypes == null) { InitializeDictionary(); }
            List<int> numbers = new List<int>();
            foreach (KeyValuePair<int, Type> kvp in dayTypes)
            {
                numbers.Add(kvp.Key);
            }
            return numbers.ToArray();
        }

        internal static Day CreateDayObject(int dayNumber)
        {
            if (dayTypes == null) { InitializeDictionary(); }
            return dayTypes.ContainsKey(dayNumber)
                ? (Day)Activator.CreateInstance(dayTypes[dayNumber])
                : null;
        }
    }
}
