﻿// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day10 : Day
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
            if (!int.TryParse(line, out int joltage) || joltage < 1 || joltage > 217)
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

        List<int> adapters = input.Select(n => int.Parse(n)).ToList();
        adapters.Sort();
        int partOneAnswer = PartOneAnswer(adapters);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static int PartOneAnswer(List<int> adapters)
    {
        int ones = 0, threes = 1;
        if (adapters[0] == 1)
        {
            ones++;
        }
        if (adapters[0] == 3)
        {
            threes++;
        }
        for (int i = 1; i < adapters.Count; i++)
        {
            int diff = adapters[i] - adapters[i - 1];
            if (diff == 1)
            {
                ones++;
            }
            if (diff == 3)
            {
                threes++;
            }
        }
        return ones * threes;
    }
}
