// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day22 : Day
    {
        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int( 0, -1),  // [0] up
            new Vector2Int( 1,  0),  // [1] right
            new Vector2Int( 0,  1),  // [2] down
            new Vector2Int(-1,  0)   // [3] left
        };

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            if (inputLines.Length % 2 != 1)
            {
                output.WriteError("Number of lines of input is not odd.", sw);
                return output;
            }
            if (inputLines[0].Length % 2 != 1)
            {
                output.WriteError("Length of first line of input is not odd.", sw);
                return output;
            }
            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                if (!Regex.IsMatch(line, @"^[.#]+$"))
                {
                    output.WriteError($"Invalid characters in input line \"{line}\".", sw);
                    return output;
                }
                if (line.Length != inputLines[0].Length)
                {
                    output.WriteError($"All lines of input are not the same length.", sw);
                    return output;
                }
            }

            int partOneAnswer = PartOneAnswer(inputLines);
            int partTwoAnswer = PartTwoAnswer(inputLines);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int PartOneAnswer(string[] inputLines)
        {
            List<Vector2Int> infectedNodes = new List<Vector2Int>();
            int colOffset = (inputLines[0].Length - 1) / 2;
            int rowOffset = (inputLines.Length - 1) / 2;
            for (int row = 0; row < inputLines.Length; row++)
            {
                for (int col = 0; col < inputLines[0].Length; col++)
                {
                    if (inputLines[row][col] == '#')
                    {
                        infectedNodes.Add(new Vector2Int(col - colOffset, row - rowOffset));
                    }
                }
            }

            Vector2Int currentPosition = Vector2Int.Zero;
            int direction = 0;
            int sum = 0;
            for (int i = 0; i < 10_000; i++)
            {
                if (infectedNodes.Contains(currentPosition))
                {
                    direction = (direction + 5) % 4;
                    infectedNodes.Remove(currentPosition);
                }
                else
                {
                    direction = (direction + 3) % 4;
                    infectedNodes.Add(currentPosition);
                    sum++;
                }
                currentPosition += directions[direction];
            }
            return sum;
        }

        private int PartTwoAnswer(string[] inputLines, int gridSize = 501)
        {
            try
            {
                if (gridSize % 2 == 0) { gridSize++; }
                char[,] nodes = new char[gridSize, gridSize];
                int colOffset = (gridSize - inputLines[0].Length) / 2;
                int rowOffset = (gridSize - inputLines.Length) / 2;
                for (int row = 0; row < inputLines.Length; row++)
                {
                    for (int col = 0; col < inputLines[0].Length; col++)
                    {
                        nodes[col + colOffset, row + rowOffset] = inputLines[row][col];
                    }
                }

                Vector2Int currentPosition = new Vector2Int(gridSize / 2, gridSize / 2);
                int direction = 0;
                int sum = 0;
                for (int i = 0; i < 10_000_000; i++)
                {
                    switch (nodes[currentPosition.X, currentPosition.Y])
                    {
                        case 'F':
                            direction = (direction + 6) % 4;
                            nodes[currentPosition.X, currentPosition.Y] = '.';
                            break;
                        case '#':
                            direction = (direction + 5) % 4;
                            nodes[currentPosition.X, currentPosition.Y] = 'F';
                            break;
                        case 'W':
                            nodes[currentPosition.X, currentPosition.Y] = '#';
                            sum++;
                            break;
                        default:
                            direction = (direction + 3) % 4;
                            nodes[currentPosition.X, currentPosition.Y] = 'W';
                            break;
                    }
                    currentPosition += directions[direction];
                }
                return sum;
            }
            catch (IndexOutOfRangeException)
            {
                return PartTwoAnswer(inputLines, gridSize * 2);
            }
            catch
            {
                return -1;
            }
        }
    }
}
