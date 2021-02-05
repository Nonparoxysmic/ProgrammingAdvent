// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2015
{
    static class Day24
    {
        static int groupWeight = -1;
        static long idealEntanglement = long.MaxValue;

        public static void Solve()
        {
            string input1Path = @"Day24\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 3) Print.PrintErrorAndExit("Day 24: Insufficent data in input file " + input1Path);
            List<int> packageWeights = new List<int>();
            int totalWeight = 0;
            for (int i = 0; i < input1.Length; i++)
            {
                if (!int.TryParse(input1[i], out int weight))
                {
                    Print.PrintErrorAndExit("Day 24: Cannot parse as an integer \"" + input1[i] + "\" in " + input1Path);
                }
                totalWeight += weight;
                packageWeights.Add(weight);
            }
            if (totalWeight < 3 || totalWeight % 3 != 0) Print.PrintErrorAndExit("Day 24: Invalid sum of values in " + input1Path);
            groupWeight = totalWeight / 3;
            packageWeights.Sort();
            packageWeights.Reverse();

            FindPartOne(packageWeights);

            Console.WriteLine("Day 24 Part One Answer: " + idealEntanglement);
        }

        static bool CanBeDividedEvenly(List<int> input)
        {
            int listSum = input.Sum();
            if (listSum % 2 != 0) return false;
            return CanBeDividedEvenly(input, listSum / 2, 0, 0);
        }

        static bool CanBeDividedEvenly(List<int> input, int groupSum, int index, int sumSoFar)
        {
            for (int i = index; i < input.Count; i++)
            {
                if (sumSoFar + input[i] == groupSum) return true;
                else if (sumSoFar + input[i] < groupSum)
                {
                    if (CanBeDividedEvenly(input, groupSum, i + 1, sumSoFar + input[i])) return true;
                }
            }
            return false;
        }

        static int LengthOfBestThird(List<int> input) => LengthOfBestThird(input, new List<int>(), 0, 0, 0);

        static int LengthOfBestThird(List<int> input, List<int> output, int index, int sumSoFar, int numSoFar)
        {
            for (int i = index; i < input.Count; i++)
            {
                if (sumSoFar + input[i] == groupWeight)
                {
                    List<int> remainingInput = new List<int>(input);
                    foreach (int j in output) remainingInput.Remove(j);
                    remainingInput.Remove(input[i]);
                    if (CanBeDividedEvenly(remainingInput))
                    {
                        return numSoFar + 1;
                    }
                }
                else if (sumSoFar + input[i] < groupWeight)
                {
                    List<int> nextOutput = new List<int>(output)
                    {
                        input[i]
                    };
                    return LengthOfBestThird(input, nextOutput, i + 1, sumSoFar + input[i], numSoFar + 1);
                }
            }
            return -1;
        }

        static void FindPartOne(List<int> input)
        {
            int combinationLength = LengthOfBestThird(input);
            FindPartOne(input, combinationLength, 0, 0, 0, 1);
        }

        static void FindPartOne(List<int> input, int combinationLength, int currentDepth, int index, int sumSoFar, long productSoFar)
        {
            for (int i = index; i < input.Count - combinationLength + currentDepth + 1; i++)
            {
                if (sumSoFar + input[i] == groupWeight)
                {
                    idealEntanglement = Math.Min(idealEntanglement, productSoFar * input[i]);
                }
                else if (sumSoFar + input[i] < groupWeight)
                {
                    if (currentDepth + 1 < combinationLength)
                    {
                        FindPartOne(input, combinationLength, currentDepth + 1, i + 1, sumSoFar + input[i], productSoFar * input[i]);
                    }
                }
            }
        }
    }
}
