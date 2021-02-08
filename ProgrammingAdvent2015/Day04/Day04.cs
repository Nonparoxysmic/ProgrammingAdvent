// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProgrammingAdvent2015
{
    static class Day04
    {
        public static void Solve()
        {
            string inputPath = @"Day04\Puzzle\Input1.txt";
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

            Console.WriteLine("Day 4 may take a moment to calculate. Please wait...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            byte[] hashFunctionInput = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(input) + 1);
            bool fiveLeadingZeroesFound = false;
            bool sixLeadingZeroesFound = false;
            for (int i = 1; i < int.MaxValue; i++)
            {
                byte[] hash = AdventMD5.ComputeHash(hashFunctionInput);
                if (hash[0] == 0 && hash[1] == 0)
                {
                    if (hash[2] < 0x10 && !fiveLeadingZeroesFound)
                    {
                        Console.WriteLine("Day 4 Part One Answer: " + i);
                        fiveLeadingZeroesFound = true;
                    }
                    if (hash[2] == 0 && !sixLeadingZeroesFound)
                    {
                        sw.Stop();
                        Console.WriteLine("Day 4 Part Two Answer: {0} (Time: {1:F1} seconds)", i, sw.ElapsedMilliseconds * 0.001);
                        sixLeadingZeroesFound = true;
                    }
                }
                if (fiveLeadingZeroesFound && sixLeadingZeroesFound) break;
                
                // Recalculate the hash function input if the last byte reaches '9' (57):
                if (hashFunctionInput[hashFunctionInput.GetUpperBound(0)] > 56)
                {
                    hashFunctionInput = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(input) + (i + 1));
                }
                // Increment the counter on the hash function input by incrementing the last byte:
                else
                {
                    hashFunctionInput[hashFunctionInput.GetUpperBound(0)]++;
                }
            }
        }
    }
}
