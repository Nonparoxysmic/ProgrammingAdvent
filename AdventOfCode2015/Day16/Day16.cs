// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2015
{
    static class Day16
    {
        public static void Solve()
        {
            string input1Path = @"Day16\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            List<string> potentialMatches = new List<string>();

            foreach (string line in input1)
            {
                if (line.Contains("children: ") && !line.Contains("children: 3"))
                {
                    continue;
                }
                if (line.Contains("cats: ") && !line.Contains("cats: 7"))
                {
                    continue;
                }
                if (line.Contains("samoyeds: ") && !line.Contains("samoyeds: 2"))
                {
                    continue;
                }
                if (line.Contains("pomeranians: ") && !line.Contains("pomeranians: 3"))
                {
                    continue;
                }
                if (line.Contains("akitas: ") && !line.Contains("akitas: 0"))
                {
                    continue;
                }
                if (line.Contains("vizslas: ") && !line.Contains("vizslas: 0"))
                {
                    continue;
                }
                if (line.Contains("goldfish: ") && !line.Contains("goldfish: 5"))
                {
                    continue;
                }
                if (line.Contains("trees: ") && !line.Contains("trees: 3"))
                {
                    continue;
                }
                if (line.Contains("cars: ") && !line.Contains("cars: 2"))
                {
                    continue;
                }
                if (line.Contains("perfumes: ") && !line.Contains("perfumes: 1"))
                {
                    continue;
                }
                potentialMatches.Add(line);
            }

            if (potentialMatches.Count == 0)
            {
                Console.WriteLine("Day 16 Part One: No Valid Answer");
            }
            else if (potentialMatches.Count == 1)
            {
                string sueNumber = potentialMatches[0].Split()[1].Trim(':');
                Console.WriteLine("Day 16 Part One Answer: " + sueNumber);
            }
            else
            {
                // Manual check by user. Didn't need this for my solution.
                Console.WriteLine("Day 16 Part One: Compare the following potential matches manually.");
                foreach (string sue in potentialMatches) Console.WriteLine(" >> " + sue);
            }
        }
    }
}
