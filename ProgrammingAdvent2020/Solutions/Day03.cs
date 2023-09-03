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
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int treeCount = 0;
        int x = 0;
        for (int y = 0; y < height; y++)
        {
            if (map[x % width, y] == '#')
            {
                treeCount++;
            }
            x += 3;
        }
        return output.WriteAnswers(treeCount, null);
    }
}
