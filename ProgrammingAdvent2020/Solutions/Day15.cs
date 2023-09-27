// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day15 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        string[] terms = input[0].Split(',');
        if (terms.Length == 0)
        {
            errorMessage = "First line of input does not contain a list of numbers.";
            return false;
        }
        foreach (string term in terms)
        {
            if (!int.TryParse(term, out int _))
            {
                errorMessage = $"Invalid term \"{term.Left(20, true)}\" in input.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        int[] startingNumbers = input[0].Split(',').Select(s => int.Parse(s)).ToArray();
        int partOneAnswer = CalculateNumber(2020, startingNumbers);
        int partTwoAnswer = CalculateNumber(30_000_000, startingNumbers);

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static int CalculateNumber(int position, int[] startingNumbers)
    {
        int[] memory = new int[position];
        int lastNumberAge = 0;
        for (int turn = 1; turn <= startingNumbers.Length; turn++)
        {
            int numberSpoken = startingNumbers[turn - 1];
            lastNumberAge = memory[numberSpoken] == 0 ? 0 : turn - memory[numberSpoken];
            memory[numberSpoken] = turn;
        }
        for (int turn = startingNumbers.Length + 1; turn < position; turn++)
        {
            int numberSpoken = lastNumberAge;
            lastNumberAge = memory[numberSpoken] == 0 ? 0 : turn - memory[numberSpoken];
            memory[numberSpoken] = turn;
        }
        return lastNumberAge;
    }
}
