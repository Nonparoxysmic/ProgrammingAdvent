// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day02
    {
        public static void Solve()
        {
            string input1Path = @"Day02\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            int totalAreaNeeded = 0;
            foreach (string line in input1)
            {
                int dimension0 = 0;
                int dimension1 = 0;
                int dimension2 = 0;
                try
                {
                    string[] lineSplit = line.Split('x');
                    if (lineSplit.Length != 3) Print.PrintErrorAndExit("Incorrect formatting in input file " + input1Path);
                    dimension0 = Int32.Parse(lineSplit[0]);
                    dimension1 = Int32.Parse(lineSplit[1]);
                    dimension2 = Int32.Parse(lineSplit[2]);
                    if (dimension0 < 1 || dimension1 < 1 || dimension2 < 1)
                        Print.PrintErrorAndExit("Unaccepted data in input file " + input1Path);
                }
                catch (Exception e)
                {
                    Print.PrintErrorAndExit("Unaccepted data in input file " + input1Path + Environment.NewLine + e.GetType());
                }

                int area0 = dimension0 * dimension1;
                int area1 = dimension0 * dimension2;
                int area2 = dimension1 * dimension2;
                int areaNeeded = 2 * (area0 + area1 + area2) + Math.Min(area0, Math.Min(area1, area2));
                totalAreaNeeded += areaNeeded;
            }
            
            Console.WriteLine("Day 2 Part One Answer: " + totalAreaNeeded);
        }
    }
}
