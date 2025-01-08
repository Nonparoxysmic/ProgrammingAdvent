// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

namespace ProgrammingAdvent2023.Common;

internal static class MoreMath
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

    /// <summary>
    /// Returns the largest of a number of 32-bit signed integers.
    /// </summary>
    /// <returns>The largest parameter.</returns>
    /// <param name="values">The set of 32-bit signed integers to compare.</param>
    public static int Max(params int[] values)
    {
        return values.Aggregate((a, b) => Math.Max(a, b));
    }

    /// <summary>
    /// Returns the smallest of a number of 32-bit signed integers.
    /// </summary>
    /// <returns>The smallest parameter.</returns>
    /// <param name="values">The set of 32-bit signed integers to compare.</param>
    public static int Min(params int[] values)
    {
        return values.Aggregate((a, b) => Math.Min(a, b));
    }

    /// <summary>
    /// Returns a specified integer raised to a specified integer power.
    /// </summary>
    /// <remarks>
    /// This method returns zero when the result would be fractional and 
    /// returns 1 for the case of zero to the power of zero.
    /// </remarks>
    /// <returns>
    /// The number x raised to the power y.
    /// </returns>
    /// <exception cref="DivideByZeroException"/>
    /// <param name="x">An integer number to be raised to a power.</param>
    /// <param name="y">An integer number that specifies a power.</param>
    public static int PowInt(int x, int y)
    {
        return y switch
        {
            < 0 => x switch
            {
                0 => throw new DivideByZeroException(),
                -1 => PowInt(x, -y),
                1 => 1,
                _ => 0
            },
            0 => 1,
            1 => x,
            2 => x * x,
            3 => x * x * x,
            4 => x * x * x * x,
            5 => x * x * x * x * x,
            _ => (int)Math.Pow(x, y)
        };
    }
}
