// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day06
    {
        public static void Solve()
        {
            string input1Path = @"Day06\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            LightGrid lightGrid = new LightGrid();

            foreach (string line in input1)
            {
                if (line.Length < 22)
                {
                    PrintErrorInInput(line);
                    continue;
                }
                string command = line.Substring(0, 7);
                switch (command)
                {
                    case "toggle ":
                        ProcessInstruction(-1, line.Substring(7), lightGrid);
                        break;
                    case "turn of":
                        ProcessInstruction(0, line.Substring(9), lightGrid);
                        break;
                    case "turn on":
                        ProcessInstruction(1, line.Substring(8), lightGrid);
                        break;
                    default:
                        PrintErrorInInput(line);
                        continue;
                }
            }

            Console.WriteLine("Day 6 Part One Answer: " + lightGrid.CountLightsOn());
        }

        static void ProcessInstruction(int command, string input, LightGrid lightGrid)
        {
            string[] coords = input.Split(' ');
            if (coords.Length != 3)
            {
                PrintErrorInInput(input);
                return;
            }
            string[] firstPair = coords[0].Split(',');
            string[] secondPair = coords[2].Split(',');
            if ((firstPair.Length != 2) || (secondPair.Length != 2))
            {
                PrintErrorInInput(input);
                return;
            }
            if ( !Int32.TryParse(firstPair[0], out int x1) || !Int32.TryParse(firstPair[1], out int y1)
                || !Int32.TryParse(secondPair[0], out int x2) || !Int32.TryParse(secondPair[1], out int y2) )
            {
                PrintErrorInInput(input);
                return;
            }

            if (command < 0)
            {
                lightGrid.ToggleRectangle(Math.Min(x1, x2), Math.Max(x1, x2), Math.Min(y1, y2), Math.Max(y1, y2));
            }
            else if (command > 0)
            {
                lightGrid.SetRectangle(Math.Min(x1, x2), Math.Max(x1, x2), Math.Min(y1, y2), Math.Max(y1, y2), true);
            }
            else
            {
                lightGrid.SetRectangle(Math.Min(x1, x2), Math.Max(x1, x2), Math.Min(y1, y2), Math.Max(y1, y2), false);
            }
        }

        static void PrintErrorInInput(string text)
        {
            Console.WriteLine("Day 6: Cannot process \"{0}\" in Input1.txt", text);
        }
    }
}
