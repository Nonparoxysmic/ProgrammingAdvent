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
                    errorMessage = "Input values must be integers greater than or equal to 6.";
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

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        int moduleFuelRequired = inputLines.Sum(line => int.Parse(line) / 3 - 2);
        int totalFuelRequired = inputLines.Sum(line => FuelRequiredRecursive(int.Parse(line)));
        return output.WriteAnswers(moduleFuelRequired, totalFuelRequired);
    }

    private int FuelRequiredRecursive(int mass)
    {
        int fuelRequired = mass / 3 - 2;
        return fuelRequired < 0 ? 0 : fuelRequired + FuelRequiredRecursive(fuelRequired);
    }
}
