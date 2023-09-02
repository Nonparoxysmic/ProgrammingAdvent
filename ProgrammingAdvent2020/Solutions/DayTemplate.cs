// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

//using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class DayTemplate : Day
{
    //private static readonly Regex _validLine = new("^ $");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        //foreach (string line in input)
        //{
        //    if (!_validLine.IsMatch(line))
        //    {
        //        errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
        //        return false;
        //    }
        //}
        errorMessage = "Input validation not implemented.";
        return false;
        //errorMessage = string.Empty;
        //return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        return output.WriteAnswers(null, null);
    }
}
