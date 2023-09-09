// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day06 : Day
{
    private static readonly Regex _validLine = new("^[a-z]{1,26}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                continue;
            }
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

        List<Group> groups = ReadInput(input);
        int partOneAnswer = groups.Sum(g => g.YesQuestions.Count);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static List<Group> ReadInput(string[] input)
    {
        List<Group> output = new();
        Group? current = null;
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                if (current is not null)
                {
                    output.Add(current);
                    current = null;
                }
                continue;
            }
            current ??= new();
            foreach (char c in line)
            {
                current.YesQuestions.Add(c);
            }
        }
        if (current is not null)
        {
            output.Add(current);
        }
        return output;
    }

    private class Group
    {
        public HashSet<char> YesQuestions = new();
    }
}
