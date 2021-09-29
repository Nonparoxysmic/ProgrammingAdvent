// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day19 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Trim().Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            if (!int.TryParse(input.Trim(), out int elfCount))
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            stopwatch.Start();

            int partOneSolution = PartOne(elfCount);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int PartOne(int elfCount)
        {
            if (elfCount < 2) return elfCount;

            var elves = new Queue<int>();
            for (int i = 1; i <= elfCount; i++)
            {
                elves.Enqueue(i);
            }

            while (elves.Count > 1)
            {
                elves.Enqueue(elves.Dequeue());
                elves.Dequeue();
            }

            return elves.Dequeue();
        }
    }
}
