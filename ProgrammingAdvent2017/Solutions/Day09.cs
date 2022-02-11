// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day09 : Day
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
            if (input[0] != '{' || input[^1] != '}')
            {
                output.WriteError("Input must start with '{' and end with '}'.", sw);
                return output;
            }

            string charactersCanceled = CancelCharacters(input);
            string garbageEmptied = EmptyGarbage(charactersCanceled, out int garbageLength);
            int score = CalculateScore(garbageEmptied);

            sw.Stop();
            output.WriteAnswers(score, garbageLength, sw);
            return output;
        }

        private string CancelCharacters(string input)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '!') { i++; }
                else { _ = output.Append(input[i]); }
            }
            return output.ToString();
        }

        private string EmptyGarbage(string input, out int garbageLength)
        {
            int lengthDiscarded = 0;
            while (true)
            {
                Match match = Regex.Match(input, @"(?<=<)[^>]+(?=>)");
                if (match.Success)
                {
                    input = input.Substring(0, match.Index)
                        + input[(match.Index + match.Value.Length)..];
                    lengthDiscarded += match.Value.Length;
                }
                else { break; }
            }
            garbageLength = lengthDiscarded;
            return input;
        }

        private int CalculateScore(string input)
        {
            int score = 0;
            int depth = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '{')
                {
                    depth++;
                }
                else if (input[i] == '}')
                {
                    score += depth;
                    depth--;
                }
            }
            return score;
        }
    }
}
