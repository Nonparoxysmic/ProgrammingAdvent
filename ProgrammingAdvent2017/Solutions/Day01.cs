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
            foreach (char c in input)
            {
                if (c < 48 || c > 57)
                {
                    if (c < 32 || c > 126)
                    {
                        output.WriteError("Invalid character in input.", sw);
                    }
                    else
                    {
                        output.WriteError($"Invalid character '{c}' in input.", sw);
                    }
                    return output;
                }
            }

            int sumOne = 0;
            int sumTwo = 0;
            int halfLen = input.Length / 2;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                {
                    sumOne += input[i] - 48;
                }
                if (i < halfLen)
                {
                    if (input[i] == input[i + halfLen])
                    {
                        sumTwo += 2 * (input[i] - 48);
                    }
                }
            }
            if (input.Length > 1 && input[0] == input[^1])
            {
                sumOne += input[0] - 48;
            }

            sw.Stop();
            output.WriteAnswers(sumOne, sumTwo, sw);
            return output;
        }
    }
}
