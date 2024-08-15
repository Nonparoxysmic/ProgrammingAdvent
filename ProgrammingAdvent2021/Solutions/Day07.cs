// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day07 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] initialPositions = input[0].Split(',').Select(str => int.Parse(str)).ToArray();
        Array.Sort(initialPositions);
        int len = initialPositions.Length;
        int partOnePosition = len % 2 == 1
            ? initialPositions[len / 2]
            : (initialPositions[len / 2] + initialPositions[len / 2 - 1]) / 2;
        int partOneAnswer = initialPositions
            .Aggregate(0, (total, next) => total + Math.Abs(partOnePosition - next));
        return ($"{partOneAnswer}", "n/a");
    }
}
