// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day11 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 4 
                || inputLines[0].Length < 30 || inputLines[1].Length < 30 
                || inputLines[2].Length < 30 || inputLines[3].Length < 30
                || inputLines[0].Substring(0, 25) != "The first floor contains "
                || inputLines[1].Substring(0, 26) != "The second floor contains "
                || inputLines[2].Substring(0, 25) != "The third floor contains "
                || inputLines[3].Substring(0, 26) != "The fourth floor contains ")
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            stopwatch.Start();

            for (int i = 0; i < 4; i++)
            {
                inputLines[i] = inputLines[i].Substring(25 + i % 2);
            }

            stopwatch.Reset();
            return solution;
        }
    }
}
