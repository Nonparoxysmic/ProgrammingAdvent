// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day06 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int indexOfStartOfPacket = IndexOfStartOfPacket(input[0]);
        int indexOfStartOfMessage = IndexOfStartOfMessage(input[0]);

        return result.WriteAnswers(indexOfStartOfPacket, indexOfStartOfMessage);
    }

    private static int IndexOfStartOfPacket(string signal)
    {
        for (int i = 3; i < signal.Length; i++)
        {
            if (signal[i] != signal[i - 1] && signal[i] != signal[i - 2]
                && signal[i] != signal[i - 3] && signal[i - 1] != signal[i - 2]
                && signal[i - 1] != signal[i - 3] && signal[i - 2] != signal[i - 3])
            {
                return i + 1;
            }
        }
        return -1;
    }

    private static int IndexOfStartOfMessage(string signal)
    {
        if (signal.Length < 14)
        {
            return -1;
        }
        int[] charCounts = new int[26];
        for (int i = 0; i < 14; i++)
        {
            charCounts[signal[i] - 'a']++;
        }
        for (int i = 14; i <= signal.Length; i++)
        {
            if (charCounts.Max() == 1)
            {
                return i;
            }
            if (i == signal.Length)
            {
                break;
            }
            charCounts[signal[i - 14] - 'a']--;
            charCounts[signal[i] - 'a']++;
        }
        return -1;
    }
}
