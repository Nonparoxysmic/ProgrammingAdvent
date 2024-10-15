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
                <= 'Z' => sharedItem - '&',
                >= 'a' => sharedItem - '`',
                _ => -32768
            };
        }

        int sumOfBadgePriorities = 0;
        for (int i = 0; i < input.Length - 2; i += 3)
        {
            char sharedItem = SharedItem(input[i], input[i + 1], input[i + 2]);
            sumOfBadgePriorities += sharedItem switch
            {
                <= 'Z' => sharedItem - '&',
                >= 'a' => sharedItem - '`',
                _ => -32768
            };
        }

        return result.WriteAnswers(sumOfPriorities, sumOfBadgePriorities);
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

    private static char SharedItem(string items1, string items2, string items3)
    {
        for (int i = 0; i < items1.Length; i++)
        {
            for (int j = 0; j < items2.Length; j++)
            {
                for (int k = 0; k < items3.Length; k++)
                {
                    if (items1[i] == items2[j] && items2[j] == items3[k])
                    {
                        return items1[i];
                    }
                }
            }
        }
        return '^';
    }
}
