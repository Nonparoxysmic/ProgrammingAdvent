// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Diagnostics;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal abstract class Day
{
    public PuzzleAnswers Solve(string[] input, string? exampleModifier = null)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        PuzzleAnswers answers = CalculateAnswers(input, exampleModifier);
        stopwatch.Stop();
        return answers.WriteDayNumber(this).WriteTime(stopwatch);
    }

    public abstract bool ValidateInput(string[] input, out string errorMessage);

    protected abstract PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null);
}
