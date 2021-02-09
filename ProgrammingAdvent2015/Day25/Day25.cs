// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.IO;

namespace ProgrammingAdvent2015
{
    static class Day25
    {
        public static void Solve()
        {
            string input1Path = @"Day25\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 1 || input1[0].Length < 92) Print.PrintErrorAndExit("Day 25: Insufficent data in input file " + input1Path);
            string[] words = input1[0].Split();
            if (words.Length < 19) Print.PrintErrorAndExit("Day 25: Insufficent data in input file " + input1Path);
            if (!int.TryParse(words[16].Trim(','), out int inputRow))
            {
                Print.PrintErrorAndExit("Day 25: Cannot parse as an integer \"" + words[16].Trim(',') + "\" in " + input1Path);
            }
            if (!int.TryParse(words[18].Trim('.'), out int inputCol))
            {
                Print.PrintErrorAndExit("Day 25: Cannot parse input integers in " + input1Path);
            }

            Console.WriteLine("Day 25 Answer: " + GetCode(GetCodeNumber(inputRow, inputCol)));
        }

        static int GetCodeNumber(int row, int col)
        {
            int previousDiagonalLastColumn = row + col - 2;
            int sum = 0;
            for (int i = 1; i <= previousDiagonalLastColumn; i++)
            {
                sum += i;
            }
            return sum + col;
        }

        static int GetCode(int codeNumber)
        {
            long code = 20151125;
            for (int i = 1; i < codeNumber; i++)
            {
                code = (code * 252533) % 33554393;
            }
            return (int)code;
        }
    }
}
