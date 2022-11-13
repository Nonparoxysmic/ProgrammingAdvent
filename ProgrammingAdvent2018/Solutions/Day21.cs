// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day21 : Day
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
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 32)
            {
                output.WriteError("Input too short.", sw);
                return output;
            }
            string[] tempA = inputLines[8].Split();
            string[] tempB = inputLines[12].Split();
            if (tempA.Length < 4 || tempB.Length < 4 ||
                !int.TryParse(tempA[1], out int A) || !int.TryParse(tempB[2], out int B)
                || A < 0 || B < 0)
            {
                output.WriteError("Input contains invalid lines.", sw);
                return output;
            }

            int partOneAnswer = PartOneAnswer(A, B);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private int PartOneAnswer(int A, int B)
        {
            int R2 = 65536;
            int R3 = A;
            while (true)
            {
                R3 += R2 & 255;
                R3 &= 16777215;
                R3 *= B;
                R3 &= 16777215;
                if (256 > R2) { break; }
                R2 /= 256;
            }
            return R3;
        }
    }
}
