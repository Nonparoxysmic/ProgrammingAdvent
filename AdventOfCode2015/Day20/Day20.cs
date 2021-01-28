// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day20
    {
        public static void Solve()
        {
            string inputPath = @"Day20\Puzzle\Input1.txt";
            string[] inputRaw = null;
            try
            {
                inputRaw = File.ReadAllLines(inputPath);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + inputPath + Environment.NewLine + e.GetType());
            }
            if (inputRaw.Length == 0) Print.PrintErrorAndExit("Day 19: No input data in " + inputPath);
            if (!int.TryParse(inputRaw[0], out int input))
            {
                Print.PrintErrorAndExit("Day 19: Cannot parse \"" + inputRaw[0] + "\" as an integer.");
            }

            int houseNumber = 0;
            while (true)
            {
                houseNumber++;
                int gifts = 10 * SumOfDivisors(houseNumber);
                if (gifts >= input) break;
            }

            Console.WriteLine("Day 20 Part One Answer: " + houseNumber);
        }

        static int SumOfDivisors(int number)
        {
            if (number < 2) return number;
            double squareRoot = Math.Sqrt(number);
            int sum = number + 1;
            for (int i = 2; i < squareRoot; i++)
            {
                if (number % i == 0) sum += number / i + i;
            }
            if (squareRoot % 1 == 0) sum += (int)squareRoot;
            return sum;
        }
    }
}
