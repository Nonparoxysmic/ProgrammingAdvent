// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day18 : Day
    {
        static readonly Regex validLine = new Regex(@"^[.|#]+$");

        static readonly (int Δx, int Δy)[] adjacentDirections = new (int, int)[]
        {
            (-1, -1),
            ( 0, -1),
            ( 1, -1),
            (-1,  0),
            ( 1,  0),
            (-1,  1),
            ( 0,  1),
            ( 1,  1)
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
                output.WriteError("Input is too small.", sw);
                return output;
            }
            MapArray<char> map = new MapArray<char>(inputLines[0].Length, inputLines.Length, 1, 'X', (0, 0));
            for (int y = 0; y < inputLines.Length; y++)
            {
                if (inputLines[y].Length != inputLines[0].Length)
                {
                    output.WriteError("Input lines must be the same length.", sw);
                    return output;
                }
                Match validMatch = validLine.Match(inputLines[y]);
                if (!validMatch.Success)
                {
                    output.WriteError($"Line {y} contains invalid characters.", sw);
                    return output;
                }
                for (int x = 0; x < map.Width; x++)
                {
                    map[x, y] = inputLines[y][x];
                }
            }

            MapArray<char> buffer = new MapArray<char>(map.Width, map.Height, 1, 'X', (0, 0));
            for (int i = 0; i < 10; i++)
            {
                Step(ref map, ref buffer);
            }

            int partOneAnswer = ResourceValue(map);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private void Step(ref MapArray<char> map, ref MapArray<char> buffer)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    buffer[x, y] = NewContents(map, x, y);
                }
            }
            MapArray<char> temp = map;
            map = buffer;
            buffer = temp;
        }

        private char NewContents(MapArray<char> map, int x, int y)
        {
            switch (map[x, y])
            {
                case '.':
                    return CountAdjacent(map, x, y, '|') >= 3 ? '|' : '.';
                case '|':
                    return CountAdjacent(map, x, y, '#') >= 3 ? '#' : '|';
                case '#':
                    bool remains = CountAdjacent(map, x, y, '#') >= 1 && CountAdjacent(map, x, y, '|') >= 1;
                    return remains ? '#' : '.';
                default:
                    return 'X';
            }
        }

        private int CountAdjacent(MapArray<char> map, int x, int y, char target)
        {
            int sum = 0;
            foreach (var (Δx, Δy) in adjacentDirections)
            {
                if (map[x + Δx, y + Δy] == target)
                {
                    sum++;
                }
            }
            return sum;
        }

        private int ResourceValue(MapArray<char> map)
        {
            int trees = 0;
            int lumberyards = 0;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[x, y] == '|')
                    {
                        trees++;
                    }
                    else if (map[x, y] == '#')
                    {
                        lumberyards++;
                    }
                }
            }
            return trees * lumberyards;
        }
    }
}
