// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal abstract class Day
{
    public void Solve(string[] input)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        (string, string) answers = CalculateAnswers(input);
        stopwatch.Stop();

        int dayNumber = int.Parse(GetType().ToString()[^2..^0]);
        Console.WriteLine($"Day {dayNumber:00} Part One: {answers.Item1}");
        Console.WriteLine($"       Part Two: {answers.Item2}");
        Console.WriteLine($"       Time: {stopwatch.ElapsedMilliseconds + 1} ms");
    }

    public (string, string) Test(string[] input)
    {
        return CalculateAnswers(input);
    }

    protected abstract (string, string) CalculateAnswers(string[] input);

    public static bool TryGetSolution(int dayNumber, [NotNullWhen(true)] out Day? solution)
    {
        if (Reflection.DayTypes.TryGetValue(dayNumber, out Type? dayType))
        {
            solution = (Day?)Activator.CreateInstance(dayType);
            return solution is not null;
        }
        else
        {
            solution = null;
            return false;
        }
    }
}
