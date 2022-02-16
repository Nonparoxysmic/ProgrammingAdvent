// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day13 : Day
    {
        readonly Dictionary<int, int> depthsAndRanges = new Dictionary<int, int>();

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                if (!Regex.IsMatch(line, @"^\d+: \d+$"))
                {
                    output.WriteError($"Invalid input \"{line}\".", sw);
                    return output;
                }
                int depth = int.Parse(Regex.Match(line, @"^\d+(?=: )").Value);
                int range = int.Parse(Regex.Match(line, @"(?<=: )\d+$").Value);
                depthsAndRanges.Add(depth, range);
            }

            int severity = 0;
            foreach (KeyValuePair<int, int> kvp in depthsAndRanges)
            {
                if (CaughtOnLayer(kvp.Key, 0))
                {
                    severity += kvp.Key * kvp.Value;
                }
            }

            sw.Stop();
            output.WriteAnswers(severity, null, sw);
            return output;
        }

        private bool CaughtOnLayer(int depth, int startTime)
        {
            int cycleLength = 2 * (depthsAndRanges[depth] - 1);
            int arrivalTime = depth + startTime;
            if (arrivalTime % cycleLength == 0) { return true; }
            return false;
        }
    }
}
