// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day17 : Day
{
    private static readonly Regex _validLine = new("^[.#]{1,48}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
            if (line.Length != input[0].Length)
            {
                errorMessage = "All lines in input must be the same length.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        char[,] initialSlice = input.ToCharArray2D();
        Grid3D grid3D = new(initialSlice);
        grid3D.SimulateCycles(6);
        int partOneAnswer = grid3D.CountActiveCubes();
        Grid4D grid4D = new(initialSlice);
        grid4D.SimulateCycles(6);
        int partTwoAnswer = grid4D.CountActiveCubes();

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private class Grid3D
    {
        private readonly Dictionary<(int, int, int), bool> _cubes = new();
        private readonly Dictionary<(int, int, int), int> _neighbors = new();

        public Grid3D(char[,] initialSlice)
        {
            for (int y = 0; y < initialSlice.GetLength(1); y++)
            {
                for (int x = 0; x < initialSlice.GetLength(0); x++)
                {
                    _cubes.Add((x, y, 0), initialSlice[x, y] == '#');
                }
            }
        }

        public void SimulateCycles(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                ExecuteCycle();
            }
        }

        public int CountActiveCubes()
        {
            return _cubes.Values.Count(isActive => isActive);
        }

        private void ExecuteCycle()
        {
            ClearNeighbors();
            CountNeighbors();
            UpdateActive();
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
            foreach (var kvp in _cubes)
            {
                if (kvp.Value)
                {
                    AddNeighbors(kvp.Key);
                }
            }
        }

        private void AddNeighbors((int X, int Y, int Z) cube)
        {
            for (int Δz = -1; Δz <= 1; Δz++)
            {
                for (int Δy = -1; Δy <= 1; Δy++)
                {
                    for (int Δx = -1; Δx <= 1; Δx++)
                    {
                        if (Δx == 0 && Δy == 0 && Δz == 0) { continue; }
                        AddNeighbor((cube.X + Δx, cube.Y + Δy, cube.Z + Δz));
                    }
                }
            }
        }

        private void AddNeighbor((int, int, int) position)
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

        private void UpdateActive()
        {
            foreach (var kvp in _neighbors)
            {
                (int, int, int) position = kvp.Key;
                int neighbors = kvp.Value;
                if (!_cubes.ContainsKey(position))
                {
                    _cubes.Add(position, false);
                }
                if (_cubes[position])
                {
                    if (neighbors < 2 || neighbors > 3)
                    {
                        _cubes[position] = false;
                    }
                }
                else
                {
                    if (neighbors == 3)
                    {
                        _cubes[position] = true;
                    }
                }
            }
        }
    }

    private class Grid4D
    {
        private readonly Dictionary<(int, int, int, int), bool> _cubes = new();
        private readonly Dictionary<(int, int, int, int), int> _neighbors = new();

        public Grid4D(char[,] initialSlice)
        {
            for (int y = 0; y < initialSlice.GetLength(1); y++)
            {
                for (int x = 0; x < initialSlice.GetLength(0); x++)
                {
                    _cubes.Add((x, y, 0, 0), initialSlice[x, y] == '#');
                }
            }
        }

        public void SimulateCycles(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                ExecuteCycle();
            }
        }

        public int CountActiveCubes()
        {
            return _cubes.Values.Count(isActive => isActive);
        }

        private void ExecuteCycle()
        {
            ClearNeighbors();
            CountNeighbors();
            UpdateActive();
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
            foreach (var kvp in _cubes)
            {
                if (kvp.Value)
                {
                    AddNeighbors(kvp.Key);
                }
            }
        }

        private void AddNeighbors((int X, int Y, int Z, int W) cube)
        {
            for (int Δz = -1; Δz <= 1; Δz++)
            {
                for (int Δy = -1; Δy <= 1; Δy++)
                {
                    for (int Δx = -1; Δx <= 1; Δx++)
                    {
                        for (int Δw = -1; Δw <= 1; Δw++)
                        {
                            if (Δx == 0 && Δy == 0 && Δz == 0 && Δw == 0) { continue; }
                            AddNeighbor((cube.X + Δx, cube.Y + Δy, cube.Z + Δz, cube.W + Δw));
                        }
                    }
                }
            }
        }

        private void AddNeighbor((int, int, int, int) position)
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

        private void UpdateActive()
        {
            foreach (var kvp in _neighbors)
            {
                (int, int, int, int) position = kvp.Key;
                int neighbors = kvp.Value;
                if (!_cubes.ContainsKey(position))
                {
                    _cubes.Add(position, false);
                }
                if (_cubes[position])
                {
                    if (neighbors < 2 || neighbors > 3)
                    {
                        _cubes[position] = false;
                    }
                }
                else
                {
                    if (neighbors == 3)
                    {
                        _cubes[position] = true;
                    }
                }
            }
        }
    }
}
