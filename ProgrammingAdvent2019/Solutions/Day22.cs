// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Numerics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day22 : Day
{
    private static readonly Regex _validLine = new("^(deal into new stack|deal with increment (?<increment>[0-9]{1,2})|cut (?<cut>-?[0-9]{1,4}))$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid input line \"{line.Left(20, true)}\".";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        (int, int)[] instructions = new(int, int)[inputLines.Length];
        for (int i = 0; i < instructions.Length; i++)
        {
            string line = inputLines[i];
            Match match = _validLine.Match(line);
            if (match.Groups["increment"].Success)
            {
                int N = int.Parse(match.Groups["increment"].Value);
                instructions[i] = (2, N);
            }
            else if (match.Groups["cut"].Success)
            {
                int N = int.Parse(match.Groups["cut"].Value);
                instructions[i] = (1, N);
            }
            else
            {
                instructions[i] = (0, 0);
            }
        }

        string partOneAnswer;
        if (exampleModifier is null)
        {
            partOneAnswer = ShuffledPosition(2019, 10_007, instructions).ToString();
        }
        else
        {
            int[] results = new int[10];
            for (int i = 0; i < 10; i++)
            {
                results[ShuffledPosition(i, 10, instructions)] = i;
            }
            partOneAnswer = string.Join(' ', results);
        }

        long deckSize = 119315717514047;
        (long A, long B) = ReverseEquationCoefficients(deckSize, instructions);
        long iterations = 101741582076661;
        BigInteger A_pow_N = BigInteger.ModPow(A, iterations, deckSize);
        BigInteger result = A_pow_N * 2020 + B * (A_pow_N - 1) * ModularInverse(A - 1, deckSize);
        if (result < 0)
        {
            result += deckSize * (-result / deckSize + 1);
        }
        result = BigInteger.ModPow(result, 1, deckSize);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static long NewStack(long position, long deckSize)
    {
        return deckSize - position - 1;
    }

    private static long Cut(long position, long deckSize, int N)
    {
        if (N > 0)
        {
            if (position < N)
            {
                return deckSize - N + position;
            }
            else
            {
                return position - N;
            }
        }
        else if (N < 0)
        {
            if (position < deckSize + N)
            {
                return position - N;
            }
            else
            {
                return position - deckSize - N;
            }
        }
        else return position;
    }

    private static long Increment(long position, long deckSize, int N)
    {
        return (position * N) % deckSize;
    }

    private static long ApplyTechnique((int, int) technique, long position, long deckSize)
    {
        return technique.Item1 switch
        {
            2 => Increment(position, deckSize, technique.Item2),
            1 => Cut(position, deckSize, technique.Item2),
            _ => NewStack(position, deckSize),
        };
    }

    private static long ShuffledPosition(long position, long deckSize, (int, int)[] instructions)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            position = ApplyTechnique(instructions[i], position, deckSize);
        }
        return position;
    }

    private static long ReverseIncrement(long position, long deckSize, long N)
    {
        int wraps = 0;
        while ((position + wraps * deckSize) % N != 0)
        {
            wraps++;
        }
        return (position + wraps * deckSize) / N;
        // Could instead return Increment(position, deckSize, ModularInverse(N, deckSize))
        // but that would require more work to avoid integer overflows.
    }

    private static long ReverseTechnique((int, int) technique, long position, long deckSize)
    {
        return technique.Item1 switch
        {
            2 => ReverseIncrement(position, deckSize, technique.Item2),
            1 => Cut(position, deckSize, -technique.Item2),
            _ => NewStack(position, deckSize),
        };
    }

    private static long ReverseShuffledPosition(long position, long deckSize, (int, int)[] instructions)
    {
        for (int i = instructions.Length - 1; i >= 0; i--)
        {
            position = ReverseTechnique(instructions[i], position, deckSize);
        }
        return position;
    }

    private static (long, long) ReverseEquationCoefficients(long deckSize, (int, int)[] instructions)
    {
        long p0 = 2020;
        long p1 = ReverseShuffledPosition(p0, deckSize, instructions);
        long p2 = ReverseShuffledPosition(p1, deckSize, instructions);
        BigInteger m1 = BigInteger.Multiply(p2 - p1, ModularInverse(p1 - p0, deckSize));
        if (m1 < 0)
        {
            m1 += deckSize * (-m1 / deckSize + 1);
        }
        long A = (long)BigInteger.ModPow(m1, 1, deckSize);
        long B = p1 - A * p0;
        if (B < 0)
        {
            B += deckSize * (-B / deckSize + 1);
        }
        return (A, B);
    }

    // https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm#Modular_integers
    private static long ModularInverse(long integer, long modulus)
    {
        long t = 0, newT = 1;
        long r = modulus, newR = integer;
        while (newR != 0)
        {
            long quotient = r / newR;
            (t, newT) = (newT, t - quotient * newT);
            (r, newR) = (newR, r - quotient * newR);
        }
        if (t < 0)
        {
            t += modulus;
        }
        return t;
    }
}
