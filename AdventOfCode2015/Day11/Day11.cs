// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day11
    {
        public static void Solve()
        {
            string inputPath = @"Day11\Puzzle\Input1.txt";
            byte[] inputRaw = new byte[0];
            byte[] input;
            try
            {
                inputRaw = File.ReadAllBytes(inputPath);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + inputPath + Environment.NewLine + e.GetType());
            }
            // Remove the Unicode byte order mark if there is one at the beginning of the file:
            if (inputRaw[0] == 239 && inputRaw[1] == 187 && inputRaw[2] == 191)
            {
                input = new byte[inputRaw.Length - 3];
                for (int i = 0; i < input.Length; i++)
                {
                    input[i] = inputRaw[i + 3];
                }
            }
            else
            {
                input = inputRaw;
            }
            if (input.Length != 8) Print.PrintErrorAndExit("Day 11: Invalid input length in Input1.txt");
            foreach (byte b in input)
            {
                if ((b < 97) || (b > 122)) Print.PrintErrorAndExit("Day 11: Invalid character '" + (char)b + "' in Input1.txt");
            }

            byte[] nextValidPassword = input;
            while (true)
            {
                nextValidPassword = IncrementLetterByteArray(nextValidPassword);
                if (PasswordIsValid(nextValidPassword)) break;
            }

            Console.WriteLine("Day 11 Part One Answer: " + ByteArrayToString(nextValidPassword));

            while (true)
            {
                nextValidPassword = IncrementLetterByteArray(nextValidPassword);
                if (PasswordIsValid(nextValidPassword)) break;
            }

            Console.WriteLine("Day 11 Part Two Answer: " + ByteArrayToString(nextValidPassword));
        }

        static string ByteArrayToString(byte[] input)
        {
            if ((input == null) || (input.Length == 0)) return "";
            return System.Text.Encoding.UTF8.GetString(input);
        }

        static byte[] IncrementLetterByteArray(byte[] input)
        {
            if (input.Length == 0) return input;

            input[input.Length - 1]++;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if ((input[i] == 105) || (input[i] == 111) || (input[i] == 108))
                {
                    input[i]++;
                    for (int j = i + 1; j < input.Length; j++)
                    {
                        input[j] = 97;
                    }
                    return input;
                }
                if (input[i] > 122)
                {
                    input[i] = 97;
                    if (i > 0) input[i - 1]++;
                }
                else break;
            }

            return input;
        }

        static bool PasswordIsValid(byte[] input)
        {
            bool containsStraight = false;
            for (int i = 0; i < input.Length - 3; i++)
            {
                if ((input[i] + 1 == input[i + 1]) && (input[i + 1] + 1 == input[i + 2]))
                {
                    containsStraight = true;
                    break;
                }
            }
            if (!containsStraight) return false;

            bool containsPairs = false;
            int pairsFound = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                {
                    pairsFound++;
                    i++;
                }
                if (pairsFound > 1)
                {
                    containsPairs = true;
                    break;
                }
            }
            if (!containsPairs) return false;

            // Made redundant by the check in IncrementLetterByteArray
            //foreach (byte b in input)
            //{
            //    if ((b == 105) || (b == 111) || (b == 108)) return false;
            //}

            return true;
        }
    }
}
