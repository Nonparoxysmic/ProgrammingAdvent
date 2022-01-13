// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;

namespace ProgrammingAdvent2017.Program
{
    internal abstract class Day
    {
        private static Dictionary<int, Day> dayObjects;

        public static Day GetDayObject(int dayNumber)
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

        public abstract PuzzleAnswers Solve(string input);
    }
}
