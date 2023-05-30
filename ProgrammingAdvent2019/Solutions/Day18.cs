﻿// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day18 : Day
{
    private static readonly Regex _validCharacters = new("^[a-zA-Z@.#]+$");
    private static readonly Vector2Int[] _steps = new Vector2Int[]
    {
        new Vector2Int(-1,  0),
        new Vector2Int( 0, -1),
        new Vector2Int( 1,  0),
        new Vector2Int( 0,  1)
    };

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        HashSet<char> keys = new();
        bool entranceFound = false;
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (line.Length != inputLines[0].Length)
            {
                errorMessage = "Input lines are not all the same length.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
            foreach (char c in line)
            {
                if ('a' <= c && c <= 'z')
                {
                    if (!keys.Add(c))
                    {
                        errorMessage = $"Input contains duplicate key '{c}'.";
                        return false;
                    }
                }
                if (c == '@')
                {
                    if (entranceFound)
                    {
                        errorMessage = "Input contains multiple entrances.";
                        return false;
                    }
                    else
                    {
                        entranceFound = true;
                    }
                }
            }
        }
        if (!entranceFound)
        {
            errorMessage = "Input contains no entrance.";
            return false;
        }
        if (keys.Count == 0)
        {
            errorMessage = "Input does not contain any keys.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        (char[,] map, char[] keys, Vector2Int entrance) = ProcessInput(inputLines);

        MazeNode entranceNode = BuildGraph(map, entrance);

        return output.WriteAnswers(null, null);
    }

    private static (char[,], char[], Vector2Int) ProcessInput(string[] input)
    {
        char[,] map = new char[input[0].Length, input.Length];
        List<char> keys = new();
        int entranceX = -1;
        int entranceY = -1;
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                map[x, y] = input[y][x];
                if (map[x, y] == '@')
                {
                    entranceX = x;
                    entranceY = y;
                }
                if ('a' <= map[x, y] && map[x, y] <= 'z')
                {
                    keys.Add(map[x, y]);
                }
            }
        }
        PruneDeadEnds(map);
        keys.Sort();
        return (map, keys.ToArray(), new Vector2Int(entranceX, entranceY));
    }

    private static void PruneDeadEnds(char[,] map)
    {
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                PruneDeadEnd(map, x, y);
            }
        }
    }

    private static void PruneDeadEnd(char[,] map, int x, int y)
    {
        if (('a' <= map[x, y] && map[x, y] <= 'z') || map[x, y] == '@')
        {
            return;
        }
        int openNeighbors = 0;
        int openX = -1;
        int openY = -1;
        foreach (Vector2Int step in _steps)
        {
            if (map[x + step.X, y + step.Y] != '#')
            {
                openNeighbors++;
                openX = x + step.X;
                openY = y + step.Y;
            }
        }
        if (openNeighbors == 0)
        {
            map[x, y] = '#';
            return;
        }
        if (openNeighbors == 1)
        {
            map[x, y] = '#';
            PruneDeadEnd(map, openX, openY);
        }
    }

    private static MazeNode BuildGraph(char[,] map, Vector2Int entrance)
    {
        MazeNode root = new(entrance.X, entrance.Y, '@');

        if (map[entrance.X - 1, entrance.Y - 1] == '.'
            && map[entrance.X, entrance.Y - 1] == '.'
            && map[entrance.X + 1, entrance.Y - 1] == '.'
            && map[entrance.X - 1, entrance.Y] == '.'
            && map[entrance.X + 1, entrance.Y] == '.'
            && map[entrance.X - 1, entrance.Y + 1] == '.'
            && map[entrance.X, entrance.Y + 1] == '.'
            && map[entrance.X + 1, entrance.Y + 1] == '.')
        {
            // Actual input can be divided into four quadrants.
            map[entrance.X - 1, entrance.Y - 1] = '@';
            map[entrance.X, entrance.Y - 1] = '#';
            map[entrance.X + 1, entrance.Y - 1] = '@';
            map[entrance.X - 1, entrance.Y] = '#';
            map[entrance.X, entrance.Y] = '#';
            map[entrance.X + 1, entrance.Y] = '#';
            map[entrance.X - 1, entrance.Y + 1] = '@';
            map[entrance.X, entrance.Y + 1] = '#';
            map[entrance.X + 1, entrance.Y + 1] = '@';

            root.CreateEntranceNeighbors();
            foreach (MazeNode neighbor in root.Neighbors)
            {
                MapToNodes(map, neighbor);
            }
        }
        else
        {
            // Example inputs do not divide into quadrants.
            MapToNodes(map, root);
        }

        return root;
    }

    private static void MapToNodes(char[,] map, MazeNode node)
    {
        map[node.X, node.Y] = '#';
        for (int direction = 0; direction < 4; direction++)
        {
            Vector2Int step = _steps[direction];
            if (map[node.X + step.X, node.Y + step.Y] == '#')
            {
                continue;
            }
            // TODO: Get the node(s) in that direction.
        }
    }

    private class MazeNode
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Feature { get; private set; }

        public readonly HashSet<MazeNode> Neighbors = new();

        public MazeNode(int x, int y, char feature)
        {
            X = x;
            Y = y;
            Feature = feature;
        }

        public MazeNode CreateNeighbor(int x, int y, int distance, List<char> doors, char feature = '.')
        {
            MazeNode neighbor = new(x, y, feature);
            Neighbors.Add(neighbor);
            neighbor.Neighbors.Add(this);
            return neighbor;
        }

        public static void ConnectNodes(MazeNode A, MazeNode B)
        {
            A.Neighbors.Add(B);
            B.Neighbors.Add(A);
        }

        public void CreateEntranceNeighbors()
        {
            MazeNode NW = CreateNeighbor(X - 1, Y - 1, 2, new List<char>());
            MazeNode NE = CreateNeighbor(X + 1, Y - 1, 2, new List<char>());
            MazeNode SW = CreateNeighbor(X - 1, Y + 1, 2, new List<char>());
            MazeNode SE = CreateNeighbor(X + 1, Y + 1, 2, new List<char>());
            ConnectNodes(NW, NE);
            ConnectNodes(NW, SW);
            ConnectNodes(NE, SE);
            ConnectNodes(SW, SE);
        }
    }
}
