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
        int tailPositions = TailPositions(motions);

        return result.WriteAnswers(tailPositions, null);
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

    private static int TailPositions((char, int)[] motions)
    {
        int Hx = 0, Hy = 0, Tx = 0, Ty = 0;
        HashSet<(int, int)> positions = [];
        positions.Add((0, 0));
        foreach ((char direction, int steps) in motions)
        {
            for (int i = 0; i < steps; i++)
            {
                switch (direction)
                {
                    case 'U':
                        Hy++;
                        break;
                    case 'D':
                        Hy--;
                        break;
                    case 'L':
                        Hx--;
                        break;
                    case 'R':
                        Hx++;
                        break;
                    default:
                        break;
                }
                if (Math.Abs(Tx - Hx) > 1 || Math.Abs(Ty - Hy) > 1)
                {
                    Tx += Math.Sign(Hx - Tx);
                    Ty += Math.Sign(Hy - Ty);
                    positions.Add((Tx, Ty));
                }
            }
        }
        return positions.Count;
    }
}
