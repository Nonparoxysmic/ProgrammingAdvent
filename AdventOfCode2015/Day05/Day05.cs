// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day05
    {
        public static void Solve()
        {
            string input1Path = @"Day05\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            int numOfNiceStrings = 0;
            foreach (string line in input1)
            {
                int numOfVowels = 0;
                bool hasDoubleLetters = false;
                bool containsUnallowedPairs = false;

                if (line.Length > 2)
                {
                    if (IsVowel(line[0])) numOfVowels++;
                    for (int i = 1; i < line.Length; i++)
                    {
                        if (IsVowel(line[i])) numOfVowels++;
                        if (line[i] == line[i - 1]) hasDoubleLetters = true;
                        if ((line[i] == 'b') && (line[i - 1] == 'a'))
                        {
                            containsUnallowedPairs = true;
                        }
                        else if ((line[i] == 'd') && (line[i - 1] == 'c'))
                        {
                            containsUnallowedPairs = true;
                        }
                        else if ((line[i] == 'q') && (line[i - 1] == 'p'))
                        {
                            containsUnallowedPairs = true;
                        }
                        else if ((line[i] == 'y') && (line[i - 1] == 'x'))
                        {
                            containsUnallowedPairs = true;
                        }
                    }
                }

                if ((numOfVowels > 2) && hasDoubleLetters && !containsUnallowedPairs)
                {
                    numOfNiceStrings++;
                }
            }

            Console.WriteLine("Day 5 Part One Answer: " + numOfNiceStrings);
        }

        static bool IsVowel(char c)
        {
            return (c == 'a') || (c == 'e') || (c == 'i') || (c == 'o') || (c == 'u');
        }
    }
}
