// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day05 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<VentLine> horizontalLines = [];
        List<VentLine> verticalLines = [];
        List<VentLine> diagonalLines = [];
        foreach (string line in input)
        {
            Match match = GeneratedRegex.ValidDay05InputLine.Match(line);
            int x1 = int.Parse(match.Groups["x1"].Value);
            int y1 = int.Parse(match.Groups["y1"].Value);
            int x2 = int.Parse(match.Groups["x2"].Value);
            int y2 = int.Parse(match.Groups["y2"].Value);
            if (y1 == y2)
            {
                (x1, x2) = (Math.Min(x1, x2), Math.Max(x1, x2));
                horizontalLines.Add(new VentLine(x1, y1, x2, y2));
            }
            else if (x1 == x2)
            {
                (y1, y2) = (Math.Min(y1, y2), Math.Max(y1, y2));
                verticalLines.Add(new VentLine(x1, y1, x2, y2));
            }
            else
            {
                diagonalLines.Add(new VentLine(x1, y1, x2, y2));
            }
        }

        // PART ONE
        HashSet<(int, int)> intersections = [];
        for (int i = 0; i < horizontalLines.Count - 1; i++)
        {
            for (int j = i + 1; j < horizontalLines.Count; j++)
            {
                if (horizontalLines[i].Y1 == horizontalLines[j].Y1)
                {
                    int lower = Math.Max(horizontalLines[i].X1, horizontalLines[j].X1);
                    int upper = Math.Min(horizontalLines[i].X2, horizontalLines[j].X2);
                    for (int k = lower; k <= upper; k++)
                    {
                        intersections.Add((k, horizontalLines[i].Y1));
                    }
                }
            }
        }
        for (int i = 0; i < verticalLines.Count - 1; i++)
        {
            for (int j = i + 1; j < verticalLines.Count; j++)
            {
                if (verticalLines[i].X1 == verticalLines[j].X1)
                {
                    int lower = Math.Max(verticalLines[i].Y1, verticalLines[j].Y1);
                    int upper = Math.Min(verticalLines[i].Y2, verticalLines[j].Y2);
                    for (int k = lower; k <= upper; k++)
                    {
                        intersections.Add((verticalLines[i].X1, k));
                    }
                }
            }
        }
        foreach (VentLine horzLine in horizontalLines)
        {
            foreach (VentLine vertLine in verticalLines)
            {
                if ((vertLine.Y1 <= horzLine.Y1 && horzLine.Y1 <= vertLine.Y2)
                     && (horzLine.X1 <= vertLine.X1 && vertLine.X1 <= horzLine.X2))
                {
                    intersections.Add((vertLine.X1, horzLine.Y1));
                }
            }
        }
        int partOneAnswer = intersections.Count;

        return ($"{partOneAnswer}", "n/a");
    }

    private class VentLine(int x1, int y1, int x2, int y2)
    {
        public int X1 { get; } = x1;
        public int Y1 { get; } = y1;
        public int X2 { get; } = x2;
        public int Y2 { get; } = y2;
    }
}
