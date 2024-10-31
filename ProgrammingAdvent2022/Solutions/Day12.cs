// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day12 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        char[,] map = InputToMap(input, out (int, int) start, out (int, int) end);
        int fewestSteps = FewestStepsInReverse(map, end, start);
        int betterFewestSteps = FewestStepsDown(map, end);

        return result.WriteAnswers(fewestSteps, betterFewestSteps);
    }

    private static char[,] InputToMap(string[] input, out (int, int) start, out (int, int) end)
    {
        start = (0, 0);
        end = (0, 0);
        int width = input[0].Length;
        int height = input.Length;
        char[,] output = new char[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                output[x, y] = input[y][x];
                if (output[x, y] == 'S')
                {
                    start = (x, y);
                    output[x, y] = 'a';
                }
                if (output[x, y] == 'E')
                {
                    end = (x, y);
                    output[x, y] = 'z';
                }
            }
        }
        return output;
    }

    private static int FewestStepsInReverse(char[,] map, (int, int) end, (int, int) start)
    {
        Position.Reset();
        MinHeap<Position> open = new();
        Position initial = new(end, map)
        {
            CostFromStart = 0,
            EstimatedCostToGoal = Distance(start, end)
        };
        open.Insert(initial);
        while (open.Count > 0)
        {
            Position current = open.FindMin();
            if ((current.X, current.Y) == start)
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
                        neighbor.EstimatedCostToGoal = Distance(neighbor.Coords, start);
                        open.Insert(neighbor);
                    }
                }
            }
        }
        return -1;
    }

    private static int FewestStepsDown(char[,] map, (int, int) end)
    {
        Position.Reset();
        MinHeap<Position> open = new();
        Position initial = new(end, map)
        {
            CostFromStart = 0,
        };
        open.Insert(initial);
        while (open.Count > 0)
        {
            Position current = open.FindMin();
            if (map[current.X, current.Y] == 'a')
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

    private class Position((int, int) position, char[,] map) : MinHeapNode<Position>
    {
        private static readonly Dictionary<(int, int), Position> _allPositions = [];

        public int X { get; } = position.Item1;
        public int Y { get; } = position.Item2;
        public (int, int) Coords { get; } = (position.Item1, position.Item2);

        private readonly char[,] _map = map;

        public override IEnumerable<(Position, int)> NeighborsAndCosts()
        {
            if (X > 0 && _map[X - 1, Y] >= _map[X, Y] - 1)
            {
                yield return (GetPosition(X - 1, Y, _map), 1);
            }
            if (Y > 0 && _map[X, Y - 1] >= _map[X, Y] - 1)
            {
                yield return (GetPosition(X, Y - 1, _map), 1);
            }
            if (X < _map.GetLength(0) - 1 && _map[X + 1, Y] >= _map[X, Y] - 1)
            {
                yield return (GetPosition(X + 1, Y, _map), 1);
            }
            if (Y < _map.GetLength(1) - 1 && _map[X, Y + 1] >= _map[X, Y] - 1)
            {
                yield return (GetPosition(X, Y + 1, _map), 1);
            }
        }

        private static Position GetPosition(int x, int y, char[,] map)
        {
            if (_allPositions.TryGetValue((x, y), out Position? position))
            {
                return position;
            }
            Position newPos = new((x, y), map);
            _allPositions.Add((x, y), newPos);
            return newPos;
        }

        public override bool Equals(MinHeapNode<Position>? other)
        {
            return other is Position pos && X == pos.X && Y == pos.Y;
        }

        public static void Reset()
        {
            _allPositions.Clear();
        }
    }
}
