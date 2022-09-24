// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day15 : Day
    {
        private readonly Regex validLine = new Regex(@"^[#\.GE]+$");

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

            if (!ReadInput(inputLines, out MapArray<char> map, out List<Unit> units, out string message1))
            {
                output.WriteError(message1, sw);
                return output;
            }
            if (!CalculateOutcome(map, units, out int partOneAnswer, out string message2))
            {
                output.WriteError(message2, sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private bool ReadInput(string[] inputLines,
            out MapArray<char> map, out List<Unit> units, out string message)
        {
            units = new List<Unit>();
            if (inputLines.Length == 0 || inputLines[0].Length == 0)
            {
                map = MapArray<char>.Empty;
                message = "Invalid input.";
                return false;
            }
            map = new MapArray<char>(inputLines[0].Length, inputLines.Length, 1, '#');
            for (int y = 0; y < inputLines.Length; y++)
            {
                if (inputLines[y].Length != inputLines[0].Length)
                {
                    message = "Input lines must be the same length.";
                    return false;
                }
                Match lineMatch = validLine.Match(inputLines[y]);
                if (!lineMatch.Success)
                {
                    message = $"Invalid characters in line {y}.";
                    return false;
                }
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    map[x, y] = inputLines[y][x];
                    if (map[x, y] == 'G' || map[x, y] == 'E')
                    {
                        units.Add(new Unit(map[x, y], x, y));
                    }
                }
            }
            if (units.Count == 0)
            {
                message = "No combat units on the map";
                return false;
            }
            message = string.Empty;
            return true;
        }

        private bool CalculateOutcome(MapArray<char> map, List<Unit> units, out int answer, out string message)
        {
            int roundLimit = 60;
            answer = -1;
            for (int round = 0; round < roundLimit; round++)
            {
                units.Sort();
                foreach (Unit unit in units)
                {
                    // TODO: Unit acts.
                }
                units.RemoveAll(u => u.IsDead);

                // TODO: Check if combat has ended. Set answer and break if so.
            }
            if (answer < 0)
            {
                message = $"Combat did not end after {roundLimit} rounds.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        internal void Debug_DrawMap(MapArray<char> map)
        {
            Debug.WriteLine("");
            for (int y = 0 - map.Border; y < map.Height + map.Border; y++)
            {
                for (int x = 0 - map.Border; x < map.Width + map.Border; x++)
                {
                    if (char.IsControl(map[x, y]))
                    {
                        Debug.Write(' ');
                    }
                    else
                    {
                        Debug.Write(map[x, y]);
                    }
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("");
        }

        internal void Debug_Tests()
        {
            (string, int)[] examples = new (string, int)[]
            {
                ("#######\xA#.G...#\xA#...EG#\xA#.#.#G#\xA#..G#E#\xA#.....#\xA#######", 27730),
                ("#######\xA#G..#E#\xA#E#E.E#\xA#G.##.#\xA#...#E#\xA#...E.#\xA#######", 36334),
                ("#######\xA#E..EG#\xA#.#G.E#\xA#E.##E#\xA#G..#.#\xA#..E#.#\xA#######", 39514),
                ("#######\xA#E.G#.#\xA#.#G..#\xA#G.#.G#\xA#G..#.#\xA#...E.#\xA#######", 27755),
                ("#######\xA#.E...#\xA#.#..G#\xA#.###.#\xA#E#G#G#\xA#...#G#\xA#######", 28944),
                ("#########\xA#G......#\xA#.E.#...#\xA#..##..G#\xA#...##..#\xA#...#...#\xA#.G...G.#\xA#.....G.#\xA#########", 18740)
            };
            for (int i = 0; i < examples.Length; i++)
            {
                string exampleInput = examples[i].Item1;
                int exampleOutput = examples[i].Item2;
                string[] lines = exampleInput.ToLines();
                if (!ReadInput(lines, out MapArray<char> map, out List<Unit> units, out string message1))
                {
                    Debug.WriteLine($"DEBUG {i}: " + message1);
                    continue;
                }
                if (!CalculateOutcome(map, units, out int calculatedAnswer, out string message2))
                {
                    Debug.WriteLine($"DEBUG {i}: " + message2);
                    continue;
                }
                if (calculatedAnswer == exampleOutput)
                {
                    Debug.WriteLine($"DEBUG {i}: MATCH");
                }
                else
                {
                    Debug.WriteLine($"DEBUG {i}: EXPECTED {exampleOutput}, ACTUAL {calculatedAnswer}");
                }
            }
        }

        private class Unit : IComparable
        {
            public bool IsDead { get; set; }
            public char Type { get; private set; }
            public int HP { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public Unit(char type, int x, int y)
            {
                IsDead = false;
                Type = type;
                HP = 200;
                X = x;
                Y = y;
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return -1;
                }
                if (obj is Unit otherUnit)
                {
                    if (X == otherUnit.X && Y == otherUnit.Y)
                    {
                        return 0;
                    }
                    if (Y < otherUnit.Y || (Y == otherUnit.Y && X < otherUnit.X))
                    {
                        return -1;
                    }
                }
                return 1;
            }
        }
    }
}
