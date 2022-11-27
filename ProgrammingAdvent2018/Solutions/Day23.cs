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

            List<(int, int, int, int)> nanobots = new List<(int, int, int, int)>();
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

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }
    }
}
