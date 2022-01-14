// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day01 : Day
    {
        public override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            foreach (char c in input)
            {
                if (c < 48 || c > 57)
                {
                    if (c < 32 || c > 126)
                    {
                        output.WriteAnswers("ERROR: Invalid character in input.", "N/A", sw.ElapsedMilliseconds);
                    }
                    else
                    {
                        output.WriteAnswers("ERROR: Invalid character '" + c + "' in input.", "N/A", sw.ElapsedMilliseconds);
                    }
                    return output;
                }
            }
            int sum = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                {
                    sum += input[i] - 48;
                }
            }
            if (input[0] == input[^1])
            {
                sum += input[0] - 48;
            }

            sw.Stop();
            output.WriteAnswers(sum.ToString(), null, sw.ElapsedMilliseconds);
            return output;
        }
    }
}
