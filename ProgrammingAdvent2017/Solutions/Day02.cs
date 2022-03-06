// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day02 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            int sumOne = 0;
            int sumTwo = 0;
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                int max = int.MinValue;
                int min = int.MaxValue;
                List<int> numbers = new List<int>();
                string[] values = line.Split();
                foreach (string value in values)
                {
                    if (int.TryParse(value, out int n))
                    {
                        max = Math.Max(max, n);
                        min = Math.Min(min, n);
                        numbers.Add(n);
                    }
                    else
                    {
                        output.WriteError($"Invalid term \"{value}\" in input.", sw);
                        return output;
                    }
                }
                sumOne += max - min;
                if (numbers.Count < 2)
                {
                    output.WriteError($"Invalid line \"{line}\" in input.", sw);
                    return output;
                }
                try
                {
                    sumTwo += GetLineResult(numbers.ToArray());
                }
                catch
                {
                    output.WriteError("Invalid values in input.", sw);
                    return output;
                }
            }

            sw.Stop();
            output.WriteAnswers(sumOne, sumTwo, sw);
            return output;
        }

        internal int GetLineResult(int[] numbers)
        {
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    int higher = Math.Max(numbers[i], numbers[j]);
                    int lower = Math.Min(numbers[i], numbers[j]);
                    if (higher % lower == 0)
                    {
                        return higher / lower;
                    }
                }
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
