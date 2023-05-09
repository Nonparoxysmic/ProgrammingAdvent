// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

namespace ProgrammingAdvent2019.Common;

internal static class MathS
{
    /// <summary>
    /// Calculates the greatest common divisor of a pair of integers.
    /// </summary>
    public static int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }

    /// <summary>
    /// Calculates the greatest common divisor of a pair of integers.
    /// </summary>
    public static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }

    /// <summary>
    /// Calculates the least common multiple of a collection of integers.
    /// </summary>
    public static long LCM(params long[] values)
    {
        return values.Length switch
        {
            0 => 0,
            1 => values[0],
            2 => values[0] / GCD(values[0], values[1]) * values[1],
            _ => values.Aggregate((a, b) => a / GCD(a, b) * b)
        };
    }
}
