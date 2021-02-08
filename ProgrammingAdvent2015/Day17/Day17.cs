// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.IO;

namespace ProgrammingAdvent2015
{
    static class Day17
    {
        const int target = 150;
        static int numOfCombinations = 0;
        static int minContainersUsed = int.MaxValue;
        static int minContainerCombinations = 0;

        public static void Solve()
        {
            string inputPath = @"Day17\Puzzle\Input1.txt";
            int[] input = null;
            try
            {
                string[] inputRaw = File.ReadAllLines(inputPath);
                input = new int[inputRaw.Length];
                for (int i = 0; i < inputRaw.Length; i++) input[i] = int.Parse(inputRaw[i]);
                Array.Sort(input);
                Array.Reverse(input);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + inputPath + Environment.NewLine + e.GetType());
            }

            CalculateCombinations(input, 0, 0, 0);

            Console.WriteLine("Day 17 Part One Answer: " + numOfCombinations);
            Console.WriteLine("Day 17 Part Two Answer: " + minContainerCombinations);
        }

        static void CalculateCombinations(int[] input, int pos, int quantitySoFar, int containersUsed)
        {
            for (int i = pos; i < input.Length; i++)
            {
                if (quantitySoFar + input[i] == target)
                {
                    numOfCombinations++;
                    if (containersUsed + 1 == minContainersUsed)
                    {
                        minContainerCombinations++;
                    }
                    else if (containersUsed + 1 < minContainersUsed)
                    {
                        minContainersUsed = containersUsed + 1;
                        minContainerCombinations = 1;
                    }
                    continue;
                }
                else if (quantitySoFar + input[i] < target)
                {
                    CalculateCombinations(input, i + 1, quantitySoFar + input[i], containersUsed + 1);
                }
            }
        }
    }
}
