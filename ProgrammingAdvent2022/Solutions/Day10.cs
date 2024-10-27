// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text;
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
        char[] lettersOnScreen = ProcessSignal(signal);

        return result.WriteAnswers(partOneAnswer, lettersOnScreen);
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

    private static char[] ProcessSignal(int[] signal)
    {
        char[] letters = new char[8];
        StringBuilder sb = new();
        for (int i = 0; i < 8; i++)
        {
            sb.Clear();
            for (int y = 0; y < 6; y++)
            {
                for (int x = i * 5; x < i * 5 + 5; x++)
                {
                    int cycle = 40 * y + x + 1;
                    if (Math.Abs(x - signal[cycle]) > 1)
                    {
                        sb.Append('.');
                    }
                    else
                    {
                        sb.Append('#');
                    }
                }
            }
            letters[i] = ReadLetter(sb.ToString());
        }
        return letters;
    }

    public static char ReadLetter(string pixels)
    {
        return pixels switch
        {
            ".##..#..#.#..#.####.#..#.#..#." => 'A',
            "###..#..#.###..#..#.#..#.###.." => 'B',
            ".##..#..#.#....#....#..#..##.." => 'C',
            "####.#....###..#....#....####." => 'E',
            "####.#....###..#....#....#...." => 'F',
            ".##..#..#.#....#.##.#..#..###." => 'G',
            "#..#.#..#.####.#..#.#..#.#..#." => 'H',
            ".###...#....#....#....#...###." => 'I',
            "..##....#....#....#.#..#..##.." => 'J',
            "#..#.#.#..##...#.#..#.#..#..#." => 'K',
            "#....#....#....#....#....####." => 'L',
            ".##..#..#.#..#.#..#.#..#..##.." => 'O',
            "###..#..#.#..#.###..#....#...." => 'P',
            "###..#..#.#..#.###..#.#..#..#." => 'R',
            ".###.#....#.....##.....#.###.." => 'S',
            "#..#.#..#.#..#.#..#.#..#..##.." => 'U',
            "#...##...#.#.#...#....#....#.." => 'Y',
            "####....#...#...#...#....####." => 'Z',
            _ => '?'
        };
    }
}
