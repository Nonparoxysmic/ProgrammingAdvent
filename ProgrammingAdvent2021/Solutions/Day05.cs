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
                if (x1 > x2)
                {
                    (x1, y1, x2, y2) = (x2, y2, x1, y1);
                }
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

        // PART TWO
        foreach (VentLine diagLine in diagonalLines)
        {
            foreach (VentLine horzLine in horizontalLines)
            {
                if (LinesIntersect(diagLine, horzLine))
                {
                    intersections.Add(GetLineIntersection(diagLine, horzLine));
                }
            }
            foreach (VentLine vertLine in verticalLines)
            {
                if (LinesIntersect(diagLine, vertLine))
                {
                    intersections.Add(GetLineIntersection(diagLine, vertLine));
                }
            }
        }
        for (int i = 0; i < diagonalLines.Count - 1; i++)
        {
            for (int j = i + 1; j < diagonalLines.Count; j++)
            {
                if (diagonalLines[i].DiagonalUp && diagonalLines[j].DiagonalUp)
                {
                    // TODO: Check for overlap
                }
                else if (!diagonalLines[i].DiagonalUp && !diagonalLines[j].DiagonalUp)
                {
                    // TODO: Check for overlap
                }
                else if (LinesIntersect(diagonalLines[i], diagonalLines[j]))
                {
                    intersections.Add(GetLineIntersection(diagonalLines[i], diagonalLines[j]));
                }
            }
        }
        int partTwoAnswer = intersections.Count;

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static bool LinesIntersect(VentLine a, VentLine b)
    {
        int o1 = Orientation(a.X1, a.Y1, a.X2, a.Y2, b.X1, b.Y1);
        int o2 = Orientation(a.X1, a.Y1, a.X2, a.Y2, b.X2, b.Y2);
        int o3 = Orientation(b.X1, b.Y1, b.X2, b.Y2, a.X1, a.Y1);
        int o4 = Orientation(b.X1, b.Y1, b.X2, b.Y2, a.X2, a.Y2);
        return o1 != o2 && o3 != o4;
    }

    private static int Orientation(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        return Math.Sign((y2 - y1) * (x3 - x2) - (y3 - y2) * (x2 - x1));
    }

    private static (int, int) GetLineIntersection(VentLine a, VentLine b)
    {
        return GetLineIntersection(a.X1, a.Y1, a.X2, a.Y2, b.X1, b.Y1, b.X2, b.Y2);
    }

    private static (int, int) GetLineIntersection(int p0_x, int p0_y, int p1_x, int p1_y, 
        int p2_x, int p2_y, int p3_x, int p3_y)
    {
        int s1_x = p1_x - p0_x;
        int s1_y = p1_y - p0_y;
        int s2_x = p3_x - p2_x;
        int s2_y = p3_y - p2_y;

        int t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

        int i_x = p0_x + (t * s1_x);
        int i_y = p0_y + (t * s1_y);
        return (i_x, i_y);
    }

    private class VentLine(int x1, int y1, int x2, int y2)
    {
        public int X1 { get; } = x1;
        public int Y1 { get; } = y1;
        public int X2 { get; } = x2;
        public int Y2 { get; } = y2;

        public bool DiagonalUp { get; } = y2 > y1;

        public override string ToString()
        {
            return $"{X1},{Y1} -> {X2},{Y2}";
        }
    }
}
