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
    internal class Day17 : Day
    {
        private readonly Regex horzClayLine = new Regex(@"^y=([0-9]{1,9}), x=([0-9]{1,9})\.\.([0-9]{1,9})$");
        private readonly Regex vertClayLine = new Regex(@"^x=([0-9]{1,9}), y=([0-9]{1,9})\.\.([0-9]{1,9})$");

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
            int minimumX = int.MaxValue;
            int minimumY = int.MaxValue;
            int maximumX = int.MinValue;
            int maximumY = int.MinValue;
            List<(int, int, int, int)> clayInput = new List<(int, int, int, int)>();
            foreach (string line in inputLines)
            {
                Match horizontalMatch = horzClayLine.Match(line);
                Match verticalMatch = vertClayLine.Match(line);
                if (!horizontalMatch.Success && !verticalMatch.Success)
                {
                    output.WriteError($"Invalid line in input \"{line}\".", sw);
                    return output;
                }
                Match match = horizontalMatch.Success ? horizontalMatch : verticalMatch;
                (int lowerX, int lowerY, int upperX, int upperY) clay = Clay(match);
                minimumX = Math.Min(minimumX, clay.lowerX);
                minimumY = Math.Min(minimumY, clay.lowerY);
                maximumX = Math.Max(maximumX, clay.upperX);
                maximumY = Math.Max(maximumY, clay.upperY);
                clayInput.Add(clay);
            }
            if (500 < minimumX || 500 > maximumX)
            {
                // Water hits no clay.
            }

            int width = maximumX - minimumX + 3;
            int height = maximumY - minimumY + 1;
            MapArray<char> map = new MapArray<char>(width, height, 2, '.', (minimumX - 1, minimumY));
            map.Fill('.');
            foreach ((int lowerX, int lowerY, int upperX, int upperY) in clayInput)
            {
                for (int y = lowerY; y <= upperY; y++)
                {
                    for (int x = lowerX; x <= upperX; x++)
                    {
                        map[x, y] = '#';
                    }
                }
            }
            (int X, int Y) springPosition = (500, 0);
            if (minimumY > 2)
            {
                springPosition = (500, minimumY - 2);
            }
            map[springPosition.X, springPosition.Y] = '+';

            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private (int, int, int, int) Clay(Match match)
        {
            int fixedCoord = int.Parse(match.Groups[1].Value);
            int lowerRange = int.Parse(match.Groups[2].Value);
            int upperRange = int.Parse(match.Groups[3].Value);
            if (upperRange < lowerRange)
            {
                int temp = upperRange;
                upperRange = lowerRange;
                lowerRange = temp;
            }
            if (match.Groups[0].Value[0] == 'x')
            {
                return (fixedCoord, lowerRange, fixedCoord, upperRange);
            }
            else
            {
                return (lowerRange, fixedCoord, upperRange, fixedCoord);
            }
        }
    }
}
