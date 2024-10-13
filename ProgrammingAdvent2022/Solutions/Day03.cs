// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day03 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int sumOfPriorities = 0;
        foreach (string line in input)
        {
            char sharedItem = SharedItem(line);
            sumOfPriorities += sharedItem switch
            {
                < '[' => sharedItem - '&',
                > '`' => sharedItem - '`',
                _ => -32768
            };
        }

        return result.WriteAnswers(sumOfPriorities, null);
    }

    private static char SharedItem(string items)
    {
        for (int i = 0; i < items.Length / 2; i++)
        {
            for (int j = items.Length / 2; j < items.Length; j++)
            {
                if (items[i] == items[j])
                {
                    return items[i];
                }
            }
        }
        return '^';
    }
}
