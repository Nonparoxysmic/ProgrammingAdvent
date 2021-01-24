// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day18
    {
        public static void Solve()
        {
            string input1Path = @"Day18\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length == 0) Print.PrintErrorAndExit("Day 18: No data in " + input1Path);
            if (input1[0].Length == 0) Print.PrintErrorAndExit("Day 18: Cannot process empty lines in " + input1Path);

            int[,] lightGrid = new int[input1.Length, input1[0].Length];

            for (int row = 0; row < input1.Length; row++)
            {
                if (input1[row].Length != input1[0].Length)
                {
                    Print.PrintErrorAndExit("Day 18: Inconsistent line lengths in " + input1Path);
                }
                for (int col = 0; col < input1[row].Length; col++)
                {
                    switch (input1[row][col])
                    {
                        case '#':
                            lightGrid[row, col] = 1;
                            break;
                        case '.':
                            lightGrid[row, col] = 0;
                            break;
                        default:
                            Print.PrintErrorAndExit("Day 18: Invalid character '" + input1[row][col] + "' in " + input1Path);
                            break;
                    }
                }
            }

            for (int iterations = 0; iterations < 100; iterations++)
            {
                lightGrid = StepLightGrid(lightGrid);
            }

            Console.WriteLine("Day 18 Part One Answer: " + SumArray(lightGrid));
        }

        static int[,] StepLightGrid(int[,] input)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            int[,] output = new int[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    output[row, col] = input[row, col];
                    int neighbors = CountNeighborsOn(row, col, input);
                    if (input[row, col] == 1 && neighbors != 2 && neighbors != 3)
                    {
                        output[row, col] = 0;
                    }
                    if (input[row, col] == 0 && neighbors == 3)
                    {
                        output[row, col] = 1;
                    }
                }
            }
            return output;
        }

        static int CountNeighborsOn(int row, int col, int[,] input)
        {
            int rows = input.GetLength(0);
            int cols = input.GetLength(1);
            int count = 0;
            for (int r = Math.Max(row - 1, 0); r <= Math.Min(row + 1, rows - 1); r++)
            {
                for (int c = Math.Max(col - 1, 0); c <= Math.Min(col + 1, cols - 1); c++)
                {
                    if (r == row && c == col) continue;
                    if (input[r, c] == 1) count++;
                }
            }
            return count;
        }

        static int SumArray(int[,] input)
        {
            int sum = 0;
            foreach (int i in input)
            {
                sum += i;
            }
            return sum;
        }
    }
}
