// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day07 : Day
    {
        private readonly Regex validLine = new Regex(@"^Step (\w) must be finished before step (\w) can begin\.$");

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
            SortedDictionary<char, int> stepPrerequisites = new SortedDictionary<char, int>();
            foreach (string line in inputLines)
            {
                Match validMatch = validLine.Match(line);
                if (!validMatch.Success)
                {
                    output.WriteError($"Invalid line \"{line}\" in input.", sw);
                    return output;
                }
                char prereq = validMatch.Groups[1].Value[0];
                char step = validMatch.Groups[2].Value[0];
                if (stepPrerequisites.ContainsKey(step))
                {
                    stepPrerequisites[step] |= CharToFlag(prereq);
                }
                else
                {
                    stepPrerequisites.Add(step, CharToFlag(prereq));
                }
                if (!stepPrerequisites.ContainsKey(prereq))
                {
                    stepPrerequisites.Add(prereq, 0);
                }
            }

            StringBuilder stepOrder = new StringBuilder();
            while (stepPrerequisites.Count > 0)
            {
                if (!NextStep(stepPrerequisites, out char nextStep))
                {
                    output.WriteError("No valid order of steps available.", sw);
                    return output;
                }
                stepOrder.Append(nextStep);
                stepPrerequisites.Remove(nextStep);
                foreach (char key in stepPrerequisites.Keys.ToList())
                {
                    stepPrerequisites[key] &= ~CharToFlag(nextStep);
                }
            }

            sw.Stop();
            output.WriteAnswers(stepOrder, null, sw);
            return output;
        }

        private int CharToFlag(char c)
        {
            if (c < 'A' || c > 'Z') { return 0; }
            return 1 << (c - '@');
        }

        private bool NextStep(SortedDictionary<char, int> stepPrerequisites, out char nextStep)
        {
            foreach (var kvp in stepPrerequisites)
            {
                if (kvp.Value == 0)
                {
                    nextStep = kvp.Key;
                    return true;
                }
            }
            nextStep = '\0';
            return false;
        }
    }
}
