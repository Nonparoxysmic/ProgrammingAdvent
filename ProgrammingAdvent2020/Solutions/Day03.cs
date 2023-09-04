// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day03 : Day
{
    private static readonly Regex _validLine = new("^[.#]{1,48}$");
    private static readonly (int X, int Y)[] _slopes = new (int, int)[]
    {
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2)
    };

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
            if (line.Length != input[0].Length)
            {
                errorMessage = "All lines in input must be the same length.";
                return false;
            }
        }
        if (input[0][0] != '.')
        {
            errorMessage = "The first character must be an open square.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        char[,] map = input.ToCharArray2D();
        (long partOneAnswer, long partTwoAnswer) = CheckSlopes(map);
        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static (long, long) CheckSlopes(char[,] map)
    {
        long[] counts = new long[_slopes.Length];
        for (int i = 0; i < _slopes.Length; i++)
        {
            counts[i] = CountTreesOnSlope(map, _slopes[i].X, _slopes[i].Y);
        }
        long product = counts.Aggregate(1L, (aggregated, element) => aggregated * element);
        return (counts[1], product);
    }

    private static int CountTreesOnSlope(char[,] map, int dx, int dy)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int treeCount = 0;
        int x = 0;
        for (int y = 0; y < height; y += dy)
        {
            if (map[x % width, y] == '#')
            {
                treeCount++;
            }
            x += dx;
        }
        return treeCount;
    }
}
