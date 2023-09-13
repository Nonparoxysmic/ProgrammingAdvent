// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day09 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (input.Length < 3)
        {
            errorMessage = "Input is too short.";
            return false;
        }
        foreach (string line in input)
        {
            if (!ulong.TryParse(line, out ulong _))
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

        int size = 25;
        if (exampleModifier is not null)
        {
            size = 5;
        }

        ulong[] numbers = input.Select(line => ulong.Parse(line)).ToArray();
        ulong? partOneAnswer = FirstNumberNotSumOfPrevious(numbers, size);
        if (partOneAnswer is null)
        {
            return output.WriteError($"No number is the sum of two of the previous {size}.");
        }

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static ulong? FirstNumberNotSumOfPrevious(ulong[] numbers, int size)
    {
        for (int i = size; i < numbers.Length; i++)
        {
            if (!IsSumOfPrevious(i, numbers, size))
            {
                return numbers[i];
            }
        }
        return null;
    }

    private static bool IsSumOfPrevious(int targetIndex, ulong[] numbers, int size)
    {
        for (int i = targetIndex - size; i < targetIndex - 1; i++)
        {
            for (int j = i + 1; j < targetIndex; j++)
            {
                if (numbers[i] + numbers[j] == numbers[targetIndex])
                {
                    return true;
                }
            }
        }
        return false;
    }
}
