// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day21 : Day
    {
        private Dictionary<string, string> rules;

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
            rules = new Dictionary<string, string>();
            foreach (string line in inputLines)
            {
                if (!Regex.IsMatch(line, @"^[.#][.#]/[.#][.#] => "
                    + @"[.#][.#][.#]/[.#][.#][.#]/[.#][.#][.#]$")
                    && !Regex.IsMatch(line, @"^[.#][.#][.#]/[.#][.#][.#]/[.#][.#][.#] => "
                    + @"[.#][.#][.#][.#]/[.#][.#][.#][.#]/[.#][.#][.#][.#]/[.#][.#][.#][.#]$"))
                {
                    output.WriteError($"Invalid line in input: \"{line}\"", sw);
                    return output;
                }
                if (line.Length == 20)
                {
                    rules.Add(line[0..5], line[9..]);
                }
                else
                {
                    rules.Add(line[0..11], line[15..]);
                }
            }

            Dictionary<string, string> missingRules = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in rules)
            {
                string[] transformations = GetTransformations(kvp.Key);
                foreach (string transformation in transformations)
                {
                    missingRules[transformation] = kvp.Value;
                }
            }
            foreach (KeyValuePair<string, string> kvp in missingRules)
            {
                rules[kvp.Key] = kvp.Value;
            }


            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private string[] GetTransformations(string pattern)
        {
            List<string> transformations = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                pattern = FlipPatternHorizontally(pattern);
                if (!transformations.Contains(pattern))
                {
                    transformations.Add(pattern);
                }
                pattern = FlipPatternDiagonally(pattern);
                if (!transformations.Contains(pattern) && i < 3)
                {
                    transformations.Add(pattern);
                }
                if (i == 3)
                {
                    // Remove the original pattern.
                    transformations.Remove(pattern);
                }
            }
            return transformations.ToArray();
        }

        private string FlipPatternHorizontally(string original)
        {
            StringBuilder flipped = new StringBuilder();
            if (original.Length == 5)
            {
                flipped.Append(original[1]);
                flipped.Append(original[0]);
                flipped.Append('/');
                flipped.Append(original[4]);
                flipped.Append(original[3]);
            }
            else if (original.Length == 11)
            {
                flipped.Append(original[2]);
                flipped.Append(original[1]);
                flipped.Append(original[0]);
                flipped.Append('/');
                flipped.Append(original[6]);
                flipped.Append(original[5]);
                flipped.Append(original[4]);
                flipped.Append('/');
                flipped.Append(original[10]);
                flipped.Append(original[9]);
                flipped.Append(original[8]);
            }
            return flipped.ToString();
        }

        private string FlipPatternDiagonally(string original)
        {
            StringBuilder flipped = new StringBuilder();
            if (original.Length == 5)
            {
                flipped.Append(original[4]);
                flipped.Append(original[1]);
                flipped.Append('/');
                flipped.Append(original[3]);
                flipped.Append(original[0]);
            }
            else if (original.Length == 11)
            {
                flipped.Append(original[10]);
                flipped.Append(original[6]);
                flipped.Append(original[2]);
                flipped.Append('/');
                flipped.Append(original[9]);
                flipped.Append(original[5]);
                flipped.Append(original[1]);
                flipped.Append('/');
                flipped.Append(original[8]);
                flipped.Append(original[4]);
                flipped.Append(original[0]);
            }
            return flipped.ToString();
        }
    }
}
