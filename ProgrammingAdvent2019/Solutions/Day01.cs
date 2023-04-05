// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day01 : Day
{
    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        foreach (string line in inputLines)
        {
            if (int.TryParse(line, out int number))
            {
                if (number < 6)
                {
                    errorMessage = $"Input values must be integers greater than or equal to 6.";
                    return false;
                }
            }
            else
            {
                errorMessage = $"Line \"{line.Left(20, true)}\" cannot be parsed as an Int32.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        int totalFuelRequired = inputLines.Sum(line => int.Parse(line) / 3 - 2);
        return output.WriteAnswers(totalFuelRequired, null);
    }
}
