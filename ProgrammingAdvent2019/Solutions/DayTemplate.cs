// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class DayTemplate : Day
{
    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        errorMessage = "Input validation not yet implemented.";
        return false;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        return output.WriteAnswers(null, null);
    }
}
