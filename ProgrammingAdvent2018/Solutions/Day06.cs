// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day06 : Day
    {
        private readonly Regex validLine = new Regex(@"^(\d+), (\d+)$");

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

            GridArray grid = new GridArray(lowestX, lowestY, highestX, highestY, coordinates);

            int largestFiniteArea = grid.LargestFiniteArea();

            int locationsInRegion = grid.LocationsInRegion(10_000);

            sw.Stop();
            output.WriteAnswers(largestFiniteArea, locationsInRegion, sw);
            return output;
        }

        class GridArray
        {
            public int NumberOfPoints { get; private set; }

            private readonly GridCoord[,] coords;
            private readonly int lengthX;
            private readonly int lengthY;

            public GridArray(int lowestX, int lowestY, int highestX, int highestY, Vector2Int[] coordinates)
            {
                NumberOfPoints = coordinates.Length;
                if (highestX < lowestX || highestY < lowestY)
                {
                    throw new ArgumentOutOfRangeException();
                }
                lengthX = highestX - lowestX + 1;
                lengthY = highestY - lowestY + 1;
                coords = new GridCoord[lengthX, lengthY];
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        coords[x, y] = new GridCoord(x + lowestX, y + lowestY, coordinates);
                    }
                }
            }

            public int LargestFiniteArea()
            {
                Dictionary<int, int> areas = new Dictionary<int, int>();
                HashSet<int> indices = InternalIndices(NumberOfPoints);
                foreach (int i in indices)
                {
                    areas.Add(i, 0);
                }
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        int i = coords[x, y].NearestPoint;
                        if (areas.ContainsKey(i))
                        {
                            areas[i]++;
                        }
                    }
                }
                return areas.Values.Max();
            }

            public int LocationsInRegion(int sumLimit)
            {
                int sum = 0;
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        if (coords[x, y].SumOfDistances < sumLimit)
                        {
                            sum++;
                        }
                    }
                }
                return sum;
            }

            public HashSet<int> InternalIndices(int numberOfPoints)
            {
                HashSet<int> indices = new HashSet<int>();
                for (int i = 0; i < numberOfPoints; i++)
                {
                    indices.Add(i);
                }
                for (int y = 0; y < lengthY; y += lengthY - 1)
                {
                    for (int x = 0; x < lengthX; x++)
                    {
                        indices.Remove(coords[x, y].NearestPoint);
                    }
                }
                for (int y = 0; y < lengthY; y++)
                {
                    for (int x = 0; x < lengthX; x += lengthX - 1)
                    {
                        indices.Remove(coords[x, y].NearestPoint);
                    }
                }
                return indices;
            }
        }

        class GridCoord
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int NearestPoint { get; private set; }
            public long SumOfDistances { get; private set; }

            private readonly int[] distances;

            public GridCoord(int x, int y, Vector2Int[] coordinates)
            {
                X = x;
                Y = y;
                distances = new int[coordinates.Length];
                for (int i = 0; i < coordinates.Length; i++)
                {
                    distances[i] = Math.Abs(coordinates[i].X - X) + Math.Abs(coordinates[i].Y - Y);
                }
                SumOfDistances = distances.Sum();
                int shortestDistance = distances.Min();
                bool shortestFound = false;
                for (int i = 0; i < distances.Length; i++)
                {
                    if (distances[i] == shortestDistance)
                    {
                        if (shortestFound)
                        {
                            NearestPoint = -1;
                            break;
                        }
                        else
                        {
                            NearestPoint = i;
                            shortestFound = true;
                        }
                    }
                }
            }
        }
    }
}
