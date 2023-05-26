// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day18 : Day
{
    private static readonly Regex _validCharacters = new("^[a-zA-Z@.#]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        HashSet<char> keys = new();
        bool entranceFound = false;
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (line.Length != inputLines[0].Length)
            {
                errorMessage = "Input lines are not all the same length.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
            foreach (char c in line)
            {
                if ('a' <= c && c <= 'z')
                {
                    if (!keys.Add(c))
                    {
                        errorMessage = $"Input contains duplicate key '{c}'.";
                        return false;
                    }
                }
                if (c == '@')
                {
                    if (entranceFound)
                    {
                        errorMessage = "Input contains multiple entrances.";
                        return false;
                    }
                    else
                    {
                        entranceFound = true;
                    }
                }
            }
        }
        if (!entranceFound)
        {
            errorMessage = "Input contains no entrance.";
            return false;
        }
        if (keys.Count == 0)
        {
            errorMessage = "Input does not contain any keys.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        return output.WriteAnswers(null, null);
    }
}
