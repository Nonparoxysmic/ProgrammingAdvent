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

            int partOneAnswer = PartOneAnswer(depth, targetX, targetY);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private int PartOneAnswer(int depth, int targetX, int targetY)
        {
            MapArray<int> erosionLevels = new MapArray<int>(targetX + 1, targetY + 1);
            erosionLevels[0, 0] = depth % 20183;
            for (int x = 1; x < erosionLevels.Width; x++)
            {
                erosionLevels[x, 0] = (x * 16807 + depth) % 20183;
            }
            for (int y = 1; y < erosionLevels.Height; y++)
            {
                erosionLevels[0, y] = (y * 48271 + depth) % 20183;
            }
            for (int y = 1; y < erosionLevels.Height; y++)
            {
                for (int x = 1; x < erosionLevels.Width; x++)
                {
                    int geologicIndex = erosionLevels[x - 1, y] * erosionLevels[x, y - 1];
                    erosionLevels[x, y] = (geologicIndex + depth) % 20183;
                }
            }
            erosionLevels[targetX, targetY] = depth % 20183;

            int riskSum = 0;
            for (int y = 0; y < erosionLevels.Height; y++)
            {
                for (int x = 0; x < erosionLevels.Width; x++)
                {
                    riskSum += erosionLevels[x, y] % 3;
                }
            }
            return riskSum;
        }
    }
}
