// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2018.Program
{
    internal abstract class Day
    {
        private static Dictionary<int, Day> dayObjects;

        internal static int ParseDayNumber(string name)
        {
            return int.Parse(Regex.Match(name, @"\d+$").Value);
        }

        internal static Day GetDayObject(int dayNumber)
        {
            if (dayObjects == null) { InitializeDictionary(); }
            return dayObjects.ContainsKey(dayNumber)
                ? dayObjects[dayNumber]
                : throw new ApplicationException();
        }

        private static void InitializeDictionary()
        {
            dayObjects = new Dictionary<int, Day>();
            int[] numbers = Reflection.GetDayNumbers();
            foreach (int i in numbers)
            {
                dayObjects.Add(i, Reflection.CreateDayObject(i));
            }
        }

        internal abstract PuzzleAnswers Solve(string input);
    }
}
