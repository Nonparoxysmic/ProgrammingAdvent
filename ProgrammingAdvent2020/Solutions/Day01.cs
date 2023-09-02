// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day01 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (!int.TryParse(line, out int number) || number < 0 || number > 2020)
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
        int[] entries = input.Select(s => int.Parse(s)).ToArray();
        int? partOneAnswer = PartOneAnswer(entries);
        if (partOneAnswer is null)
        {
            return output.WriteError("No two entries sum to 2020.");
        }
        return output.WriteAnswers(partOneAnswer, null);
    }

    private static int? PartOneAnswer(int[] entries)
    {
        if (entries.Length < 2)
        {
            return null;
        }
        for (int i = 0; i < entries.Length - 1; i++)
        {
            for (int j = i + 1; j < entries.Length; j++)
            {
                if (entries[i] + entries[j] == 2020)
                {
                    return entries[i] * entries[j];
                }
            }
        }
        return null;
    }
}
