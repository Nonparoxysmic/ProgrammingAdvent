// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2015
{
    static class Day03
    {
        public static void Solve()
        {
            string input1Path = @"Day03\Puzzle\Input1.txt";
            string input1 = "";
            try
            {
                input1 = File.ReadAllText(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            int xPos = 0;
            int yPos = 0;
            List<string> houses = new List<string>
            {
                xPos.ToString() + yPos.ToString()
            };

            foreach (char c in input1)
            {
                switch (c)
                {
                    case '^':
                        yPos++;
                        break;
                    case 'v':
                        yPos--;
                        break;
                    case '>':
                        xPos++;
                        break;
                    case '<':
                        xPos--;
                        break;
                    default:
                        Print.PrintErrorAndExit("Unaccepted character in input file " + input1Path);
                        break;
                }
                string houseLabel = xPos.ToString() + yPos.ToString();
                if (!houses.Contains(houseLabel))
                {
                    houses.Add(houseLabel);
                }
            }

            Console.WriteLine("Day 3 Part One Answer: " + houses.Count);
        }
    }
}
