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
    internal class Day03 : Day
    {
        private readonly Regex validLine = new Regex(@"^#(\d+) @ (\d+),(\d+): (\d+)x(\d+)$");

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

            List<FabricClaim> claims = new List<FabricClaim>();
            int maxWidth = 0;
            int maxHeight = 0;
            foreach (string line in inputLines)
            {
                Match match = validLine.Match(line);
                if (!match.Success)
                {
                    output.WriteError($"Invalid line \"{line}\" in input.", sw);
                    return output;
                }
                int claimNumber = int.Parse(match.Groups[1].Value);
                int x = int.Parse(match.Groups[2].Value);
                int y = int.Parse(match.Groups[3].Value);
                int width = int.Parse(match.Groups[4].Value);
                int height = int.Parse(match.Groups[5].Value);
                maxWidth = Math.Max(maxWidth, x + width);
                maxHeight = Math.Max(maxHeight, y + height);
                claims.Add(new FabricClaim(claimNumber, x, y, width, height));
            }

            int[,] fabric = new int[maxWidth, maxHeight];
            foreach (FabricClaim claim in claims)
            {
                for (int x = claim.X; x < claim.X + claim.Width; x++)
                {
                    for (int y = claim.Y; y < claim.Y + claim.Height; y++)
                    {
                        fabric[x, y]++;
                    }
                }
            }

            int overlaps = 0;
            for (int x = 0; x < maxWidth; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    if (fabric[x, y] > 1) { overlaps++; }
                }
            }

            sw.Stop();
            output.WriteAnswers(overlaps, null, sw);
            return output;
        }

        private class FabricClaim
        {
            public int ID { get; }
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }

            public FabricClaim(int id, int x, int y, int width, int height)
            {
                ID = id;
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }
    }
}
