// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
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
            int partOneX = -1;
            int partOneY = -1;
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

            int partTwoPower = int.MinValue;
            int partTwoX = -1;
            int partTwoY = -1;
            int partTwoSize = -1;
            for (int y = 1; y <= 300; y++)
            {
                for (int x = 1; x <= 300; x++)
                {
                    int maxSquareSize = Math.Min(300 - x + 1, 300 - y + 1);
                    int sum = 0;
                    for (int squareSize = 1; squareSize <= maxSquareSize; squareSize++)
                    {
                        int stepX = x + squareSize - 1;
                        int stepY = y;
                        while (stepY <= y + squareSize - 1)
                        {
                            sum += gridPower[stepX - 1, stepY - 1];
                            if (stepY < y + squareSize - 1) { stepY++; }
                            else break;
                        }
                        while (stepX > x)
                        {
                            stepX--;
                            sum += gridPower[stepX - 1, stepY - 1];
                        }
                        if (sum > partTwoPower)
                        {
                            partTwoPower = sum;
                            partTwoX = x;
                            partTwoY = y;
                            partTwoSize = squareSize;
                        }
                    }
                }
            }
            string partTwoAnswer = partTwoX + "," + partTwoY + "," + partTwoSize;

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int CellPowerLevel(int x, int y, int serialNumber)
        {
            return ((((x + 10) * y) + serialNumber) * (x + 10) / 100 % 10) - 5;
        }
    }
}
