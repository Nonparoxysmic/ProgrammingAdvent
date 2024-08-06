// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day01 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        IEnumerable<int> values = input.Select(str => int.Parse(str));
        int previous = int.MaxValue;
        int partOneAnswer = 0;
        foreach (int value in values)
        {
            if (value > previous)
            {
                partOneAnswer++;
            }
            previous = value;
        }
        return ($"{partOneAnswer}", "n/a");
    }
}
