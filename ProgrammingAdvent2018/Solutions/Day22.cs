// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day22 : Day
    {
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


            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
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
    }
}
