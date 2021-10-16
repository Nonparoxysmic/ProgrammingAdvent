// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ProgrammingAdvent2016
{
    public class Day24 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        Bitmap map;
        Dictionary<int, Coordinates> locations;

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            map = new Bitmap(inputLines[0].Length, inputLines.Length);
            locations = new Dictionary<int, Coordinates>();
            for (int y = 0; y < inputLines.Length; y++)
            {
                var line = inputLines[y];
                if (line.Length != inputLines[0].Length)
                {
                    solution.WriteSolution(1, "ERROR: Inconsistent line length in input.", 0);
                    return solution;
                }
                for (int x = 0; x < line.Length; x++)
                {
                    char character = line[x];
                    if ((x == 0 || x == line.Length - 1)
                        && (y == 0 || y == inputLines.Length - 1)
                        && character != '#')
                    {
                        solution.WriteSolution(1, "ERROR: Invalid boundary walls.", 0);
                        return solution;
                    }
                    switch (character)
                    {
                        case '#':
                            map.SetPixel(x, y, Color.Black);
                            break;
                        case '.':
                            map.SetPixel(x, y, Color.White);
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            int pointNumber = int.Parse(character.ToString());
                            if (locations.ContainsKey(pointNumber))
                            {
                                solution.WriteSolution(1, "ERROR: Duplicate locations in input.", 0);
                                return solution;
                            }
                            locations.Add(pointNumber, new Coordinates(x, y));
                            map.SetPixel(x, y, Color.Red);
                            break;
                        default:
                            solution.WriteSolution(1, "ERROR: Invalid character '" + character + "' in input.", 0);
                            return solution;
                    }
                }
            }
            if (locations.Count < 3)
            {
                solution.WriteSolution(1, "ERROR: Insufficent locations in input.", 0);
                return solution;
            }
            for (int i = 0; i < locations.Count; i++)
            {
                if (!locations.ContainsKey(i))
                {
                    solution.WriteSolution(1, "ERROR: Misnumbered locations in input.", 0);
                    return solution;
                }
            }

            int[,] distances = new int[locations.Count, locations.Count];
            for (int i = 0; i < locations.Count - 1; i++)
            {
                for (int j = i + 1; j < locations.Count; j++)
                {
                    var steps = NumberOfSteps(locations[i].x, locations[i].y, locations[j].x, locations[j].y);
                    distances[i, j] = steps;
                }
            }

            stopwatch.Reset();
            return solution;
        }

        bool IsWall(int x, int y)
        {
            if (map.GetPixel(x, y).R == 0
                && map.GetPixel(x, y).G == 0
                && map.GetPixel(x, y).B == 0)
            {
                return true;
            }
            return false;
        }

        int NumberOfSteps(int startX, int startY, int goalX, int goalY)
        {
            // Initialize starting node.
            var allNodes = new Dictionary<string, MazeNode>();
            var startingNode = new MazeNode(startX, startY)
            {
                stepsFromStart = 0,
                distanceToEnd = Math.Abs(startX - goalX) + Math.Abs(startY - goalY)
            };
            startingNode.score = startingNode.stepsFromStart + startingNode.distanceToEnd;
            allNodes.Add(startingNode.coords, startingNode);
            // Initialize node lists.
            var openList = new List<string>();
            var closedList = new List<string>();
            openList.Add(startingNode.coords);
            // While there are nodes to be analyzed...
            while (openList.Count > 0)
            {
                // Get a node on the open list with the lowest score
                MazeNode currentNode = null;
                int lowestScore = int.MaxValue;
                foreach (var nodeKey in openList)
                {
                    var node = allNodes[nodeKey];
                    if (node.score < lowestScore)
                    {
                        lowestScore = node.score;
                        currentNode = node;
                    }
                }
                // If the current node is the goal node, return the path length.
                if (currentNode.x == goalX && currentNode.y == goalY)
                {
                    return currentNode.stepsFromStart;
                }
                // Move current node from open list to closed list.
                openList.Remove(currentNode.coords);
                closedList.Add(currentNode.coords);
                // Make a list of the neighbors of the current node.
                var neighborList = new List<MazeNode>();
                if (!IsWall(currentNode.x - 1, currentNode.y))
                {
                    string keyMinusX = PathNode.CoordsToString(currentNode.x - 1, currentNode.y);
                    if (!allNodes.ContainsKey(keyMinusX))
                    {
                        allNodes.Add(keyMinusX, new MazeNode(currentNode.x - 1, currentNode.y));
                    }
                    neighborList.Add(allNodes[keyMinusX]);
                }
                if (!IsWall(currentNode.x, currentNode.y - 1))
                {
                    string keyMinusY = PathNode.CoordsToString(currentNode.x, currentNode.y - 1);
                    if (!allNodes.ContainsKey(keyMinusY))
                    {
                        allNodes.Add(keyMinusY, new MazeNode(currentNode.x, currentNode.y - 1));
                    }
                    neighborList.Add(allNodes[keyMinusY]);
                }
                if (!IsWall(currentNode.x + 1, currentNode.y))
                {
                    string keyPlusX = PathNode.CoordsToString(currentNode.x + 1, currentNode.y);
                    if (!allNodes.ContainsKey(keyPlusX))
                    {
                        allNodes.Add(keyPlusX, new MazeNode(currentNode.x + 1, currentNode.y));
                    }
                    neighborList.Add(allNodes[keyPlusX]);
                }
                if (!IsWall(currentNode.x, currentNode.y + 1))
                {
                    string keyPlusY = PathNode.CoordsToString(currentNode.x, currentNode.y + 1);
                    if (!allNodes.ContainsKey(keyPlusY))
                    {
                        allNodes.Add(keyPlusY, new MazeNode(currentNode.x, currentNode.y + 1));
                    }
                    neighborList.Add(allNodes[keyPlusY]);
                }
                // For each neighbor node...
                foreach (var neighbor in neighborList)
                {
                    // If the neighbor node has already been fully analyzed, skip it.
                    if (closedList.Contains(neighbor.coords)) continue;
                    // If the neighbor node is not on the open list, or if the path through the current node
                    // is shorter than the neighbor node's previously considered path...
                    if (!openList.Contains(neighbor.coords)
                        || currentNode.stepsFromStart + 1 < neighbor.stepsFromStart)
                    {
                        // Recalculate the neighbor node's values.
                        neighbor.stepsFromStart = currentNode.stepsFromStart + 1;
                        neighbor.distanceToEnd = Math.Abs(neighbor.x - goalX) + Math.Abs(neighbor.y - goalY);
                        neighbor.score = neighbor.stepsFromStart + neighbor.distanceToEnd;
                        // Add the neighbor to the open list if it hasn't been added yet.
                        if (!openList.Contains(neighbor.coords)) openList.Add(neighbor.coords);
                    }
                }
            }
            // The goal node was not found, so there is no path.
            return int.MaxValue;
        }
    }

    class Coordinates
    {
        public int x;
        public int y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class MazeNode
    {
        public int x;
        public int y;
        public string coords;
        public int stepsFromStart;
        public int distanceToEnd;
        public int score;

        public MazeNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            coords = CoordsToString(x, y);
            stepsFromStart = int.MaxValue;
            distanceToEnd = int.MaxValue;
            score = int.MaxValue;
        }

        public static string CoordsToString(int x, int y)
        {
            return x.ToString() + "," + y.ToString();
        }
    }
}
