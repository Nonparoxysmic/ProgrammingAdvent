// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day22 : Day
{
    private static readonly Regex _validLine = new("^(deal into new stack|deal with increment (?<increment>[0-9]{1,2})|cut (?<cut>-?[0-9]{1,4}))$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        return output.WriteAnswers(null, null);
    }

    private static int NewStack(int position, int deckSize)
    {
        return deckSize - position - 1;
    }

    private static int Cut(int position, int deckSize, int N)
    {
        if (N > 0)
        {
            if (position < N)
            {
                return deckSize - N + position;
            }
            else
            {
                return position - N;
            }
        }
        else if (N < 0)
        {
            if (position < deckSize + N)
            {
                return position - N;
            }
            else
            {
                return position - deckSize - N;
            }
        }
        else return position;
    }
}
