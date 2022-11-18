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
    internal class Day22 : Day
    {
        static readonly (int Δx, int Δy)[] adjacentDirections = new (int, int)[]
        {
            (-1,  0),
            ( 1,  0),
            ( 0, -1),
            ( 0,  1)
        };

        private readonly Regex depthLine = new Regex(@"depth: ([0-9]{1,9})");
        private readonly Regex targetLine = new Regex(@"target: ([0-9]{1,4}),([0-9]{1,4})");

        private int targetX;
        private int targetY;

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
            if (inputLines.Length < 2)
            {
                output.WriteError("Input too short.", sw);
                return output;
            }
            Match depthMatch = depthLine.Match(inputLines[0]);
            Match targetMatch = targetLine.Match(inputLines[1]);
            if (!depthMatch.Success || !targetMatch.Success)
            {
                output.WriteError("Invalid input.", sw);
                return output;
            }
            int depth = int.Parse(depthMatch.Groups[1].Value);
            targetX = int.Parse(targetMatch.Groups[1].Value);
            targetY = int.Parse(targetMatch.Groups[2].Value);

            MapArray<int> regions = MakeMap(depth);

            int partOneAnswer = TotalRiskLevel(regions);

            ConvertToBitFlags(ref regions);

            int partTwoAnswer = ShortestTimeToTarget(regions);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private MapArray<int> MakeMap(int depth)
        {
            int width = targetX + 100;
            int height = targetY + 100;
            MapArray<int> erosionLevels = new MapArray<int>(width, height);
            erosionLevels[0, 0] = depth % 20183;
            for (int x = 1; x < width; x++)
            {
                erosionLevels[x, 0] = (x * 16807 + depth) % 20183;
            }
            for (int y = 1; y < height; y++)
            {
                erosionLevels[0, y] = (y * 48271 + depth) % 20183;
            }
            for (int y = 1; y < height; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    if (x == targetX && y == targetY)
                    {
                        erosionLevels[x, y] = depth % 20183;
                        continue;
                    }
                    int geologicIndex = erosionLevels[x - 1, y] * erosionLevels[x, y - 1];
                    erosionLevels[x, y] = (geologicIndex + depth) % 20183;
                }
            }

            MapArray<int> regionTypes = new MapArray<int>(width, height, 1, 0, (0, 0));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    regionTypes[x, y] = erosionLevels[x, y] % 3;
                }
            }
            return regionTypes;
        }

        private int TotalRiskLevel(MapArray<int> regionTypes)
        {
            int riskSum = 0;
            for (int y = 0; y <= targetY; y++)
            {
                for (int x = 0; x <= targetX; x++)
                {
                    riskSum += regionTypes[x, y];
                }
            }
            return riskSum;
        }

        private void ConvertToBitFlags(ref MapArray<int> regions)
        {
            for (int y = 0; y < regions.Height; y++)
            {
                for (int x = 0; x < regions.Width; x++)
                {
                    switch (regions[x, y])
                    {
                        case 0:
                            regions[x, y] = 6;
                            break;
                        case 1:
                            regions[x, y] = 5;
                            break;
                        case 2:
                            regions[x, y] = 3;
                            break;
                    }
                }
            }
        }

        private int ShortestTimeToTarget(MapArray<int> regions)
        {
            SearchNode.Target = (targetX, targetY);
            List<SearchNode> visitedNodes = new List<SearchNode>();
            List<SearchNode> seenNodes = new List<SearchNode>
            {
                new SearchNode(0, 0, Tool.Torch, 0)
            };
            SearchNode currentNode = seenNodes[0];
            while (true)
            {
                // If there are no more nodes to search, return an error code.
                if (seenNodes.Count == 0)
                {
                    return -1;
                }

                // Select the unvisited node with the shortest tentative travel time.
                int shortestTime = int.MaxValue;
                foreach (SearchNode node in seenNodes)
                {
                    if (node.Time < shortestTime)
                    {
                        currentNode = node;
                        shortestTime = node.Time;
                    }
                }

                // If the target has been reached, return the travel time.
                if (currentNode.IsTarget())
                {
                    return currentNode.Time;
                }

                // Consider all unvisited neighbors and update their tentative times.
                foreach ((int Δx, int Δy) in adjacentDirections)
                {
                    if ((currentNode.Tool & regions[currentNode.X + Δx, currentNode.Y + Δy]) == 0)
                    {
                        continue;
                    }
                    SearchNode neighborNode = new SearchNode(currentNode.X + Δx, currentNode.Y + Δy, currentNode.Tool, currentNode.Time + 1);
                    if (visitedNodes.Contains(neighborNode))
                    {
                        continue;
                    }
                    if (!seenNodes.Contains(neighborNode))
                    {
                        seenNodes.Add(neighborNode);
                        continue;
                    }
                    for (int i = 0; i < seenNodes.Count; i++)
                    {
                        SearchNode node = seenNodes[i];
                        if (node == neighborNode)
                        {
                            node.Time = Math.Min(node.Time, currentNode.Time + 1);
                            break;
                        }
                    }
                }
                foreach (int tool in Enum.GetValues(typeof(Tool)))
                {
                    if (tool == currentNode.Tool)
                    {
                        continue;
                    }
                    if ((tool & regions[currentNode.X, currentNode.Y]) == 0)
                    {
                        continue;
                    }
                    SearchNode neighborNode = new SearchNode(currentNode.X, currentNode.Y, tool, currentNode.Time + 7);
                    if (visitedNodes.Contains(neighborNode))
                    {
                        break;
                    }
                    if (!seenNodes.Contains(neighborNode))
                    {
                        seenNodes.Add(neighborNode);
                        break;
                    }
                    for (int i = 0; i < seenNodes.Count; i++)
                    {
                        SearchNode node = seenNodes[i];
                        if (node == neighborNode)
                        {
                            node.Time = Math.Min(node.Time, currentNode.Time + 7);
                            break;
                        }
                    }
                    break;
                }

                // Mark the current node as visited.
                visitedNodes.Add(currentNode);
                seenNodes.Remove(currentNode);
            }
        }

        private struct SearchNode : IEquatable<SearchNode>
        {
            public static (int X, int Y) Target { get; set; }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Tool { get; private set; }
            public int Time { get; set; }

            public SearchNode(int x, int y, Tool tool, int time)
            {
                X = x;
                Y = y;
                Tool = (int)tool;
                Time = time;
            }

            public SearchNode(int x, int y, int tool, int time)
            {
                X = x;
                Y = y;
                Tool = tool;
                Time = time;
            }

            public bool IsTarget()
            {
                if (X == Target.X && Y == Target.Y && Tool == 2)
                {
                    return true;
                }
                return false;
            }

            bool IEquatable<SearchNode>.Equals(SearchNode other)
            {
                if (X == other.X && Y == other.Y && Tool == other.Tool)
                {
                    return true;
                }
                return false;
            }

            public static bool operator ==(SearchNode a, SearchNode b) => a.Equals(b);

            public static bool operator !=(SearchNode a, SearchNode b) => !a.Equals(b);

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private enum Tool
        {
            None = 1,
            Torch = 2,
            Gear = 4
        }
    }
}
