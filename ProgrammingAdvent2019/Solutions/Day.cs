// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Diagnostics;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal abstract class Day
{
    public PuzzleAnswers Solve(string[] input)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        PuzzleAnswers answers = CalculateAnswers(input);
        stopwatch.Stop();
        return answers.WriteDayNumber(this).WriteTime(stopwatch);
    }

    public abstract bool ValidateInput(string[] input, out string errorMessage);

    protected abstract PuzzleAnswers CalculateAnswers(string[] input);
}
