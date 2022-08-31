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

            int[,] gridPower = new int[300, 300];
            for (int y = 1; y <= 300; y++)
            {
                for (int x = 1; x <= 300; x++)
                {
                    gridPower[x - 1, y - 1] = CellPowerLevel(x, y, gridSerialNumber);
                }
            }
            int[,] threeByThreePower = new int[298, 298];
            for (int y = 1; y <= 298; y++)
            {
                for (int x = 1; x <= 298; x++)
                {
                    for (int Δy = 0; Δy < 3; Δy++)
                    {
                        for (int Δx = 0; Δx < 3; Δx++)
                        {
                            threeByThreePower[x - 1, y - 1] += gridPower[x + Δx - 1, y + Δy - 1];
                        }
                    }
                }
            }
            int partOnePower = int.MinValue;
            int partOneX = 0;
            int partOneY = 0;
            for (int y = 1; y <= 298; y++)
            {
                for (int x = 1; x <= 298; x++)
                {
                    if (threeByThreePower[x - 1, y - 1] > partOnePower)
                    {
                        partOnePower = threeByThreePower[x - 1, y - 1];
                        partOneX = x;
                        partOneY = y;
                    }
                }
            }
            string partOneAnswer = partOneX + "," + partOneY;

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private int CellPowerLevel(int x, int y, int serialNumber)
        {
            return ((((x + 10) * y) + serialNumber) * (x + 10) / 100 % 10) - 5;
        }
    }
}
