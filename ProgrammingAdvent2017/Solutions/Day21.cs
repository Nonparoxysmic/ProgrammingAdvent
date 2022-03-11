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

            string startingPattern = ".#./..#/###";

            if (!SearchPixelsOn(startingPattern, 5, out int partOneAnswer))
            {
                if (partOneAnswer == -404)
                {
                    output.WriteError("Missing rules in input.", sw);
                    return output;
                }
                else
                {
                    output.WriteError("Unknown error.", sw);
                    return output;
                }
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
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

        private int CountPixelsOn(string pattern)
        {
            int sum = 0;
            foreach (char c in pattern)
            {
                if (c == '#') { sum++; }
            }
            return sum;
        }

        private bool SearchPixelsOn(string sizeThreePattern, int depth, out int result)
        {
            if (depth < 0)
            {
                result = -400;
                return false;
            }
            else if (depth == 0)
            {
                result = CountPixelsOn(sizeThreePattern);
                return true;
            }
            else if (depth == 1)
            {
                if (TryOneIteration(sizeThreePattern, out string newPattern))
                {
                    result = CountPixelsOn(newPattern);
                    return true;
                }
                else
                {
                    result = -404;
                    return false;
                }
            }
            else if (depth == 2)
            {
                if (TryTwoIterations(sizeThreePattern, out string newPattern))
                {
                    result = CountPixelsOn(newPattern);
                    return true;
                }
                else
                {
                    result = -404;
                    return false;
                }
            }
            else
            {
                if (TryThreeIterations(sizeThreePattern, out string[] newPatterns))
                {
                    int sum = 0;
                    foreach (string pattern in newPatterns)
                    {
                        if (!SearchPixelsOn(pattern, depth - 3, out int pixels))
                        {
                            result = pixels;
                            return false;
                        }
                        sum += pixels;
                    }
                    result = sum;
                    return true;
                }
                else
                {
                    result = -404;
                    return false;
                }
            }
        }

        private bool TryOneIteration(string sizeThreePattern, out string newPattern)
        {
            try
            {
                newPattern = rules[sizeThreePattern];
                return true;
            }
            catch
            {
                newPattern = null;
                return false;
            }
        }

        private bool TryTwoIterations(string sizeThreePattern, out string newPattern)
        {
            try
            {
                string oneIteration = rules[sizeThreePattern];
                int[] offsets = new int[] { 0, 2, 10, 12 };
                StringBuilder twoIterations = new StringBuilder();
                foreach (int offset in offsets)
                {
                    StringBuilder sizeTwoPattern = new StringBuilder();
                    sizeTwoPattern.Append(oneIteration[offset]);
                    sizeTwoPattern.Append(oneIteration[offset + 1]);
                    sizeTwoPattern.Append('/');
                    sizeTwoPattern.Append(oneIteration[offset + 5]);
                    sizeTwoPattern.Append(oneIteration[offset + 6]);
                    twoIterations.Append(rules[sizeTwoPattern.ToString()]);
                }
                newPattern = twoIterations.ToString();
                return true;
            }
            catch
            {
                newPattern = null;
                return false;
            }
        }

        private bool TryThreeIterations(string sizeThreePattern, out string[] newPatterns)
        {
            if (!TryTwoIterations(sizeThreePattern, out string sizeSixPattern))
            {
                newPatterns = null;
                return false;
            }
            try
            {
                int[][] offsets = new int[][]
                {
                    new int[] { 0, 1, 4, 5 },
                    new int[] { 2, 11, 6, 15 },
                    new int[] { 12, 13, 16, 17 },
                    new int[] { 8, 9, 22, 23 },
                    new int[] { 10, 19, 24, 33 },
                    new int[] { 20, 21, 34, 35 },
                    new int[] { 26, 27, 30, 31 },
                    new int[] { 28, 37, 32, 41 },
                    new int[] { 38, 39, 42, 43 }
                };
                List<string> output = new List<string>();
                for (int i = 0; i < 9; i++)
                {
                    StringBuilder sizeTwoPattern = new StringBuilder();
                    sizeTwoPattern.Append(sizeSixPattern[offsets[i][0]]);
                    sizeTwoPattern.Append(sizeSixPattern[offsets[i][1]]);
                    sizeTwoPattern.Append('/');
                    sizeTwoPattern.Append(sizeSixPattern[offsets[i][2]]);
                    sizeTwoPattern.Append(sizeSixPattern[offsets[i][3]]);
                    output.Add(rules[sizeTwoPattern.ToString()]);
                }
                newPatterns = output.ToArray();
                return true;
            }
            catch
            {
                newPatterns = null;
                return false;
            }
        }
    }
}
