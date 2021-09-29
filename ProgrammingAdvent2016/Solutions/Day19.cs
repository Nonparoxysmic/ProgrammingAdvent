// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

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
            if (!uint.TryParse(input.Trim(), out uint elfCount))
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            stopwatch.Start();

            uint partOneSolution = PartOneQuick(elfCount);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        //int PartOne(int elfCount)
        //{
        //    if (elfCount < 2) return elfCount;
        //    var elves = new Queue<int>();
        //    for (int i = 1; i <= elfCount; i++)
        //    {
        //        elves.Enqueue(i);
        //    }
        //    while (elves.Count > 1)
        //    {
        //        elves.Enqueue(elves.Dequeue());
        //        elves.Dequeue();
        //    }
        //    return elves.Dequeue();
        //}

        uint PartOneQuick(uint elfCount)
        {
            if (elfCount < 2) return elfCount;
            if (elfCount >= 0x80000000)
            {
                elfCount <<= 1;
                return ++elfCount;
            }

            uint msb = elfCount;
            for (int i = 1; i <= 16; i *= 2)
            {
                msb |= msb >> i;
            }
            msb = ++msb >> 1;

            elfCount &= ~msb;
            elfCount <<= 1;
            return ++elfCount;
        }
    }
}
