// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

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
            new Vector2Int( 0, -1),  // [0] north
            new Vector2Int( 1,  0),  // [1] east
            new Vector2Int( 0,  1),  // [2] south
            new Vector2Int(-1,  0)   // [3] west
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
            int partOneAnswer = 0;
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
                    partOneAnswer++;
                }
                currentPosition += directions[direction];
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }
    }
}
