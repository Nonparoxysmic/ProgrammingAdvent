// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day24 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] A = new int[14];
        int[] B = new int[14];
        try
        {
            for (int i = 0; i < A.Length; i++)
            {
                A[i] = int.Parse(input[5 + i * 18][6..^0]);
                B[i] = int.Parse(input[15 + i * 18][6..^0]);
            }
        }
        catch
        {
            return ("Invalid input", "n/a");
        }

        Stack<int> indices = [];
        List<(int, int)> matchingIndices = [];
        for (int i = 0; i < 14; i++)
        {
            if (A[i] >= 0)
            {
                indices.Push(i);
                continue;
            }
            matchingIndices.Add((indices.Pop(), i));
        }
        if (indices.Count > 0 || matchingIndices.Count != 7)
        {
            return ("Invalid input", "n/a");
        }

        int[] partOneAnswer = new int[14];
        foreach ((int i, int j) in matchingIndices)
        {
            int diff = B[i] + A[j];
            if (diff >= 0)
            {
                partOneAnswer[j] = 9;
                partOneAnswer[i] = 9 - diff;
            }
            else
            {
                partOneAnswer[i] = 9;
                partOneAnswer[j] = 9 + diff;
            }
        }

        return (string.Join(null, partOneAnswer), "n/a");
    }
}
