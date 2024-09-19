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
        if (target.Ymax >= 0 || target.Xmin <= 0)
        {
            return ("Not solved for this case.", "n/a");
        }

        int yPos = 0;
        for (int v = -target.Ymin - 1; v > 0; v--)
        {
            yPos += v;
        }
        int partOneAnswer = yPos;

        int xMaxVelocity = target.Xmax;
        int yMinVelocity = target.Ymin;
        int yMaxVelocity = -target.Ymin - 1;
        int xMinVelocity = MininumForwardVelocity(target);
        int goodVelocities = 0;
        for (int y = yMinVelocity; y <= yMaxVelocity; y++)
        {
            for (int x = xMinVelocity; x <= xMaxVelocity; x++)
            {
                if (HitsTarget(x, y, target))
                {
                    goodVelocities++;
                }
            }
        }
        int partTwoAnswer = goodVelocities;

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static int MininumForwardVelocity(Target target)
    {
        int x = target.Xmin;
        int velocity = 0;
        while (x > 0)
        {
            velocity++;
            x -= velocity;
        }
        return velocity;
    }

    private static bool HitsTarget(int xVelocity, int yVelocity, Target target)
    {
        int x = 0, y = 0;
        while (x <= target.Xmax && y >= target.Ymin)
        {
            x += xVelocity;
            y += yVelocity;
            if (target.Contains(x, y))
            {
                return true;
            }
            xVelocity -= Math.Sign(xVelocity);
            yVelocity--;
        }
        return false;
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

        public bool Contains(int x, int y)
        {
            return Xmin <= x && x <= Xmax && Ymin <= y && y <= Ymax;
        }
    }
}
