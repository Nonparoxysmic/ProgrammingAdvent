// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day01 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] values = input.Select(str => int.Parse(str)).ToArray();
        int partOneAnswer = 0, partTwoAnswer = 0;
        for (int i = 1; i < 3; i++)
        {
            if (values[i] > values[i - 1])
            {
                partOneAnswer++;
            }
        }
        for (int i = 3; i < values.Length; i++)
        {
            if (values[i] > values[i - 1])
            {
                partOneAnswer++;
            }
            if (values[i] > values[i - 3])
            {
                partTwoAnswer++;
            }
        }
        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }
}
