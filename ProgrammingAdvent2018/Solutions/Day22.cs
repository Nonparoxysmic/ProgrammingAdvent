﻿// Puzzle solution by Nonparoxysmic
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

        static readonly (int Δx, int Δy, int Δtool, int Δtime)[] adjacentNodes = new (int, int, int, int)[]
        {
            (-1,  0,  0,  1),
            ( 1,  0,  0,  1),
            ( 0, -1,  0,  1),
            ( 0,  1,  0,  1),
            ( 0,  0,  1,  7),
            ( 0,  0,  2,  7)
        };

        public static readonly int[] ToolFlags = new int[] { 1, 2, 4 };

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

            MapArray<SearchNode>[] nodes = new MapArray<SearchNode>[3];
            for (int i = 0; i < 3; i++)
            {
                nodes[i] = new MapArray<SearchNode>(regions.Width, regions.Height, 1, SearchNode.Empty, (0, 0));
            }
            for (int tool = 0; tool < 3; tool++)
            {
                for (int y = 0; y < regions.Height; y++)
                {
                    for (int x = 0; x < regions.Width; x++)
                    {
                        nodes[tool][x, y] = new SearchNode(x, y, tool, int.MaxValue - 1);
                    }
                }
            }

            SearchNode currentNode = nodes[1][0, 0];
            currentNode.UpdateScores(0);
            HashSet<SearchNode> openSet = new HashSet<SearchNode>
            {
                currentNode
            };

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

                // Consider all neighboring nodes and update their tentative times.
                foreach ((int Δx, int Δy, int Δtool, int Δtime) in adjacentNodes)
                {
                    int neighborX = currentNode.X + Δx;
                    int neighborY = currentNode.Y + Δy;
                    int neighborTool = (currentNode.Tool + Δtool) % 3;
                    SearchNode neighbor = nodes[neighborTool][neighborX, neighborY];
                    if ((neighbor.ToolFlag & regions[neighborX, neighborY]) == 0)
                    {
                        continue;
                    }
                    int newTime = currentNode.TimeFromStart + Δtime;
                    if (newTime < neighbor.TimeFromStart)
                    {
                        neighbor.UpdateScores(newTime);
                        openSet.Add(neighbor);
                    }
                }

                // Remove the current node from the open set.
                openSet.Remove(currentNode);
            }
        }

        private class SearchNode
        {
            static readonly SearchNode _empty = new SearchNode();
            public static SearchNode Empty { get => _empty; }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Tool { get; private set; }
            public int ToolFlag { get; private set; }
            public int DistanceToEnd { get; private set; }
            public int TimeFromStart { get; set; }
            public int Score { get; private set; }

            // Tool  Name   Flag
            //  0    None    1
            //  1    Torch   2
            //  2    Gear    4

            public SearchNode(int x, int y, int tool, int time)
            {
                X = x;
                Y = y;
                Tool = tool;
                ToolFlag = ToolFlags[tool];
                DistanceToEnd = Math.Abs(x - Target.X) + Math.Abs(y - Target.Y);
                UpdateScores(time);
            }

            public SearchNode()
            {
                X = 0;
                Y = 0;
                Tool = 0;
                ToolFlag = 0;
                DistanceToEnd = int.MaxValue;
                TimeFromStart = 0;
                Score = int.MaxValue;
            }

            public void UpdateScores(int time)
            {
                TimeFromStart = time;
                Score = time + DistanceToEnd;
            }

            public bool IsTarget()
            {
                if (X == Target.X && Y == Target.Y && ToolFlag == 2)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
