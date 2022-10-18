// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
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

            for (int i = 10; i < 512; i++)
            {
                Step(ref map, ref buffer);
            }
            CycleDetector cycleDetector = new CycleDetector(512, ResourceValue(map));
            for (int i = 512; i < 4096; i++)
            {
                Step(ref map, ref buffer);
                cycleDetector.Add(i + 1, ResourceValue(map));
                if (cycleDetector.Success)
                {
                    break;
                }
            }
            if (!cycleDetector.Success)
            {
                output.WriteError("Did not find a cycle after 4,096 minutes.", sw);
                return output;
            }

            int partTwoAnswer = cycleDetector.Extrapolate(1_000_000_000);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
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

        private class CycleDetector
        {
            public bool Success { get; private set; }

            private readonly int _firstKey;
            private int _cycleIndex;
            private int _cycleLength;
            private readonly Dictionary<int, int> _history = new Dictionary<int, int>();

            public CycleDetector(int minutesPassed, int resourceValue)
            {
                _firstKey = minutesPassed;
                _cycleIndex = 0;
                _cycleLength = 1;
                Add(minutesPassed, resourceValue);
            }

            public void Add(int minutesPassed, int resourceValue)
            {
                _history.Add(minutesPassed, resourceValue);
                if (minutesPassed % 64 == 0)
                {
                    CheckForCycle(minutesPassed);
                }
            }

            private void CheckForCycle(int lastKey)
            {
                if (Success || lastKey - _firstKey < 4)
                {
                    return;
                }
                int middleKey = lastKey - (lastKey - _firstKey) / 2;
                for (int i = lastKey - 1; i >= middleKey; i--)
                {
                    if (_history[lastKey] == _history[i] && _history[i] == _history[2 * i - lastKey])
                    {
                        if (IsCycle(2 * i - lastKey, lastKey - i))
                        {
                            Success = true;
                            _cycleIndex = 2 * i - lastKey;
                            _cycleLength = lastKey - i;
                            return;
                        }
                    }
                }
            }

            private bool IsCycle(int startingIndex, int length)
            {
                for (int i = startingIndex; i < startingIndex + length; i++)
                {
                    if (_history[i] != _history[i + length])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int Extrapolate(int minutesPassed)
            {
                if (!Success || minutesPassed < _cycleIndex)
                {
                    return -400;
                }
                int index = (minutesPassed - _cycleIndex) % _cycleLength + _cycleIndex;
                if (!_history.ContainsKey(index))
                {
                    return -500;
                }
                return _history[index];
            }
        }
    }
}
