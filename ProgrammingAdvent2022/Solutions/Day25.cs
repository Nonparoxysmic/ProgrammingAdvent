// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day25 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        string sum = DecimalToSnafu(input.Select(s => SnafuToDecimal(s)).Sum());

        return result.WriteAnswers(sum, null);
    }

    private static decimal SnafuToDecimal(string snafu)
    {
        decimal result = 0;
        for (int i = 0; i < snafu.Length; i++)
        {
            result *= 5;
            result += snafu[i] switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new ArgumentOutOfRangeException(nameof(snafu))
            };
        }
        return result;
    }

    private static string DecimalToSnafu(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        StringBuilder sb = new();

        decimal current = value;
        while (current > 0)
        {
            int digit = (int)(current % 5);
            sb.Insert(0, digit);
            current = Math.Truncate(current / 5);
        }
        int[] base5 = sb.ToString().Select(c => c - '0').ToArray();

        int[] values = new int[32];
        Array.Copy(base5, 0, values, values.Length - base5.Length, base5.Length);
        int i = values.Length - 1;
        for (; i > 0; i--)
        {
            if (values[i] <= 2)
            {
                continue;
            }
            values[i - 1]++;
            values[i] -= 5;
        }
        i = 0;
        while (values[i] == 0)
        {
            i++;
        }
        sb.Clear();
        for (; i < values.Length; i++)
        {
            sb.Append(SnafuDigit(values[i]));
        }
        return sb.ToString();
    }

    private static char SnafuDigit(int digit)
    {
        return digit switch
        {
            -2 => '=',
            -1 => '-',
            0 => '0',
            1 => '1',
            2 => '2',
            _ => throw new ArgumentOutOfRangeException(nameof(digit))
        };
    }
}
