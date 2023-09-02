// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day02 : Day
{
    private static readonly Regex _validLine
        = new("^(?<min>[0-9]{1,2})-(?<max>[0-9]{1,2}) (?<letter>[a-z]): (?<password>[a-z]{1,32})$");

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
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        int validPasswords = 0;
        foreach (string line in input)
        {
            Match match = _validLine.Match(line);
            if (match.Success)
            {
                int min = int.Parse(match.Groups["min"].Value);
                int max = int.Parse(match.Groups["max"].Value);
                char letter = match.Groups["letter"].Value[0];
                string password = match.Groups["password"].Value;
                int count = 0;
                foreach (char c in password)
                {
                    if (c == letter)
                    {
                        count++;
                    }
                }
                if (min <= count && count <= max)
                {
                    validPasswords++;
                }
            }
        }
        return output.WriteAnswers(validPasswords, null);
    }
}
