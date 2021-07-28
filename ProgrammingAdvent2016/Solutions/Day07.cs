// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day07 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.Trim().ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            int numberOfTLS = 0;
            foreach (string line in inputLines)
            {
                string[] terms = line.Split(new char[] { '[', ']' });
                if (terms.Length == 0) continue;
                if (terms.Length == 1)
                {
                    if (ContainsABBA(terms[0])) numberOfTLS++;
                }
                if (terms.Length > 1)
                {
                    bool hasGoodABBA = false;
                    bool hasBadABBA = false;
                    for (int i = 0; i < terms.Length; i+=2)
                    {
                        if (ContainsABBA(terms[i])) hasGoodABBA = true;
                    }
                    for (int i = 1; i < terms.Length; i += 2)
                    {
                        if (ContainsABBA(terms[i])) hasBadABBA = true;
                    }
                    if (hasGoodABBA && !hasBadABBA) numberOfTLS++;
                }
            }
            solution.WriteSolution(1, numberOfTLS.ToString(), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        bool ContainsABBA(string input)
        {
            if (input.Length < 4) return false;
            for (int i = 0; i <= input.Length - 4; i++)
            {
                if (input[i] == input[i + 3] 
                    && input[i + 1] == input[i + 2] 
                    && input[i] != input[i + 1])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
