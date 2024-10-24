// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day10 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int[] signal = InputToSignal(input);
        int partOneAnswer = 20 * signal[20] + 60 * signal[60] + 100 * signal[100]
            + 140 * signal[140] + 180 * signal[180] + 220 * signal[220];

        return result.WriteAnswers(partOneAnswer, null);
    }

    private static int[] InputToSignal(string[] input)
    {
        int[] signal = new int[Math.Max(221, input.Length * 2)];
        signal[0] = 1;
        int cycle = 1, currentSignal = 1;
        foreach (string line in input)
        {
            if (line == "noop")
            {
                signal[cycle++] = currentSignal;
            }
            else if (int.TryParse(line[5..], out int offset))
            {
                signal[cycle++] = currentSignal;
                signal[cycle++] = currentSignal;
                currentSignal += offset;
            }
        }
        return signal;
    }
}
