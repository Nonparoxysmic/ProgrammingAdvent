// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day14 : Day
    {
        internal static int[] bitsSet = new int[]
        {
            0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4
        };

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }

            int sum = 0;
            for (int i = 0; i < 128; i++)
            {
                byte[] hash = Day10.KnotHashBytes(input + "-" + i);
                int bits = CountBitsSet(hash);
                sum += bits;
            }

            sw.Stop();
            output.WriteAnswers(sum, null, sw);
            return output;
        }

        internal static int CountBitsSet(byte[] bytes)
        {
            int sum = 0;
            foreach (byte b in bytes)
            {
                sum += bitsSet[b & 0b1111] + bitsSet[b >> 4];
            }
            return sum;
        }
    }
}
