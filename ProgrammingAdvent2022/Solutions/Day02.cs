// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day02 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^[A-C] [X-Z]$")]
    private static partial Regex ValidInputLineRegex();

    private static readonly int[,] _outcomeScores =
    {
        { 3, 6, 0 },
        { 0, 3, 6 },
        { 6, 0, 3 },
    };

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        bool validInput = input.Aggregate(true, (valid, next) => valid && ValidInputLine.IsMatch(next));
        if (!validInput)
        {
            return result.WriteError("Invalid input.");
        }

        int totalScore = 0;
        foreach (string line in input)
        {
            totalScore += line[2] - 'W' + _outcomeScores[line[0] - 'A', line[2] - 'X'];
        }

        return result.WriteAnswers(totalScore, null);
    }
}
