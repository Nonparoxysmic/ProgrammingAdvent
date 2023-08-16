// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class DayTemplate : Day
{
    //private static readonly Regex _validCharacters = new("^[0-9,-]+$");
    //private static readonly Regex _validLine = new("^ $");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }

        // Intcode Validation

        //if (!ValidateIntcodeInput(inputLines, out errorMessage))
        //{
        //    return false;
        //}
        //errorMessage = string.Empty;
        //return true;

        // Line Validations

        //foreach (string line in inputLines)
        //{
        //    Match match = _validLine.Match(line);
        //    if (!match.Success)
        //    {
        //        errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
        //        return false;
        //    }
        //    if (line.Length == 0)
        //    {
        //        errorMessage = "Input contains an empty line.";
        //        return false;
        //    }
        //    if (!_validCharacters.IsMatch(line))
        //    {
        //        errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
        //        return false;
        //    }
        //    foreach (char c in line)
        //    {
        //    }
        //}

        errorMessage = "Input validation not yet implemented.";
        return false;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        return output.WriteAnswers(null, null);
    }
}
