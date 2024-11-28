// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day23 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        ElfCollection elves = new(input);
        for (int i = 0; i < 10; i++)
        {
            elves.Move();
        }
        int round10GroundReached = elves.BoundingArea() - elves.Count;

        return result.WriteAnswers(round10GroundReached, null);
    }

    private class ElfCollection
    {
        public int Count { get; }
        public int Round { get; private set; }

        private readonly int[] _directions = [3, 1, 2, 0];
        private readonly int[] _adjacency = new int[4];

        private readonly int[,] _map;
        private readonly Vector2Int[] _elves;

        public ElfCollection(string[] input)
        {
            _map = new int[150, 150];
            List<Vector2Int> elves = [];
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        elves.Add(new Vector2Int(x + 20, y + 20));
                        _map[x + 20, y + 20] = 1;
                    }
                }
            }
            _elves = [.. elves];
            Count = elves.Count;
            Round = 1;
        }

        public void Move()
        {
            List<Vector2Int> stationaryElves = [];
            for (int e = 0; e < Count; e++)
            {
                Vector2Int position = _elves[e];
                int adjacency = CalculateAdjacency(position);
                if (adjacency == 4 || adjacency == 0)
                {
                    stationaryElves.Add(position);
                }
                else
                {
                    foreach (int direction in _directions)
                    {
                        if (_adjacency[direction] == 0)
                        {
                            Vector2Int proposedMove = position + DirectionVector(direction);
                            if (_map[proposedMove.X, proposedMove.Y] == Round + 1)
                            {
                                Vector2Int collisionSource = proposedMove + DirectionVector(direction);
                                _map[proposedMove.X, proposedMove.Y] = 0;
                                stationaryElves.Add(collisionSource);
                                stationaryElves.Add(position);
                                for (int i = e - 1; i >= 0; i--)
                                {
                                    if (_elves[i] == proposedMove)
                                    {
                                        _elves[i] = collisionSource;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                _elves[e] = proposedMove;
                                _map[proposedMove.X, proposedMove.Y] = Round + 1;
                            }
                            break;
                        }
                    }
                }
            }
            foreach (Vector2Int notMoving in stationaryElves)
            {
                _map[notMoving.X, notMoving.Y] = Round + 1;
            }
            RotateDirectionPriority();
            Round++;
        }

        public int BoundingArea()
        {
            int xMin = int.MaxValue, yMin = int.MaxValue, xMax = int.MinValue, yMax = int.MinValue;
            foreach (Vector2Int position in _elves)
            {
                xMin = Math.Min(xMin, position.X);
                yMin = Math.Min(yMin, position.Y);
                xMax = Math.Max(xMax, position.X);
                yMax = Math.Max(yMax, position.Y);
            }
            return (xMax - xMin + 1) * (yMax - yMin + 1);
        }

        private int CalculateAdjacency(Vector2Int position)
        {
            _adjacency[0] = 0;
            _adjacency[1] = 0;
            _adjacency[2] = 0;
            _adjacency[3] = 0;
            if (_map[position.X - 1, position.Y - 1] == Round)
            {
                _adjacency[2]++;
                _adjacency[3]++;
            }
            if (_map[position.X, position.Y - 1] == Round)
            {
                _adjacency[3]++;
            }
            if (_map[position.X + 1, position.Y - 1] == Round)
            {
                _adjacency[0]++;
                _adjacency[3]++;
            }
            if (_map[position.X - 1, position.Y] == Round)
            {
                _adjacency[2]++;
            }
            if (_map[position.X + 1, position.Y] == Round)
            {
                _adjacency[0]++;
            }
            if (_map[position.X - 1, position.Y + 1] == Round)
            {
                _adjacency[1]++;
                _adjacency[2]++;
            }
            if (_map[position.X, position.Y + 1] == Round)
            {
                _adjacency[1]++;
            }
            if (_map[position.X + 1, position.Y + 1] == Round)
            {
                _adjacency[0]++;
                _adjacency[1]++;
            }
            return Math.Sign(_adjacency[0]) + Math.Sign(_adjacency[1])
                + Math.Sign(_adjacency[2]) + Math.Sign(_adjacency[3]);
        }

        private static Vector2Int DirectionVector(int direction)
        {
            return direction switch
            {
                0 => Vector2Int.UnitX,
                1 => Vector2Int.UnitY,
                2 => -Vector2Int.UnitX,
                3 => -Vector2Int.UnitY,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        private void RotateDirectionPriority()
        {
            int temp = _directions[0];
            _directions[0] = _directions[1];
            _directions[1] = _directions[2];
            _directions[2] = _directions[3];
            _directions[3] = temp;
        }
    }
}
