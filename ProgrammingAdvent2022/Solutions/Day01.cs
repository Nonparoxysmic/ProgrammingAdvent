// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day01 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        bool validInput = input.Aggregate(true, (valid, next) =>
            valid && (next == string.Empty || int.TryParse(next, out int _)));
        if (!validInput)
        {
            return result.WriteError("Invalid input.");
        }

        List<int> calorieTotals = [];
        int sum = 0;
        foreach (string line in input)
        {
            if (line == string.Empty)
            {
                calorieTotals.Add(sum);
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }
        int mostTotalCalories = calorieTotals.Max();

        return result.WriteAnswers(mostTotalCalories, null);
    }
}
