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
    internal class Day06 : Day
    {
        private readonly Regex validLine = new Regex(@"^(\d+), (\d+)$");

        private readonly Vector2Int[] adjacentDirections = new Vector2Int[]
        {
            new Vector2Int(-1,  0),
            new Vector2Int( 0, -1),
            new Vector2Int( 1,  0),
            new Vector2Int( 0,  1)
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

            List<Vector2Int> coordList = new List<Vector2Int>();
            int highestX = int.MinValue;
            int highestY = int.MinValue;
            int lowestX = int.MaxValue;
            int lowestY = int.MaxValue;
            foreach (string line in inputLines)
            {
                Match validMatch = validLine.Match(line);
                if (!validMatch.Success)
                {
                    output.WriteError($"Invalid line \"{line}\" in input.", sw);
                    return output;
                }
                int x = int.Parse(validMatch.Groups[1].Value);
                int y = int.Parse(validMatch.Groups[2].Value);
                coordList.Add(new Vector2Int(x, y));
                highestX = Math.Max(highestX, x);
                highestY = Math.Max(highestY, y);
                lowestX = Math.Min(lowestX, x);
                lowestY = Math.Min(lowestY, y);
            }
            Vector2Int[] coordinates = coordList.ToArray();

            GridArray grid = new GridArray(lowestX, lowestY, highestX, highestY);
            int coordNumber = 1;
            foreach (Vector2Int coord in coordinates)
            {
                grid[coord.X, coord.Y] = coordNumber++;
            }
            grid.Apply();

            bool hasUndeterminedNodes = true;
            while (hasUndeterminedNodes)
            {
                hasUndeterminedNodes = false;
                for (int y = lowestY; y <= highestY; y++)
                {
                    for (int x = lowestX; x <= highestX; x++)
                    {
                        if (grid[x, y] != 0) { continue; }
                        hasUndeterminedNodes = true;
                        int adjacentValues = 0;
                        foreach (Vector2Int dir in adjacentDirections)
                        {
                            if (grid[x + dir.X, y + dir.Y] > 0)
                            {
                                if (adjacentValues == 0)
                                {
                                    adjacentValues = grid[x + dir.X, y + dir.Y];
                                }
                                else if (grid[x + dir.X, y + dir.Y] != adjacentValues)
                                {
                                    adjacentValues = -1;
                                }
                            }
                            if (grid[x + dir.X, y + dir.Y] < 0)
                            {
                                adjacentValues = -1;
                            }
                        }
                        grid[x, y] = adjacentValues;
                    }
                }
                grid.Apply();
            }

            HashSet<int> indices = grid.InternalIndices();
            Dictionary<int, int> areas = new Dictionary<int, int>();
            foreach (int i in indices)
            {
                areas.Add(i, 0);
            }
            for (int y = lowestY; y <= highestY; y++)
            {
                for (int x = lowestX; x <= highestX; x++)
                {
                    if (areas.ContainsKey(grid[x, y]))
                    {
                        areas[grid[x, y]]++;
                    }
                }
            }

            int largestNoninfiniteArea = 0;
            foreach (var kvp in areas)
            {
                largestNoninfiniteArea = Math.Max(largestNoninfiniteArea, kvp.Value);
            }

            sw.Stop();
            output.WriteAnswers(largestNoninfiniteArea, null, sw);
            return output;
        }

        class GridArray
        {
            private readonly int[,] values;
            private readonly int[,] buffer;
            private readonly int lengthX;
            private readonly int lengthY;
            private readonly int shiftX;
            private readonly int shiftY;

            public GridArray(int lowestX, int lowestY, int highestX, int highestY)
            {
                if (highestX < lowestX || highestY < lowestY)
                {
                    throw new ArgumentOutOfRangeException();
                }
                lengthX = highestX - lowestX + 1;
                lengthY = highestY - lowestY + 1;
                values = new int[lengthX, lengthY];
                buffer = new int[lengthX, lengthY];
                shiftX = lowestX;
                shiftY = lowestY;
            }

            public int this[int x, int y]
            {
                get { return GetValue(x, y); }
                set { SetValue(x, y, value); }
            }

            private int GetValue(int x, int y)
            {
                int xIndex = x - shiftX;
                int yIndex = y - shiftY;
                if (xIndex < 0 || yIndex < 0 || xIndex >= lengthX || yIndex >= lengthY)
                {
                    return 0;
                }
                return values[xIndex, yIndex];
            }

            private void SetValue(int x, int y, int value)
            {
                int xIndex = x - shiftX;
                int yIndex = y - shiftY;
                if (xIndex < 0 || yIndex < 0 || xIndex >= lengthX || yIndex >= lengthY)
                {
                    return;
                }
                buffer[xIndex, yIndex] = value;
            }

            public void Apply()
            {
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        values[x, y] = buffer[x, y];
                    }
                }
            }

            public HashSet<int> InternalIndices()
            {
                HashSet<int> indices = new HashSet<int>();
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        indices.Add(values[x, y]);
                    }
                }
                indices.Remove(-1);
                for (int y = 0; y < lengthY; y += lengthY - 1)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        indices.Remove(values[x, y]);
                    }
                }
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x += lengthX - 1)
                    {
                        indices.Remove(values[x, y]);
                    }
                }
                return indices;
            }
        }
    }
}
