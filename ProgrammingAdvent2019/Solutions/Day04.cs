// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day04 : Day
{
    private static readonly Regex _validInput = new("^(?<Start>[0-9]{6})-(?<End>[0-9]{6})$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        Match match = _validInput.Match(inputLines[0]);
        if (!match.Success)
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" is invalid.";
            return false;
        }
        int start = int.Parse(match.Groups["Start"].Value);
        int end = int.Parse(match.Groups["End"].Value);
        if (end < start)
        {
            errorMessage = "End of range must be greater than or equal to the start.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        Match match = _validInput.Match(inputLines[0]);
        string start = match.Groups["Start"].Value;
        int end = int.Parse(match.Groups["End"].Value);
        int partOneAnswer = CountPotentialPasswords(start, end - int.Parse(start) + 1, 1);
        int partTwoAnswer = CountPotentialPasswords(start, end - int.Parse(start) + 1, 2);
        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static int CountPotentialPasswords(string start, int steps, int part)
    {
        char[] current = new char[start.Length];
        Array.Copy(start.ToCharArray(), current, start.Length);
        int count = 0;
        for (int i = 0; i < steps; i++)
        {
            count += IsPotentialPassword(current)[part - 1] ? 1 : 0;
            Increment(current);
        }
        return count;
    }

    private static bool[] IsPotentialPassword(char[] chars)
    {
        bool hasDoubles = false;
        for (int i = 0; i < chars.Length - 1; i++)
        {
            if (chars[i] > chars[i + 1])
            {
                return new bool[] { false, false };
            }
            hasDoubles |= chars[i] == chars[i + 1];
        }
        if (hasDoubles)
        {
            for (int i = 1; i < chars.Length - 2; i++)
            {
                if (chars[i] == chars[i + 1] && chars[i - 1] != chars[i] && chars[i + 1] != chars[i + 2])
                {
                    return new bool[] { true, true };
                }
            }
            if (chars[0] == chars[1] && chars[1] != chars[2])
            {
                return new bool[] { true, true };
            }
            if (chars[^1] == chars[^2] && chars[^2] != chars[^3])
            {
                return new bool[] { true, true };
            }
            return new bool[] { true, false };
        }
        return new bool[] { false, false };
    }

    private static void Increment(char[] chars, int carryoverPosition = 0)
    {
        if (chars[^(carryoverPosition + 1)] < '9')
        {
            chars[^(carryoverPosition + 1)]++;
            return;
        }
        chars[^(carryoverPosition + 1)] = '0';
        Increment(chars, carryoverPosition + 1);
    }
}
