// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day01
    {
        public static void Solve()
        {
            string input1Path = @"Day01\Puzzle\Input1.txt";
            string input1 = "";
            try
            {
                input1 = File.ReadAllText(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            
            int floor = 0;
            bool basementFound = false;
            int basementFoundPosition = 0;
            for (int i = 0; i < input1.Length; i++)
            {
                if (input1[i] == '(') floor++;
                else if (input1[i] == ')') floor--;
                else
                {
                    Print.PrintErrorAndExit("Unaccepted character in input file " + input1Path);
                }
                if (!basementFound && floor < 0)
                {
                    basementFoundPosition = i + 1;
                    basementFound = true;
                }
            }
            
            Console.WriteLine("Day 1 Part One Answer: " + floor);
            Console.WriteLine("Day 1 Part Two Answer: " + basementFoundPosition);
        }
    }
}
