// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2017.Program
{
    internal static class Reflection
    {
        private static SortedDictionary<int, Type> dayTypes;

        private static Type[] GetDayClassTypes()
        {
            Type t = typeof(Day);
            return Assembly.GetAssembly(t).GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(t)).ToArray();
        }

        public static void InitializeDictionary()
        {
            dayTypes = new SortedDictionary<int, Type>();
            Type[] types = GetDayClassTypes();
            foreach (Type t in types)
            {
                int dayNumber = int.Parse(Regex.Match(t.Name, @"\d+$").Value);
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

        public static int[] GetDayNumbers()
        {
            if (dayTypes == null) { InitializeDictionary(); }
            List<int> numbers = new List<int>();
            foreach (KeyValuePair<int, Type> kvp in dayTypes)
            {
                numbers.Add(kvp.Key);
            }
            return numbers.ToArray();
        }

        public static Day CreateDayObject(int dayNumber)
        {
            if (dayTypes == null) { InitializeDictionary(); }
            return dayTypes.ContainsKey(dayNumber)
                ? (Day)Activator.CreateInstance(dayTypes[dayNumber])
                : null;
        }
    }
}
