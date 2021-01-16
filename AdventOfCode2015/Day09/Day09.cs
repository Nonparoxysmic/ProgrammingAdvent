// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2015
{
    static class Day09
    {
        public static void Solve()
        {
            string input1Path = @"Day09\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            List<string> locations = new List<string>();

            foreach (string line in input1)
            {
                string[] data = line.Split();
                if ((data.Length != 5) || (data[1] != "to") || (data[3] != "="))
                {
                    PrintErrorInInput("\"" + line + "\"");
                    continue;
                }
                if (!int.TryParse(data[4], out int distance))
                {
                    PrintErrorInInput("as an integer \"" + data[4] + "\" in \"" + line + "\"");
                    continue;
                }

                if (!locations.Contains(data[0])) locations.Add(data[0]);
                if (!locations.Contains(data[2])) locations.Add(data[2]);



            }

            Console.WriteLine("{0} locations found:", locations.Count);
            foreach (string location in locations)
            {
                Console.WriteLine(location);
            }



            Console.WriteLine("Day 9 Part One Answer: " + "TO BE IMPLEMENTED");
        }

        static void PrintErrorInInput(string text)
        {
            Console.WriteLine("Day 9: Cannot process {0} in Input1.txt", text);
        }
    }
}
