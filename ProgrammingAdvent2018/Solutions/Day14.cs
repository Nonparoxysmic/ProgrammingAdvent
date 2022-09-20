// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using System.Text;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day14 : Day
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
            if (!int.TryParse(input, out int inputValue))
            {
                output.WriteError("Cannot parse input as an integer.", sw);
                return output;
            }

            string partOneAnswer = GetScores(inputValue, 10);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private string GetScores(int position, int length)
        {
            StringBuilder sb = new StringBuilder("37");
            int position0 = 0;
            int position1 = 1;
            while (sb.Length < position + length)
            {
                int next = sb[position0] + sb[position1] - 96;
                sb.Append(next);
                position0 += 1 + sb[position0] - 48;
                position0 %= sb.Length;
                position1 += 1 + sb[position1] - 48;
                position1 %= sb.Length;
            }
            return sb.ToString(position, length);
        }
    }
}
