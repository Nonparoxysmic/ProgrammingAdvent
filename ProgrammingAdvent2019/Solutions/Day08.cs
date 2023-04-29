// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day08 : Day
{
    private static readonly Regex _validCharacters = new("^[012]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!_validCharacters.IsMatch(inputLines[0]))
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains invalid characters.";
            return false;
        }
        if (inputLines[0].Length < 25 * 6)
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains insufficient characters.";
            return false;
        }
        if (inputLines[0].Length % (25 * 6) != 0)
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains the wrong number of characters.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        char[,,] pixels = new char[inputLines[0].Length / 150, 25, 6];
        int fewestZeroes = int.MaxValue;
        int fewestZeroesLayer = 0;
        for (int layer = 0; layer < inputLines[0].Length / 150; layer++)
        {
            int zeroCount = 0;
            for (int i = 0; i < 150; i++)
            {
                char c = inputLines[0][i + layer * 150];
                if (c == '0')
                {
                    zeroCount++;
                }
                int x = i % 25;
                int y = i / 25;
                pixels[layer, x, y] = c;
            }
            if (zeroCount < fewestZeroes)
            {
                fewestZeroes = zeroCount;
                fewestZeroesLayer = layer;
            }
        }
        int oneCount = 0;
        int twoCount = 0;
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                char pixel = pixels[fewestZeroesLayer, x, y];
                if (pixel == '1')
                {
                    oneCount++;
                }
                else if (pixel == '2')
                {
                    twoCount++;
                }
            }
        }
        return output.WriteAnswers(oneCount * twoCount, null);
    }
}
