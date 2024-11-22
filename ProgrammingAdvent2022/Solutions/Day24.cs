// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day24 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        char[,] map = InputToMap(input);
        (int, int) start = (0, -1);
        (int, int) goal = (map.GetLength(0) - 1, map.GetLength(1));
        Position.Reset();
        int fewestMinutesAcrossValley = FewestMinutes(start, goal, map);

        return result.WriteAnswers(fewestMinutesAcrossValley, null);
    }

    private static char[,] InputToMap(string[] input)
    {
        char[,] map = new char[input[0].Length - 2, input.Length - 2];
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x, y] = input[y + 1][x + 1];
            }
        }
        return map;
    }

    private static bool IsNotBlizzard(int xPos, int yPos, int time, char[,] map)
    {
        if (time == 0)
        {
            return map[xPos, yPos] == '.';
        }
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int lookX = (xPos + time) % width;
        if (map[lookX, yPos] == '<') { return false; }
        lookX = (xPos + width - time % width) % width;
        if (map[lookX, yPos] == '>') { return false; }
        int lookY = (yPos + time) % height;
        if (map[xPos, lookY] == '^') { return false; }
        lookY = (yPos + height - time % height) % height;
        if (map[xPos, lookY] == 'v') { return false; }
        return true;
    }

    private static int FewestMinutes((int, int) start, (int, int) goal, char[,] map)
    {
        MinHeap<Position> open = new();
        Position initial = new(start.Item1, start.Item2, 0, map)
        {
            CostFromStart = 0,
            EstimatedCostToGoal = Distance(start, goal)
        };
        open.Insert(initial);
        while (open.Count > 0)
        {
            Position current = open.FindMin();
            if ((current.X, current.Y) == goal)
            {
                return current.CostFromStart;
            }
            open.DeleteMin();
            foreach ((Position neighbor, int cost) in current.NeighborsAndCosts())
            {
                int costToReachNeighbor = current.CostFromStart + cost;
                if (costToReachNeighbor < neighbor.CostFromStart)
                {
                    neighbor.CostFromStart = costToReachNeighbor;
                    if (neighbor.Open)
                    {
                        open.KeyDecreased(neighbor);
                    }
                    else
                    {
                        neighbor.EstimatedCostToGoal = Distance(neighbor.Coords, goal);
                        open.Insert(neighbor);
                    }
                }
            }
        }
        return -1;
    }

    private static int Distance((int, int) a, (int, int) b)
    {
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    private class Position(int x, int y, int time, char[,] map) : MinHeapNode<Position>
    {
        private static readonly Dictionary<(int, int, int), Position> _allPositions = [];

        public int X { get; } = x;
        public int Y { get; } = y;
        public int Time { get; } = time;
        public (int, int) Coords { get; } = (x, y);

        private readonly char[,] _map = map;
        private readonly int _width = map.GetLength(0);
        private readonly int _height = map.GetLength(1);

        public override IEnumerable<(Position, int)> NeighborsAndCosts()
        {
            if (Y < 0 || Y >= _height || IsNotBlizzard(X, Y, Time + 1, _map))
            {
                // can wait
                yield return (GetPosition(X, Y, Time + 1, _map), 1);
            }
            if (X > 0 && Y < _height && IsNotBlizzard(X - 1, Y, Time + 1, _map))
            {
                // can move left
                yield return (GetPosition(X - 1, Y, Time + 1, _map), 1);
            }
            if (X < _width - 1 && Y >= 0 && IsNotBlizzard(X + 1, Y, Time + 1, _map))
            {
                // can move right
                yield return (GetPosition(X + 1, Y, Time + 1, _map), 1);
            }
            if (Y > 0 || (X == 0 && Y == 0))
            {
                if (Y == 0 || IsNotBlizzard(X, Y - 1, Time + 1, _map))
                {
                    // can move up
                    yield return (GetPosition(X, Y - 1, Time + 1, _map), 1);
                }
            }
            if (Y < _height - 1 || (X == _width - 1 && Y == _height - 1))
            {
                if (Y == _height - 1 || IsNotBlizzard(X, Y + 1, Time + 1, _map))
                {
                    // can move down
                    yield return (GetPosition(X, Y + 1, Time + 1, _map), 1);
                }
            }
        }

        private static Position GetPosition(int x, int y, int time, char[,] map)
        {
            if (_allPositions.TryGetValue((x, y, time), out Position? position))
            {
                return position;
            }
            Position newPos = new(x, y, time, map);
            _allPositions.Add((x, y, time), newPos);
            return newPos;
        }

        public override bool Equals(MinHeapNode<Position>? other)
        {
            return other is Position pos && X == pos.X && Y == pos.Y && Time == pos.Time;
        }

        public static void Reset()
        {
            _allPositions.Clear();
        }
    }
}
