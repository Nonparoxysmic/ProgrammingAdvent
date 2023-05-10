// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal abstract class Day
{
    private static readonly Regex _intcodeCharacters = new("^[0-9,-]+$");

    public PuzzleAnswers Solve(string[] input, string? exampleModifier = null)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        PuzzleAnswers answers = CalculateAnswers(input, exampleModifier);
        stopwatch.Stop();
        return answers.WriteDayNumber(this).WriteTime(stopwatch);
    }
    public static bool ValidateIntcodeInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0 || input[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!_intcodeCharacters.IsMatch(input[0]))
        {
            errorMessage = $"Input \"{input[0].Left(20, true)}\" contains invalid characters.";
            return false;
        }
        string[] terms = input[0].Split(',');
        if (terms.Length == 0)
        {
            errorMessage = "Input is just commas.";
            return false;
        }
        foreach (string term in terms)
        {
            if (!long.TryParse(term, out long _))
            {
                errorMessage = $"Term \"{term.Left(20, true)}\" cannot be parsed as an Int64.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    public abstract bool ValidateInput(string[] input, out string errorMessage);

    protected abstract PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null);
}
