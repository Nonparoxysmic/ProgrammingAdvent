// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day13 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        static int inputNumber;

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            if (!int.TryParse(input, out inputNumber))
            {
                solution.WriteSolution(1, "ERROR: Cannot parse input as an integer.", 0);
                return solution;
            }
            stopwatch.Start();

            int partOneSolution = GetPathLength(1, 1, 31, 39);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        // Pathfinding code adapted from one of my previous projects.
        int GetPathLength(int startX, int startY, int goalX, int goalY)
        {
            // Ensure that input parameters are valid.
            if (startX < 0 || startY < 0 || goalX < 0 || goalY < 0)
            {
                throw new ArgumentOutOfRangeException("Out of bounds coordinates.");
            }
            if (!CoordinateIsOpenSpace(startX, startY) || !CoordinateIsOpenSpace(goalX, goalY))
            {
                throw new ArgumentException("Invalid coordinates.");
            }
            if (startX == goalX && startY == goalY) return 0;

            // A* Pathfinding

            // Initialize starting node.
            Dictionary<string, PathNode> allNodes = new Dictionary<string, PathNode>();
            PathNode startingNode = new PathNode(startX, startY)
            {
                stepsFromStart = 0,
                distanceToEnd = Math.Abs(startX - goalX) + Math.Abs(startY - goalY)
            };
            startingNode.score = startingNode.stepsFromStart + startingNode.distanceToEnd;
            allNodes.Add(startingNode.coords, startingNode);
            // Initialize node lists.
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();
            openList.Add(startingNode);
            // While there are nodes to be analyzed...
            while (openList.Count > 0)
            {
                // Get a node on the open list with the lowest score
                PathNode currentNode = null;
                int lowestScore = int.MaxValue;
                foreach (PathNode node in openList)
                {
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
                openList.Remove(currentNode);
                closedList.Add(currentNode);
                // Make a list of the neighbors of the current node.
                List<PathNode> neighborList = new List<PathNode>();
                if (currentNode.x > 0)
                {
                    string keyMinusX = PathNode.CoordsToString(currentNode.x - 1, currentNode.y);
                    if (!allNodes.ContainsKey(keyMinusX))
                    {
                        allNodes.Add(keyMinusX, new PathNode(currentNode.x - 1, currentNode.y));
                    }
                    neighborList.Add(allNodes[keyMinusX]);
                }
                if (currentNode.y > 0)
                {
                    string keyMinusY = PathNode.CoordsToString(currentNode.x, currentNode.y - 1);
                    if (!allNodes.ContainsKey(keyMinusY))
                    {
                        allNodes.Add(keyMinusY, new PathNode(currentNode.x, currentNode.y - 1));
                    }
                    neighborList.Add(allNodes[keyMinusY]);
                }
                string keyPlusX = PathNode.CoordsToString(currentNode.x + 1, currentNode.y);
                if (!allNodes.ContainsKey(keyPlusX))
                {
                    allNodes.Add(keyPlusX, new PathNode(currentNode.x + 1, currentNode.y));
                }
                neighborList.Add(allNodes[keyPlusX]);
                string keyPlusY = PathNode.CoordsToString(currentNode.x, currentNode.y + 1);
                if (!allNodes.ContainsKey(keyPlusY))
                {
                    allNodes.Add(keyPlusY, new PathNode(currentNode.x, currentNode.y + 1));
                }
                neighborList.Add(allNodes[keyPlusY]);
                // For each neighbor node...
                foreach (PathNode neighbor in neighborList)
                {
                    // If the neighbor node is not a path, or if it has already been fully analyzed, skip it.
                    if (!neighbor.isPath || closedList.Contains(neighbor)) continue;
                    // If the neighbor node is not on the open list, or if the path through the current node
                    // is shorter than the neighbor node's previously considered path...
                    if (!openList.Contains(neighbor) || currentNode.stepsFromStart + 1 < neighbor.stepsFromStart)
                    {
                        // Recalculate the neighbor node's values.
                        neighbor.stepsFromStart = currentNode.stepsFromStart + 1;
                        neighbor.distanceToEnd = Math.Abs(neighbor.x - goalX) + Math.Abs(neighbor.y - goalY);
                        neighbor.score = neighbor.stepsFromStart + neighbor.distanceToEnd;
                        // Add the neighbor to the open list if it hasn't been added yet.
                        if (!openList.Contains(neighbor)) openList.Add(neighbor);
                    }
                }
            }
            // The goal node was not found, so there is no path.
            return int.MaxValue;
        }

        public static bool CoordinateIsOpenSpace(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            int sum = x * x + 3 * x + 2 * x * y + y + y * y + inputNumber;
            bool output = true;
            for (int i = 0; i < 32; i++)
            {
                if ((sum >> i & 1) == 1) output = !output;
            }
            return output;
        }
    }

    class PathNode
    {
        public int x;
        public int y;
        public string coords;
        public bool isPath;
        public int stepsFromStart;
        public int distanceToEnd;
        public int score;

        public PathNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            coords = CoordsToString(x, y);
            isPath = Day13.CoordinateIsOpenSpace(x, y);
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
