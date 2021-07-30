// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day08 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            LittleScreen screen = new LittleScreen(50, 6);
            foreach (string line in inputLines)
            {
                // TODO: process each instruction
            }

            stopwatch.Reset();
            return solution;
        }
    }

    class LittleScreen
    {
        readonly int width;
        readonly int height;
        readonly bool[,] pixels;

        public LittleScreen(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new bool[width, height];
        }

        public void Rect(int wide, int tall)
        {
            for (int col = 0; col < wide; col++)
            {
                for (int row = 0; row < tall; row++)
                {
                    pixels[col, row] = true;
                }
            }
        }

        public void RotateRow(int row, int steps)
        {
            bool wrap = pixels[width - 1, row];
            for (int i = 0; i < width - 1; i++)
            {
                pixels[i + 1, row] = pixels[i, row];
            }
            pixels[0, row] = wrap;
        }

        public void RotateColumn(int col, int steps)
        {
            bool wrap = pixels[col, height - 1];
            for (int i = 0; i < height - 1; i++)
            {
                pixels[col, i + 1] = pixels[col, i];
            }
            pixels[col, 0] = wrap;
        }
    }
}
