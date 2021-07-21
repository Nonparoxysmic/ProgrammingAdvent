// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day05 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            input = input.Trim();
            stopwatch.Start();

            StringBuilder sb = new StringBuilder();
            byte[] hashFunctionInput = Encoding.UTF8.GetBytes(input + "0");
            for (int i = 1; i < int.MaxValue; i++)
            {
                byte[] hash = AdventMD5.ComputeHash(hashFunctionInput);
                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10)
                {
                    // Five leading zeroes found.
                    sb.Append(hash[2].ToString("x"));
                    if (sb.Length >= 8) break;
                }

                // Recalculate the hash function input if the last byte reaches '9' (57):
                    if (hashFunctionInput[hashFunctionInput.GetUpperBound(0)] > 56)
                {
                    hashFunctionInput = Encoding.UTF8.GetBytes(input + i);
                }
                // Increment the counter on the hash function input by incrementing the last byte:
                else
                {
                    hashFunctionInput[hashFunctionInput.GetUpperBound(0)]++;
                }
            }
            solution.WriteSolution(1, sb.ToString(), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
