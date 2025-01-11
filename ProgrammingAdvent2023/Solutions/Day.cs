// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

using System.Diagnostics;
using ProgrammingAdvent2023.Data;

namespace ProgrammingAdvent2023.Solutions;

internal abstract class Day
{
    public PuzzleAnswers Solve(string[] input, string? exampleModifier = null)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        PuzzleAnswers answers = CalculateAnswers(input, exampleModifier);
        stopwatch.Stop();
        return answers.WriteDayNumber(this).WriteTime(stopwatch);
    }

    protected abstract PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null);
}
