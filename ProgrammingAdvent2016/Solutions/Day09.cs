// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day09 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            input = input.RemoveWhitespace();
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            StringBuilder decompressed = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    // TODO: process marker
                }
                else decompressed.Append(input[i]);
            }
            solution.WriteSolution(1, decompressed.Length, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
