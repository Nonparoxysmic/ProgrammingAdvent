// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Numerics;

namespace ProgrammingAdvent2021.Solutions;

internal class Day08 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] outputLengths = new int[8];
        int partTwoAnswer = 0;
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
            partTwoAnswer += OutputValue(terms);
        }
        int partOneAnswer = outputLengths[2] + outputLengths[3]
            + outputLengths[4] + outputLengths[7];
        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static int OutputValue(string[] terms)
    {
        int[] encodings = new int[10];
        List<int> twoThreeFive = new(3);
        List<int> zeroSixNine = new(3);
        for (int i = 0; i < 10; i++)
        {
            switch (terms[i].Length)
            {
                case 2:
                    encodings[1] = Encode(terms[i]);
                    break;
                case 3:
                    encodings[7] = Encode(terms[i]);
                    break;
                case 4:
                    encodings[4] = Encode(terms[i]);
                    break;
                case 5:
                    twoThreeFive.Add(Encode(terms[i]));
                    break;
                case 6:
                    zeroSixNine.Add(Encode(terms[i]));
                    break;
                case 7:
                    encodings[8] = Encode(terms[i]);
                    break;
                default:
                    break;
            }
        }
        foreach (int i in twoThreeFive)
        {
            if ((i & encodings[1]) == encodings[1])
            {
                encodings[3] = i;
            }
            else if (BitOperations.PopCount((uint)(i & encodings[4])) == 3)
            {
                encodings[5] = i;
            }
            else
            {
                encodings[2] = i;
            }
        }
        foreach (int i in zeroSixNine)
        {
            if ((i & encodings[1]) != encodings[1])
            {
                encodings[6] = i;
            }
            else if ((i & encodings[4]) == encodings[4])
            {
                encodings[9] = i;
            }
            else
            {
                encodings[0] = i;
            }
        }
        int result = 0;
        for (int i = 11; i < 15; i++)
        {
            result *= 10;
            result += Array.IndexOf(encodings, Encode(terms[i]));
        }
        return result;
    }

    private static int Encode(string term)
    {
        int encoding = 0;
        foreach (char c in term)
        {
            encoding |= 1 << (c - 'a');
        }
        return encoding;
    }
}
