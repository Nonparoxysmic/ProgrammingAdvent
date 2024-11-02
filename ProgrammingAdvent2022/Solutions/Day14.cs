// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day14 : Day
{
    public static readonly Regex CoordinateLine = CoordinateLineRegex();

    [GeneratedRegex("(?=(?<=(^| ))(?<x1>[0-9]{1,3}),(?<y1>[0-9]{1,3}) -> (?<x2>[0-9]{1,3}),(?<y2>[0-9]{1,3})(?=($| )))")]
    private static partial Regex CoordinateLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        char[,] map = InputToMap(input, out int offset);
        int sandAtRest = SandAtRest(map, 500 - offset, 0);
        int actualSandAtRest = MoreMath.PowInt(map.GetLength(1), 2) - map.Cast<char>().Count(c => c == '#');

        return result.WriteAnswers(sandAtRest, actualSandAtRest);
    }

    private static char[,] InputToMap(string[] input, out int offset)
    {
        int xMin = 500, xMax = 500, yMax = 0;
        List<(int, int, int, int)> lines = [];
        foreach (string line in input)
        {
            MatchCollection matches = CoordinateLine.Matches(line);
            foreach (Match match in matches)
            {
                int x1 = int.Parse(match.Groups["x1"].Value);
                int y1 = int.Parse(match.Groups["y1"].Value);
                int x2 = int.Parse(match.Groups["x2"].Value);
                int y2 = int.Parse(match.Groups["y2"].Value);
                xMin = Math.Min(xMin, Math.Min(x1, x2));
                xMax = Math.Max(xMax, Math.Max(x1, x2));
                yMax = Math.Max(yMax, Math.Max(y1, y2));
                lines.Add((x1, y1, x2, y2));
            }
        }
        offset = xMin - 1;
        char[,] map = new char[xMax - xMin + 3, yMax + 2];
        map.Fill('.');
        foreach ((int x1, int y1, int x2, int y2) in lines)
        {
            DrawLine(map, x1, y1, x2, y2, offset);
        }
        map[500 - offset, 0] = '+';
        for (int y = 0; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                if (map[x - 1, y] == '#' && map[x, y] == '#' && map[x + 1, y] == '#')
                {
                    map[x, y + 1] = '#';
                }
            }
        }
        return map;
    }

    private static void DrawLine(char[,] map, int x1, int y1, int x2, int y2, int offset)
    {
        for (int y = y1; ; y += Math.Sign(y2 - y1))
        {
            for (int x = x1 - offset; ; x += Math.Sign(x2 - x1))
            {
                map[x, y] = '#';
                if (x == x2 - offset)
                {
                    break;
                }
            }
            if (y == y2)
            {
                break;
            }
        }
    }

    private static int SandAtRest(char[,] map, int x, int y)
    {
        int count = 0;
        SandRests(map, x, y, ref count);
        return count;
    }

    private static bool SandRests(char[,] map, int x, int y, ref int count)
    {
        if (y == map.GetLength(1) - 1)
        {
            return false;
        }
        bool noFall = map[x, y + 1] == '#' || SandRests(map, x, y + 1, ref count);
        if (noFall)
        {
            bool noLeft = map[x - 1, y + 1] == '#' || SandRests(map, x - 1, y + 1, ref count);
            if (noLeft)
            {
                bool noRight = map[x + 1, y + 1] == '#' || SandRests(map, x + 1, y + 1, ref count);
                if (noRight)
                {
                    if (map[x, y] != 'o')
                    {
                        count++;
                        map[x, y] = 'o';
                    }
                    return true;
                }
            }
        }
        return false;
    }
}
