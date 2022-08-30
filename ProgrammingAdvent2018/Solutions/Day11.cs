// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day11 : Day
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
            if (!int.TryParse(input, out int gridSerialNumber))
            {
                output.WriteError("Invalid input.", sw);
                return output;
            }



            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private int CellPowerLevel(int x, int y, int serialNumber)
        {
            return ((((x + 10) * y) + serialNumber) * (x + 10) / 100 % 10) - 5;
        }
    }
}
