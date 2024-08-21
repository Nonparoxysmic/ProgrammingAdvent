// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day08 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] outputLengths = new int[8];
        foreach (string line in input)
        {
            string[] terms = line.Split(' ');
            if (terms.Length != 15)
            {
                return ("Invalid line in input.", "n/a");
            }
            for (int i = 11; i < 15; i++)
            {
                outputLengths[terms[i].Length]++;
            }
        }
        int partOneAnswer = outputLengths[2] + outputLengths[3]
            + outputLengths[4] + outputLengths[7];

        return ($"{partOneAnswer}", "n/a");
    }
}
