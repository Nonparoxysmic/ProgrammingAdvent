// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day02 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();

            int countWithTwo = 0;
            int countWithThree = 0;
            foreach (string line in inputLines)
            {
                char[] characters = line.ToCharArray();
                Array.Sort(characters);
                char current = '\0';
                int count = 1;
                bool hasTwo = false;
                bool hasThree = false;
                foreach (char c in characters)
                {
                    if (c != current)
                    {
                        hasTwo |= count == 2;
                        hasThree |= count == 3;
                        count = 1;
                        current = c;
                    }
                    else
                    {
                        count++;
                    }
                }
                hasTwo |= count == 2;
                hasThree |= count == 3;
                if (hasTwo) { countWithTwo++; }
                if (hasThree) { countWithThree++; }
            }
            int checksum = countWithTwo * countWithThree;

            sw.Stop();
            output.WriteAnswers(checksum, null, sw);
            return output;
        }
    }
}
