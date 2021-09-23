// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day16 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            input = input.Trim();
            if (!Regex.IsMatch(input, @"^[01]+$"))
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }

            

            stopwatch.Reset();
            return solution;
        }

        string ReverseAndInvertBits(string input)
        {
            if (input.Length == 0) return input;
            StringBuilder sb = new StringBuilder();
            for (int i = input.Length - 1; i >= 0; i--)
            {
                switch (input[i])
                {
                    case '0':
                        sb.Append('1');
                        break;
                    case '1':
                        sb.Append('0');
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
