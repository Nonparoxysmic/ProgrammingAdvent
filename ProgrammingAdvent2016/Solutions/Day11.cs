﻿// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day11 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 4 
                || inputLines[0].Length < 30 || inputLines[1].Length < 30 
                || inputLines[2].Length < 30 || inputLines[3].Length < 30
                || inputLines[0].Substring(0, 25) != "The first floor contains "
                || inputLines[1].Substring(0, 26) != "The second floor contains "
                || inputLines[2].Substring(0, 25) != "The third floor contains "
                || inputLines[3].Substring(0, 26) != "The fourth floor contains ")
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            stopwatch.Start();

            List<string> elements = new List<string>();
            for (int floor = 0; floor < 4; floor++)
            {
                inputLines[floor] = inputLines[floor].Substring(25 + floor % 2).TrimEnd('.');
                string[] terms = inputLines[floor].Split(',');
                foreach (string term in terms)
                {
                    string[] words = term.Split();
                    if (words.Length < 3) continue;
                    string element = "";
                    for (int i = 1; i < words.Length; i++)
                    {
                        if (words[i] == "generator")
                        {
                            element = words[i - 1];
                            break;
                        }
                        else if (words[i] == "microchip")
                        {
                            if (words[i - 1].Length > 11)
                            {
                                element = words[i - 1].Substring(0, words[i - 1].Length - 11);
                            }
                            break;
                        }
                    }
                    if (element != "")
                    {
                        if (!elements.Contains(element)) elements.Add(element);

                        // TODO: Add item to initial state:
                        //     Element number is "elements.IndexOf(element)"
                        //     Type is either generator or microchip.
                        //     Floor number is "floor"
                    }
                }
            }

            stopwatch.Reset();
            return solution;
        }
    }
}
