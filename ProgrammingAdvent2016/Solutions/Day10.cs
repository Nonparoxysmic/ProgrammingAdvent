// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day10 : Day
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
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            Dictionary<int, Day10Bot> bots = new Dictionary<int, Day10Bot>();
            foreach (string line in inputLines)
            {
                string[] terms = line.Split();
                if (terms.Length == 12)
                {
                    // probably a sorting instruction
                }
                else if (terms.Length == 6)
                {
                    // probably a starting value
                }
            }

            stopwatch.Reset();
            return solution;
        }
    }

    class Day10Bot
    {
        public OutputTarget lowTarget;
        public int lowNumber;
        public OutputTarget highTarget;
        public int highNumber;

        public List<int> chips;

        public Day10Bot(OutputTarget lowTarget, int lowNumber, OutputTarget highTarget, int highNumber)
        {
            this.lowTarget = lowTarget;
            this.lowNumber = lowNumber;
            this.highTarget = highTarget;
            this.highNumber = highNumber;
            chips = new List<int>();
        }

        public void AddChip(int value, Dictionary<int, Day10Bot> bots)
        {
            chips.Add(value);
            if (chips.Count == 2)
            {
                // Give chips to targets
            }
        }
    }

    enum OutputTarget
    {
        OutputBin,
        Bot
    }
}
