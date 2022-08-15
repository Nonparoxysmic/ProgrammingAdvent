// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

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

            sw.Stop();
            output.WriteAnswers(postReactionLength, null, sw);
            return output;
        }

        private int PostReactionLength(string input)
        {
            if (input.Length < 2) { return input.Length; }
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
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
