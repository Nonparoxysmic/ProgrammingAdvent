// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day17 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        Match match = GeneratedRegex.ValidDay17Input.Match(input[0]);
        if (!match.Success)
        {
            return ("Invalid input.", "n/a");
        }
        int xMin = int.Parse(match.Groups["xMin"].Value);
        int xMax = int.Parse(match.Groups["xMax"].Value);
        int yMin = int.Parse(match.Groups["yMin"].Value);
        int yMax = int.Parse(match.Groups["yMax"].Value);
        if (xMax < xMin)
        {
            (xMin, xMax) = (xMax, xMin);
        }
        if (yMax < yMin)
        {
            (yMin, yMax) = (yMax, yMin);
        }
        Target target = new(xMin, xMax, yMin, yMax);
        if (target.Ymax >= 0)
        {
            return ("Not solved for this case.", "n/a");
        }

        int y = 0;
        for (int v = -target.Ymin - 1; v > 0; v--)
        {
            y += v;
        }
        int partOneAnswer = y;

        return ($"{partOneAnswer}", "n/a");
    }

    private readonly struct Target(int xMin, int xMax, int yMin, int yMax)
    {
        public int Xmin { get; } = xMin;
        public int Xmax { get; } = xMax;
        public int Ymin { get; } = yMin;
        public int Ymax { get; } = yMax;

        public override string ToString()
        {
            return $"target area: x={Xmin}..{Xmax}, y={Ymin}..{Ymax}";
        }
    }
}
