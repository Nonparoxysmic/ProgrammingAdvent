// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day24 : Day
{
    //  Coordinate System:
    //
    //           (-r)
    //        NW      NE
    //  (+s)    \    /    (+q)
    //           \  /
    //     W ----    ---- E
    //           /  \
    //  (-q)    /    \    (-s)
    //        SW      SE
    //           (+r)
    //
    //  q + r + s = 0
    //
    //  https://www.redblobgames.com/grids/hexagons/

    private static readonly Dictionary<string, Vector2Int> _directions = new()
    {
        //                       q   r
        {  "e",  new Vector2Int( 1,  0) },
        { "se",  new Vector2Int( 0,  1) },
        { "sw",  new Vector2Int(-1,  1) },
        {  "w",  new Vector2Int(-1,  0) },
        { "nw",  new Vector2Int( 0, -1) },
        { "ne",  new Vector2Int( 1, -1) }
    };

    private static readonly Regex _validLine = new("^(e|se|sw|w|nw|ne)+$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                break;
            }
            if (line.Length > 64)
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" is too long.";
                return false;
            }
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        HashSet<Vector2Int> blackTiles = new();
        foreach (string line in input)
        {
            Match match = _validLine.Match(line);
            if (match.Success)
            {
                IEnumerable<string> directions = match.Groups[1].Captures.Select(c => c.Value);
                Vector2Int position = Vector2Int.Zero;
                foreach (string direction in directions)
                {
                    position += _directions[direction];
                }
                if (blackTiles.Contains(position))
                {
                    blackTiles.Remove(position);
                }
                else
                {
                    blackTiles.Add(position);
                }
            }
        }

        HexGrid grid = new(blackTiles);
        grid.ExecuteProcess(100);
        int partTwoAnswer = grid.CountBlackTiles();

        return output.WriteAnswers(blackTiles.Count, partTwoAnswer);
    }

    private class HexGrid
    {
        private readonly Dictionary<(int, int), bool> _tiles = new();
        private readonly Dictionary<(int, int), int> _neighbors = new();

        public HexGrid(HashSet<Vector2Int> blackTiles)
        {
            foreach (Vector2Int tile in blackTiles)
            {
                _tiles.Add((tile.X, tile.Y), true);
                _neighbors.Add((tile.X, tile.Y), 0);
            }
        }

        public void ExecuteProcess(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                ExecuteCycle();
            }
        }

        public int CountBlackTiles()
        {
            return _tiles.Values.Count(isBlack => isBlack);
        }

        private void ExecuteCycle()
        {
            ClearNeighbors();
            CountNeighbors();
            UpdateTiles();
        }

        private void ClearNeighbors()
        {
            foreach (var key in _neighbors.Keys)
            {
                _neighbors[key] = 0;
            }
        }

        private void CountNeighbors()
        {
            foreach (var kvp in _tiles)
            {
                if (kvp.Value)
                {
                    AddNeighbors(kvp.Key);
                }
            }
        }

        private void AddNeighbors((int Q, int R) tile)
        {
            foreach (Vector2Int step in _directions.Values)
            {
                AddNeighbor((tile.Q + step.X, tile.R + step.Y));
            }
        }

        private void AddNeighbor((int, int) position)
        {
            if (!_neighbors.ContainsKey(position))
            {
                _neighbors.Add(position, 1);
            }
            else
            {
                _neighbors[position]++;
            }
        }

        private void UpdateTiles()
        {
            foreach (var kvp in _neighbors)
            {
                (int, int) position = kvp.Key;
                int neighbors = kvp.Value;
                if (!_tiles.ContainsKey(position))
                {
                    _tiles.Add(position, false);
                }
                if (_tiles[position])
                {
                    // Black Tile
                    if (neighbors == 0 || neighbors > 2)
                    {
                        _tiles[position] = false;
                    }
                }
                else
                {
                    // White Tile
                    if (neighbors == 2)
                    {
                        _tiles[position] = true;
                    }
                }
            }
        }
    }
}
