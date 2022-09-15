// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day13 : Day
    {
        private readonly Regex validLine = new Regex(@"^[ |\-/\\+^v<>]{2,}$");

        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int( 0, -1),  // [0] ^ up
            new Vector2Int( 0,  1),  // [1] v down
            new Vector2Int(-1,  0),  // [2] < left
            new Vector2Int( 1,  0)   // [3] > right
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
            if (inputLines.Length < 2 || inputLines[0].Length < 2)
            {
                output.WriteError("Insufficient input.", sw);
                return output;
            }

            TrackMap tracks = new TrackMap(inputLines[0].Length, inputLines.Length);
            for (int y = 0; y < inputLines.Length; y++)
            {
                if (inputLines[y].Length != inputLines[0].Length)
                {
                    output.WriteError("Input lines must be the same length.", sw);
                    return output;
                }
                Match lineMatch = validLine.Match(inputLines[y]);
                if (!lineMatch.Success)
                {
                    output.WriteError($"Invalid characters in line {y}.", sw);
                    return output;
                }
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    char c = inputLines[y][x];
                    if (c == '^' || c == 'v' || c == '<' || c == '>')
                    {
                        int direction;
                        switch (c)
                        {
                            case '^':
                                direction = 0;
                                c = '|';
                                break;
                            case 'v':
                                direction = 1;
                                c = '|';
                                break;
                            case '<':
                                direction = 2;
                                c = '-';
                                break;
                            case '>':
                                direction = 3;
                                c = '-';
                                break;
                            default:
                                throw new ApplicationException();
                        }
                    }
                    tracks[x, y] = c;
                }
            }

            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private class TrackMap
        {
            public int Width { get; private set; }
            public int Height { get; private set; }

            private readonly char[,] tracks;

            public TrackMap(int width, int height)
            {
                Width = width;
                Height = height;
                tracks = new char[width + 2, height + 2];
            }

            public char this[int x, int y]
            {
                get => GetValue(x, y);
                set => SetValue(x, y, value);
            }

            private char GetValue(int x, int y)
            {
                return tracks[x + 1, y + 1];
            }

            private void SetValue(int x, int y, char c)
            {
                tracks[x + 1, y + 1] = c;
            }
        }
    }
}
