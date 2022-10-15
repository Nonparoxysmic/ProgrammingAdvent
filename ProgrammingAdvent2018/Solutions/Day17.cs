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
    internal class Day17 : Day
    {
        private readonly Regex horzClayLine = new Regex(@"^y=([0-9]{1,9}), x=([0-9]{1,9})\.\.([0-9]{1,9})$");
        private readonly Regex vertClayLine = new Regex(@"^x=([0-9]{1,9}), y=([0-9]{1,9})\.\.([0-9]{1,9})$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Read input.
            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            int minimumX = int.MaxValue;
            int minimumY = int.MaxValue;
            int maximumX = int.MinValue;
            int maximumY = int.MinValue;
            List<(int, int, int, int)> clayInput = new List<(int, int, int, int)>();
            foreach (string line in inputLines)
            {
                Match horizontalMatch = horzClayLine.Match(line);
                Match verticalMatch = vertClayLine.Match(line);
                if (!horizontalMatch.Success && !verticalMatch.Success)
                {
                    output.WriteError($"Invalid line in input \"{line}\".", sw);
                    return output;
                }
                Match match = horizontalMatch.Success ? horizontalMatch : verticalMatch;
                (int lowerX, int lowerY, int upperX, int upperY) clay = Clay(match);
                minimumX = Math.Min(minimumX, clay.lowerX);
                minimumY = Math.Min(minimumY, clay.lowerY);
                maximumX = Math.Max(maximumX, clay.upperX);
                maximumY = Math.Max(maximumY, clay.upperY);
                clayInput.Add(clay);
            }
            if (500 < minimumX || 500 > maximumX)
            {
                output.WriteError("Water hits no clay.", sw);
                return output;
            }

            // Create map from input.
            int width = maximumX - minimumX + 3;
            int height = maximumY - minimumY + 1;
            MapArray<char> map = new MapArray<char>(width, height, 2, '.', (minimumX - 1, minimumY));
            map.Fill('.');
            foreach ((int lowerX, int lowerY, int upperX, int upperY) in clayInput)
            {
                for (int y = lowerY; y <= upperY; y++)
                {
                    for (int x = lowerX; x <= upperX; x++)
                    {
                        map[x, y] = '#';
                    }
                }
            }
            (int X, int Y) springPosition = (500, 0);
            if (minimumY > 2)
            {
                springPosition = (500, minimumY - 2);
            }
            map[springPosition.X, springPosition.Y] = '+';

            // Carry out water pathfinding simulation.
            Pathfinder.Reset();
            Pathfinder.Map = map;
            new Pathfinder(springPosition.X, springPosition.Y, Pathfinder.State.Falling);
            int stepLimit = 65536;
            for (; stepLimit > 0; stepLimit--)
            {
                int count = Pathfinder.Count;
                if (count == 0)
                {
                    break;
                }
                for (int i = 0; i < count; i++)
                {
                    Pathfinder current = Pathfinder.Dequeue();
                    if (!current.Step())
                    {
                        output.WriteError($"Error encountered at ({current.X}, {current.Y}).", sw);
                        return output;
                    }
                    if (current.IsAlive && current.Y < maximumY + 2)
                    {
                        Pathfinder.Enqueue(current);
                    }
                }
            }
            if (stepLimit <= 0)
            {
                output.WriteError("Could not complete simulation in 65,536 steps.", sw);
                return output;
            }

            CountWaterTiles(map, out int partOneAnswer, out int partTwoAnswer);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private (int, int, int, int) Clay(Match match)
        {
            int fixedCoord = int.Parse(match.Groups[1].Value);
            int lowerRange = int.Parse(match.Groups[2].Value);
            int upperRange = int.Parse(match.Groups[3].Value);
            if (upperRange < lowerRange)
            {
                int temp = upperRange;
                upperRange = lowerRange;
                lowerRange = temp;
            }
            if (match.Groups[0].Value[0] == 'x')
            {
                return (fixedCoord, lowerRange, fixedCoord, upperRange);
            }
            else
            {
                return (lowerRange, fixedCoord, upperRange, fixedCoord);
            }
        }

        private void CountWaterTiles(MapArray<char> map, out int wetTiles, out int waterTiles)
        {
            int wetSandSum = 0;
            waterTiles = 0;
            for (int y = map.Position.Y; y < map.Position.Y + map.Height; y++)
            {
                for (int x = map.Position.X; x < map.Position.X + map.Width; x++)
                {
                    switch (map[x, y])
                    {
                        case '~':
                            waterTiles++;
                            break;
                        case '|':
                        case '+':
                            wetSandSum++;
                            break;
                    }
                }
            }
            wetTiles = waterTiles + wetSandSum;
        }

        private class Pathfinder
        {
            public static MapArray<char> Map { get; set; }
            public static int Count { get => _queue.Count; }

            public bool IsAlive { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }

            private static readonly Queue<Pathfinder> _queue = new Queue<Pathfinder>();
            private static readonly List<(int, int)> _splits = new List<(int, int)>();

            private State _state;

            public Pathfinder(int x, int y, State state)
            {
                IsAlive = true;
                X = x;
                Y = y;
                _state = state;
                _queue.Enqueue(this);
            }

            public static Pathfinder Dequeue()
            {
                return _queue.Dequeue();
            }

            public static void Enqueue(Pathfinder item)
            {
                _queue.Enqueue(item);
            }

            public static void Reset()
            {
                Map = null;
                _queue.Clear();
                _splits.Clear();
            }

            public bool Step()
            {
                if (_state == State.Dead)
                {
                    return true;
                }
                if (Map[X, Y] == '#')
                {
                    _state = State.Invalid;
                }
                if (Map[X, Y] == '~')
                {
                    _state = State.Floating;
                }
                switch (_state)
                {
                    case State.Falling:
                        // Fall down until hitting a solid surface.
                        if (Map[X, Y + 1] == '.' || Map[X, Y + 1] == '|')
                        {
                            Y++;
                            Map[X, Y] = '|';
                        }
                        else
                        {
                            _state = State.Spreading;
                        }
                        break;
                    case State.Floating:
                        // Rise above the surface of the water.
                        while (Map[X, Y] == '~')
                        {
                            Y--;
                        }
                        if (Map[X, Y] == '.' || Map[X, Y] == '|')
                        {
                            Map[X, Y] = '|';
                            _state = State.Spreading;
                        }
                        else
                        {
                            _state = State.Invalid;
                            Map[X, Y + 1] = 'X';
                        }
                        break;
                    case State.SlidingLeft:
                        Slide(-1);
                        break;
                    case State.SlidingRight:
                        Slide(1);
                        break;
                    case State.Splitting:
                        Split();
                        break;
                    case State.Spreading:
                        Spread();
                        break;
                }
                if (_state == State.Invalid)
                {
                    return false;
                }
                return true;
            }

            private void Slide(int direction)
            {
                // Not supposed to slide into a wall or in water.
                if (Map[X + direction, Y] == '#' || Map[X + direction, Y] == '~')
                {
                    _state = State.Invalid;
                    Map[X, Y] = 'X';
                    return;
                }
                // Slide horizontally until the fall.
                X += direction;
                Map[X, Y] = '|';
                if (Map[X, Y + 1] == '.' || Map[X, Y + 1] == '|')
                {
                    _state = State.Falling;
                }
            }

            private void Split()
            {
                // If a previous Pathfinder has already split from this location, this one can terminate.
                // This prevents the number of Pathfinders from exploding.
                if (_splits.Contains((X, Y)))
                {
                    IsAlive = false;
                    _state = State.Dead;
                    return;
                }
                new Pathfinder(X, Y, State.SlidingLeft);
                _state = State.SlidingRight;
                _splits.Add((X, Y));
            }

            private void Spread()
            {
                // Check for walls or falls on both sides, while spreading '|' tiles.
                Map[X, Y] = '|';
                bool leftWall = false;
                for (int Δx = -1; ; Δx--)
                {
                    if (Map[X + Δx, Y] == '#')
                    {
                        leftWall = true;
                        break;
                    }
                    Map[X + Δx, Y] = '|';
                    if (Map[X + Δx, Y + 1] == '.' || Map[X + Δx, Y + 1] == '|')
                    {
                        break;
                    }
                }
                bool rightWall = false;
                for (int Δx = 1; ; Δx++)
                {
                    if (Map[X + Δx, Y] == '#')
                    {
                        rightWall = true;
                        break;
                    }
                    Map[X + Δx, Y] = '|';
                    if (Map[X + Δx, Y + 1] == '.' || Map[X + Δx, Y + 1] == '|')
                    {
                        break;
                    }
                }

                // If both are walls, fill with water ('~').
                // Otherwise, send a pathfinder toward each fall.
                if (leftWall && rightWall)
                {
                    for (int Δx = -1; ; Δx--)
                    {
                        if (Map[X + Δx, Y] == '#')
                        {
                            break;
                        }
                        Map[X + Δx, Y] = '~';
                    }
                    for (int Δx = 1; ; Δx++)
                    {
                        if (Map[X + Δx, Y] == '#')
                        {
                            break;
                        }
                        Map[X + Δx, Y] = '~';
                    }
                    Map[X, Y] = '~';
                }
                else if (!leftWall && !rightWall)
                {
                    _state = State.Splitting;
                }
                else if (leftWall)
                {
                    _state = State.SlidingRight;
                }
                else if (rightWall)
                {
                    _state = State.SlidingLeft;
                }
            }

            public enum State
            {
                Invalid = -1,
                Dead,
                Falling,
                Floating,
                SlidingLeft,
                SlidingRight,
                Splitting,
                Spreading
            }
        }
    }
}
