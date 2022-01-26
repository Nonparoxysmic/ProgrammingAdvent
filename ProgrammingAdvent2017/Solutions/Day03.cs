// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day03 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (!int.TryParse(input.Trim(), out int inputValue))
            {
                output.WriteError("Input is not an integer.", sw);
                return output;
            }
            if (inputValue < 1)
            {
                output.WriteError("Input is less than one.", sw);
                return output;
            }

            (int, int) inputValueCoords = FindCoordinates(inputValue);
            int steps = Math.Abs(inputValueCoords.Item1) + Math.Abs(inputValueCoords.Item2);

            sw.Stop();
            output.WriteAnswers(steps, null, sw);
            return output;
        }

        private (int, int) FindCoordinates(int number)
        {
            if (number < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (number == 1)
            {
                return (0, 0);
            }
            int squareSize = SquareSize(number);
            int x = squareSize / 2;
            int y = 1 - x;
            int value = (squareSize - 2) * (squareSize - 2) + 1;
            int[] corners = new int[4];
            corners[3] = squareSize * squareSize;
            corners[2] = corners[3] - squareSize + 1;
            corners[1] = corners[2] - squareSize + 1;
            corners[0] = corners[1] - squareSize + 1;
            while (value < corners[0])
            {
                if (value == number)
                {
                    return (x, y);
                }
                value++;
                y++;
            }
            while (value < corners[1])
            {
                if (value == number)
                {
                    return (x, y);
                }
                value++;
                x--;
            }
            while (value < corners[2])
            {
                if (value == number)
                {
                    return (x, y);
                }
                value++;
                y--;
            }
            while (value < corners[3])
            {
                if (value == number)
                {
                    return (x, y);
                }
                value++;
                x++;
            }
            return (x, y);
        }

        private int SquareSize(int number)
        {
            for (int size = 1; size < 100_000; size += 2)
            {
                if ((size * size) >= number) { return size; }
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
