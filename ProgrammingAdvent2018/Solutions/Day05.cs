// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Diagnostics;
using System.Text;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day05 : Day
    {
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

            int postReactionLength = PostReactionLength(input);

            int shortestLength = int.MaxValue;
            for (char c = 'a'; c <= 'z'; c++)
            {
                shortestLength = Math.Min(shortestLength, PostReactionLength(input, c));
            }

            sw.Stop();
            output.WriteAnswers(postReactionLength, shortestLength, sw);
            return output;
        }

        private int PostReactionLength(string input, char? removedType = null)
        {
            if (input.Length < 2) { return input.Length; }
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (char.ToLower(c) == removedType) { continue; }
                if (sb.Length == 0)
                {
                    sb.Append(c);
                    continue;
                }
                if (char.ToLower(sb[^1]) == char.ToLower(c) && sb[^1] != c)
                {
                    sb.Remove(sb.Length - 1, 1);
                    continue;
                }
                sb.Append(c);
            }
            return sb.Length;
        }
    }
}
