// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day20 : Day
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
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            var ranges = new SortedDictionary<uint, uint>();
            var validLine = new Regex(@"^\d+-\d+$");
            foreach (string line in inputLines)
            {
                if (validLine.IsMatch(line))
                {
                    int dashPos = line.IndexOf('-');
                    var lower = uint.Parse(line.Substring(0, dashPos));
                    var higher = uint.Parse(line.Substring(dashPos + 1));
                    if (lower > higher)
                    {
                        solution.WriteSolution(1, "ERROR: Invalid input: \"" + line + "\"", 0);
                        return solution;
                    }
                    if (ranges.ContainsKey(lower))
                    {
                        ranges[lower] = Math.Max(ranges[lower], higher);
                    }
                    else { ranges.Add(lower, higher); }
                }
                else
                {
                    solution.WriteSolution(1, "ERROR: Invalid input: \"" + line + "\"", 0);
                    return solution;
                }
            }

            string partOneSolution = "";
            uint partTwoSolution = 0;
            for (uint i = 0; i < uint.MaxValue; )
            {
                bool isBlocked = false;
                foreach (KeyValuePair<uint, uint> kvp in ranges)
                {
                    if (kvp.Key <= i && i <= kvp.Value)
                    {
                        isBlocked = true;
                        if (kvp.Value == uint.MaxValue)
                        {
                            i = uint.MaxValue;
                            if (partOneSolution == "")
                            {
                                partOneSolution = "No unblocked IPs.";
                                solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);
                            }
                        }
                        else
                        {
                            i = kvp.Value + 1;
                        }
                        break;
                    }
                }
                if (!isBlocked)
                {
                    if (partOneSolution == "")
                    {
                        partOneSolution = i.ToString();
                        solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);
                    }
                    partTwoSolution++;
                    i++;
                }
            }
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
