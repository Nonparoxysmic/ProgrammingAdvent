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
            for (int y = 0; y < 300; y++)
            {
                for (int x = 0; x < 300; x++)
                {
                    gridPower[x, y] = CellPowerLevel(x + 1, y + 1, gridSerialNumber);
                }
            }

            int[,] summedAreaTable = new int[300, 300];
            for (int y = 0; y < 300; y++)
            {
                for (int x = 0; x < 300; x++)
                {
                    summedAreaTable[x, y] = gridPower[x, y];
                    if (x > 0)
                    {
                        summedAreaTable[x, y] += summedAreaTable[x - 1, y];
                    }
                    if (y > 0)
                    {
                        summedAreaTable[x, y] += summedAreaTable[x, y - 1];
                    }
                    if (x > 0 && y > 0)
                    {
                        summedAreaTable[x, y] -= summedAreaTable[x - 1, y - 1];
                    }
                }
            }

            int partOnePower = int.MinValue;
            int partOneX = -1;
            int partOneY = -1;
            int partTwoPower = int.MinValue;
            int partTwoX = -1;
            int partTwoY = -1;
            int partTwoSize = -1;
            for (int y = 0; y < 300; y++)
            {
                for (int x = 0; x < 300; x++)
                {
                    int maxSquareSize = Math.Min(300 - x, 300 - y);
                    for (int squareSize = 1; squareSize <= maxSquareSize; squareSize++)
                    {
                        int totalPower = summedAreaTable[x + squareSize - 1, y + squareSize - 1];
                        if (x > 0)
                        {
                            totalPower -= summedAreaTable[x - 1, y + squareSize - 1];
                        }
                        if (y > 0)
                        {
                            totalPower -= summedAreaTable[x + squareSize - 1, y - 1];
                        }
                        if (x > 0 && y > 0)
                        {
                            totalPower += summedAreaTable[x - 1, y - 1];
                        }
                        if (totalPower > partTwoPower)
                        {
                            partTwoPower = totalPower;
                            partTwoX = x;
                            partTwoY = y;
                            partTwoSize = squareSize;
                        }
                        if (squareSize == 3 && totalPower > partOnePower)
                        {
                            partOnePower = totalPower;
                            partOneX = x;
                            partOneY = y;
                        }
                    }
                }
            }
            partOneX++;
            partOneY++;
            string partOneAnswer = partOneX + "," + partOneY;
            partTwoX++;
            partTwoY++;
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
