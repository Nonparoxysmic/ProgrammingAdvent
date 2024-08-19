// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day06 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<int> initialFish = input[0].Split(',').Select(int.Parse).ToList();
        long[] populations = new long[10];
        foreach (int fish in initialFish)
        {
            populations[fish]++;
        }

        for (int day = 0; day < 80; day++)
        {
            long finished = populations[0];
            for (int i = 1; i < populations.Length; i++)
            {
                populations[i - 1] = populations[i];
            }
            populations[6] += finished;
            populations[8] += finished;
        }
        long partOneAnswer = populations.Sum();

        for (int day = 80; day < 256; day++)
        {
            long finished = populations[0];
            for (int i = 1; i < populations.Length; i++)
            {
                populations[i - 1] = populations[i];
            }
            populations[6] += finished;
            populations[8] += finished;
        }
        long partTwoAnswer = populations.Sum();

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }
}
