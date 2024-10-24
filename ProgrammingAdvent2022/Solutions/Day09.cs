// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day09 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        (char, int)[] motions = Motions(input);
        int shortTailPositions = TailPositions(motions, 2);
        int longTailPositions = TailPositions(motions, 10);

        return result.WriteAnswers(shortTailPositions, longTailPositions);
    }

    private static (char, int)[] Motions(string[] input)
    {
        try
        {
            return input.Select(s => (s[0], int.Parse(s[2..]))).ToArray();
        }
        catch
        {
            return [];
        }
    }

    private static int TailPositions((char, int)[] motions, int size)
    {
        int[] x = new int[size];
        int[] y = new int[size];
        HashSet<(int, int)> positions = [];
        positions.Add((0, 0));
        foreach ((char direction, int steps) in motions)
        {
            for (int i = 0; i < steps; i++)
            {
                switch (direction)
                {
                    case 'U':
                        y[0]++;
                        break;
                    case 'D':
                        y[0]--;
                        break;
                    case 'L':
                        x[0]--;
                        break;
                    case 'R':
                        x[0]++;
                        break;
                    default:
                        break;
                }
                for (int j = 1; j < size; j++)
                {
                    if (Math.Abs(x[j] - x[j - 1]) > 1 || Math.Abs(y[j] - y[j - 1]) > 1)
                    {
                        x[j] += Math.Sign(x[j - 1] - x[j]);
                        y[j] += Math.Sign(y[j - 1] - y[j]);
                    }
                }
                positions.Add((x[size - 1], y[size - 1]));
            }
        }
        return positions.Count;
    }
}
