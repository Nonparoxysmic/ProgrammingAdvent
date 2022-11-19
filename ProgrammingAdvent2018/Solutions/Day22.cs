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
        public static (int X, int Y) Target { get; private set; }

        static readonly (int Δx, int Δy)[] adjacentDirections = new (int, int)[]
        {
            (-1,  0),
            ( 1,  0),
            ( 0, -1),
            ( 0,  1)
        };

        private readonly Regex depthLine = new Regex(@"depth: ([0-9]{1,9})");
        private readonly Regex targetLine = new Regex(@"target: ([0-9]{1,4}),([0-9]{1,4})");

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
            int targetX = int.Parse(targetMatch.Groups[1].Value);
            int targetY = int.Parse(targetMatch.Groups[2].Value);
            Target = (targetX, targetY);

            MapArray<int> regions = MakeMap(depth);

            int partOneAnswer = TotalRiskLevel(regions);

            int partTwoAnswer = ShortestTimeToTarget(regions);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private MapArray<int> MakeMap(int depth)
        {
            int width = Target.X + 100;
            int height = Target.Y + 100;
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
                    if (x == Target.X && y == Target.Y)
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
            for (int y = 0; y <= Target.Y; y++)
            {
                for (int x = 0; x <= Target.X; x++)
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
            ConvertToBitFlags(ref regions);

            SearchNode currentNode = new SearchNode(0, 0, Tool.Torch, 0);
            HashSet<SearchNode> openSet = new HashSet<SearchNode>
            {
                currentNode
            };
            HashSet<SearchNode> closedSet = new HashSet<SearchNode>();

            while (true)
            {
                // If there are no more nodes to search, return an error code.
                if (openSet.Count == 0)
                {
                    return -1;
                }

                // Select the open node with the best tentative score.
                int bestScore = int.MaxValue;
                foreach (SearchNode node in openSet)
                {
                    if (node.Score < bestScore)
                    {
                        currentNode = node;
                        bestScore = node.Score;
                    }
                }

                // If the target has been reached, return the travel time.
                if (currentNode.IsTarget())
                {
                    return currentNode.TimeFromStart;
                }

                // Consider all neighbors and update their tentative times.
                foreach ((int Δx, int Δy) in adjacentDirections)
                {
                    if ((currentNode.Tool & regions[currentNode.X + Δx, currentNode.Y + Δy]) == 0)
                    {
                        continue;
                    }
                    SearchNode neighborNode = new SearchNode(currentNode.X + Δx, currentNode.Y + Δy, currentNode.Tool, currentNode.TimeFromStart + 1);
                    if (closedSet.Contains(neighborNode))
                    {
                        SearchNode closedNode = GetNode(neighborNode, closedSet);
                        if (currentNode.TimeFromStart + 1 < closedNode.TimeFromStart)
                        {
                            closedNode.TimeFromStart = currentNode.TimeFromStart + 1;
                            openSet.Add(closedNode);
                            closedSet.Remove(closedNode);
                        }
                        continue;
                    }
                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Add(neighborNode);
                        continue;
                    }
                    SearchNode nodeToUpdate = GetNode(neighborNode, openSet);
                    nodeToUpdate.TimeFromStart = Math.Min(nodeToUpdate.TimeFromStart, currentNode.TimeFromStart + 1);
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
                    SearchNode neighborNode = new SearchNode(currentNode.X, currentNode.Y, tool, currentNode.TimeFromStart + 7);
                    if (closedSet.Contains(neighborNode))
                    {
                        SearchNode closedNode = GetNode(neighborNode, closedSet);
                        if (currentNode.TimeFromStart + 7 < closedNode.TimeFromStart)
                        {
                            closedNode.TimeFromStart = currentNode.TimeFromStart + 7;
                            openSet.Add(closedNode);
                            closedSet.Remove(closedNode);
                        }
                        break;
                    }
                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Add(neighborNode);
                        break;
                    }
                    SearchNode nodeToUpdate = GetNode(neighborNode, openSet);
                    nodeToUpdate.TimeFromStart = Math.Min(nodeToUpdate.TimeFromStart, currentNode.TimeFromStart + 7);
                    break;
                }

                // Move the current node to the closed set.
                closedSet.Add(currentNode);
                openSet.Remove(currentNode);
            }
        }

        private SearchNode GetNode(SearchNode example, HashSet<SearchNode> hashSet)
        {
            foreach (SearchNode node in hashSet)
            {
                if (node.Equals(example))
                {
                    return node;
                }
            }
            return new SearchNode(0, 0, Tool.Torch, 0);
        }

        private struct SearchNode
        {
            public static (int X, int Y) Target { get => Day22.Target; }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Tool { get; private set; }
            public int DistanceToEnd { get; private set; }
            public int TimeFromStart { get; set; }
            public int Score { get => CalculateScore(); }

            public SearchNode(int x, int y, int tool, int time)
            {
                X = x;
                Y = y;
                Tool = tool;
                DistanceToEnd = Math.Abs(x - Target.X) + Math.Abs(y - Target.Y);
                TimeFromStart = time;
            }

            public SearchNode(int x, int y, Tool tool, int time) : this(x, y, (int)tool, time) { }

            private int CalculateScore()
            {
                int score = TimeFromStart + DistanceToEnd;
                if (Tool == 1 || Tool == 4)
                {
                    score += 7;
                }
                return score;
            }

            public bool IsTarget()
            {
                if (X == Target.X && Y == Target.Y && Tool == 2)
                {
                    return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return X << 16 ^ Y << 3 ^ Tool;
            }

            public override bool Equals(object obj)
            {
                if (obj is SearchNode otherNode)
                {
                    if (X == otherNode.X && Y == otherNode.Y && Tool == otherNode.Tool)
                    {
                        return true;
                    }
                }
                return false;
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
