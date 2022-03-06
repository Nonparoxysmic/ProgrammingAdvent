// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day13 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            Dictionary<int, int> depthsAndRanges = new Dictionary<int, int>();
            foreach (string line in inputLines)
            {
                if (!Regex.IsMatch(line, @"^\d+: \d+$"))
                {
                    output.WriteError($"Invalid line in input: \"{line}\".", sw);
                    return output;
                }
                int depth = int.Parse(Regex.Match(line, @"^\d+(?=: )").Value);
                int range = int.Parse(Regex.Match(line, @"(?<=: )\d+$").Value);
                depthsAndRanges.Add(depth, range);
            }

            int severity = 0;
            foreach (KeyValuePair<int, int> kvp in depthsAndRanges)
            {
                if (CaughtOnLayer(kvp.Key, kvp.Value, 0))
                {
                    severity += kvp.Key * kvp.Value;
                }
            }

            int delay;
            for (delay = 0; delay < int.MaxValue; delay++)
            {
                bool caught = false;
                foreach (KeyValuePair<int, int> kvp in depthsAndRanges)
                {
                    if (CaughtOnLayer(kvp.Key, kvp.Value, delay))
                    {
                        caught = true;
                        break;
                    }
                }
                if (!caught) { break; }
            }

            sw.Stop();
            output.WriteAnswers(severity, delay, sw);
            return output;
        }

        private bool CaughtOnLayer(int depth, int range, int startTime)
        {
            int cycleLength = 2 * (range - 1);
            int arrivalTime = depth + startTime;
            if (arrivalTime % cycleLength == 0) { return true; }
            return false;
        }
    }
}
