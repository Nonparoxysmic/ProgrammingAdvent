// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
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

            int sum = 0;
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                int max = int.MinValue;
                int min = int.MaxValue;
                string[] values = line.Split();
                foreach (string value in values)
                {
                    if (int.TryParse(value, out int n))
                    {
                        max = Math.Max(max, n);
                        min = Math.Min(min, n);
                    }
                    else
                    {
                        output.WriteAnswers($"ERROR: Invalid term \"{value}\" in input.", "N/A",
                            sw.ElapsedMilliseconds);
                        return output;
                    }
                }
                sum += max - min;
            }


            sw.Stop();
            output.WriteAnswers(sum.ToString(), null, sw.ElapsedMilliseconds);
            return output;
        }
    }
}
