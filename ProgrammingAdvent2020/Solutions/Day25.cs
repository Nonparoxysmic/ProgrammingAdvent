// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day25 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (input.Length < 2)
        {
            errorMessage = "Incomplete input";
            return false;
        }
        for (int i = 0; i < 2; i++)
        {
            if (!int.TryParse(input[i], out int key) || key < 1)
            {
                errorMessage = $"\"{input[i].Left(20, true)}\" is not a valid key.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        ulong[] publicKey = new ulong[] { ulong.Parse(input[0]), ulong.Parse(input[1]) };

        int[] loopSize = new int[2];
        ulong testKey = 1;
        for (int i = 1; i < 100_000_000; i++)
        {
            testKey = testKey * 7 % 20201227;
            if (testKey == publicKey[0])
            {
                loopSize[0] = i;
                loopSize[1] = 0;
                break;
            }
            if (testKey == publicKey[1])
            {
                loopSize[0] = 0;
                loopSize[1] = i;
                break;
            }
        }
        ulong encryptionKey;
        if (loopSize[0] != 0)
        {
            encryptionKey = TransformNumber(publicKey[1], loopSize[0]);
        }
        else if (loopSize[1] != 0)
        {
            encryptionKey = TransformNumber(publicKey[0], loopSize[1]);
        }
        else
        {
            return output.WriteError("No answer found.");
        }

        return output.WriteAnswers(encryptionKey, null);
    }

    private static ulong TransformNumber(ulong subjectNumber, int loopSize)
    {
        return ModularExponentiation(subjectNumber, (ulong)loopSize, 20201227);
    }

    // https://en.wikipedia.org/wiki/Modular_arithmetic#Example_implementations

    private static ulong ModularExponentiation(ulong a, ulong b, ulong m)
    {
        ulong r = m == 1 ? 0 : 1UL;
        while (b > 0)
        {
            if ((b & 1) > 0)
            {
                r = ModularMultiplication(r, a, m);
            }
            b >>= 1;
            a = ModularMultiplication(a, a, m);
        }
        return r;
    }

    private static ulong ModularMultiplication(ulong a, ulong b, ulong m)
    {
        if (((a | b) & (0xFFFF_FFFFUL << 32)) == 0)
        {
            return a * b % m;
        }
        ulong d = 0, mp2 = m >> 1;
        if (a >= m) { a %= m; }
        if (b >= m) { b %= m; }
        for (int i = 0; i < 64; i++)
        {
            d = (d > mp2) ? (d << 1) - m : d << 1;
            if ((a & 0x8000_0000_0000_0000UL) > 0) { d += b; }
            if (d >= m) { d -= m; }
            a <<= 1;
        }
        return d;
    }
}
