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

            StringBuilder partOnePassword = new StringBuilder();
            long partOneTime = 0;
            char[] partTwoPassword = new char[8];
            int partTwoCharsFound = 0;
            byte[] hashFunctionInput = Encoding.UTF8.GetBytes(input + "0");
            for (int i = 1; i < int.MaxValue; i++)
            {
                byte[] hash = AdventMD5.ComputeHash(hashFunctionInput);
                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10)
                {
                    // Five leading zeroes found.
                    if (partOnePassword.Length < 8)
                    {
                        partOnePassword.Append(hash[2].ToString("x"));
                        partOneTime = stopwatch.ElapsedMilliseconds;
                    }
                    if (hash[2] < 8)
                    {
                        // Sixth character is a valid position.
                        if (partTwoPassword[hash[2]] == 0)
                        {
                            // Character in this position has not yet been set.
                            partTwoPassword[hash[2]] = (hash[3] >> 4).ToString("x")[0];
                            partTwoCharsFound++;
                        }
                    }
                    if (partOnePassword.Length >= 8 && partTwoCharsFound >= 8) break;
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
            solution.WriteSolution(1, partOnePassword, partOneTime);
            solution.WriteSolution(2, new string(partTwoPassword), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
