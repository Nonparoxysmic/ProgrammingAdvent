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
                HouseLabel(xPos, yPos)
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
                string houseLabel = HouseLabel(xPos, yPos);
                if (!houses.Contains(houseLabel))
                {
                    houses.Add(houseLabel);
                }
            }

            Console.WriteLine("Day 3 Part One Answer: " + houses.Count);


            int santaXPos = 0;
            int santaYPos = 0;
            int robotXPos = 0;
            int robotYPos = 0;
            houses = new List<string>
            {
                HouseLabel(santaXPos, santaYPos)
            };

            bool santaTurn = true;
            foreach (char c in input1)
            {
                if (santaTurn)
                {
                    switch (c)
                    {
                        case '^':
                            santaYPos++;
                            break;
                        case 'v':
                            santaYPos--;
                            break;
                        case '>':
                            santaXPos++;
                            break;
                        case '<':
                            santaXPos--;
                            break;
                        default:
                            Print.PrintErrorAndExit("Unaccepted character in input file " + input1Path);
                            break;
                    }
                    string houseLabel = HouseLabel(santaXPos, santaYPos);
                    if (!houses.Contains(houseLabel))
                    {
                        houses.Add(houseLabel);
                    }
                    santaTurn = false;
                }
                else
                {
                    switch (c)
                    {
                        case '^':
                            robotYPos++;
                            break;
                        case 'v':
                            robotYPos--;
                            break;
                        case '>':
                            robotXPos++;
                            break;
                        case '<':
                            robotXPos--;
                            break;
                        default:
                            Print.PrintErrorAndExit("Unaccepted character in input file " + input1Path);
                            break;
                    }
                    string houseLabel = HouseLabel(robotXPos, robotYPos);
                    if (!houses.Contains(houseLabel))
                    {
                        houses.Add(houseLabel);
                    }
                    santaTurn = true;
                }
            }

            Console.WriteLine("Day 3 Part Two Answer: " + houses.Count);
        }

        static string HouseLabel(int x, int y)
        {
            return x + "," + y;
        }
    }
}
