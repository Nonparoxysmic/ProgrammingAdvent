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
    internal class Day23 : Day
    {
        private readonly Regex validLine = new Regex(@"pos=<(-?[0-9]{1,9}),(-?[0-9]{1,9}),(-?[0-9]{1,9})>, r=([0-9]{1,9})");

        private List<(int, int, int, int)> nanobots;

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();

            nanobots = new List<(int, int, int, int)>();
            (int X, int Y, int Z, int R) strongestNanobot = (0, 0, 0, -1);
            for (int i = 0; i < inputLines.Length; i++)
            {
                Match match = validLine.Match(inputLines[i]);
                if (!match.Success)
                {
                    output.WriteError($"Invalid line {i} in input.", sw);
                    return output;
                }
                int X = int.Parse(match.Groups[1].Value);
                int Y = int.Parse(match.Groups[2].Value);
                int Z = int.Parse(match.Groups[3].Value);
                int R = int.Parse(match.Groups[4].Value);
                if (R > strongestNanobot.R)
                {
                    strongestNanobot = (X, Y, Z, R);
                }
                nanobots.Add((X, Y, Z, R));
            }

            int partOneAnswer = 0;
            foreach ((int X, int Y, int Z, int R) in nanobots)
            {
                int distance = Math.Abs(X - strongestNanobot.X)
                    + Math.Abs(Y - strongestNanobot.Y)
                    + Math.Abs(Z - strongestNanobot.Z);
                if (distance <= strongestNanobot.R)
                {
                    partOneAnswer++;
                }
            }

            int partTwoAnswer = PartTwoAnswer();

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int PartTwoAnswer()
        {
            (int X, int Y, int Z) center = AveragePosition();
            for (int i = 24; i > 0; i--)
            {
                int step = (int)Math.Pow(2, i);
                (int X, int Y, int Z) bestPosition = center;
                int bestPositionNanobots = -1;
                for (int z = center.Z - 8 * step; z <= center.Z + 8 * step; z += step)
                {
                    for (int y = center.Y - 8 * step; y <= center.Y + 8 * step; y += step)
                    {
                        for (int x = center.X - 8 * step; x <= center.X + 8 * step; x += step)
                        {
                            int nanobotsInRange = NanobotsInRange(x, y, z);
                            if (nanobotsInRange > bestPositionNanobots ||
                                (nanobotsInRange == bestPositionNanobots &&
                                x + y + z < bestPosition.X + bestPosition.Y + bestPosition.Z))
                            {
                                bestPositionNanobots = nanobotsInRange;
                                bestPosition = (x, y, z);
                            }
                        }
                    }
                }
                center = bestPosition;
            }
            return center.X + center.Y + center.Z;
        }

        private (int, int, int) AveragePosition()
        {
            int sumX = 0, sumY = 0, sumZ = 0;
            foreach ((int X, int Y, int Z, int R) in nanobots)
            {
                sumX += X;
                sumY += Y;
                sumZ += Z;
            }
            return (sumX / nanobots.Count, sumY / nanobots.Count, sumZ / nanobots.Count);
        }

        private int NanobotsInRange(int x, int y, int z)
        {
            int count = 0;
            foreach ((int X, int Y, int Z, int R) in nanobots)
            {
                int distance = Math.Abs(X - x) + Math.Abs(Y - y) + Math.Abs(Z - z);
                if (distance <= R)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
