// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Diagnostics;

namespace ProgrammingAdvent2021.Solutions;

internal abstract class Day
{
    public void Solve(string[] input)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        (string, string) answers = CalculateAnswers(input);
        stopwatch.Stop();

        int dayNumber = int.Parse(GetType().ToString()[^2..^0]);
        // TODO: Print results better
        Console.WriteLine($"Day {dayNumber}: {answers} {stopwatch.ElapsedMilliseconds}");
    }

    public (string, string) Test(string[] input)
    {
        return CalculateAnswers(input);
    }

    protected abstract (string, string) CalculateAnswers(string[] input);
}
