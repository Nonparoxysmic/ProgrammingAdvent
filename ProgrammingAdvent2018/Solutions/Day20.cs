﻿// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day20 : Day
    {
        private readonly Regex backtracking = new Regex(@"(NS|SN|EW|WE)");
        private readonly Regex innerRouteOptions = new Regex(@"\([NSEW|]*\)");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input.Length < 3)
            {
                output.WriteError("Input is too short.", sw);
                return output;
            }
            if (input[0] != '^' || input[^1] != '$')
            {
                output.WriteError("Input must begin with '^' and end with '$'.", sw);
                return output;
            }
            for (int i = 1; i < input.Length - 1; i++)
            {
                switch (input[i])
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                    case '|':
                    case '(':
                    case ')':
                        continue;
                    default:
                        output.WriteError($"Input contains invalid character '{input[i]}'.", sw);
                        return output;
                }
            }
            Regex inputRegex;
            try
            {
                inputRegex = new Regex(input);
            }
            catch
            {
                output.WriteError("Input is not a valid regular expression.", sw);
                return output;
            }

            int partOneAnswer = LongestShortestPath(input);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private string LongestBranch(Match match)
        {
            string[] options = match.Value[1..^1].Split('|');
            string longest = string.Empty;
            int longestLength = 0;
            for (int i = 0; i < options.Length; i++)
            {
                // Cancel out backtracking.
                string next = options[i];
                while (true)
                {
                    next = backtracking.Replace(options[i], "");
                    if (options[i] == next)
                    {
                        break;
                    }
                    options[i] = next;
                }

                if (options[i].Length > longestLength)
                {
                    longestLength = options[i].Length;
                    longest = options[i];
                }
            }
            return longest;
        }

        private int LongestShortestPath(string input)
        {
            MatchEvaluator longestBranchEvaluator = new MatchEvaluator(LongestBranch);
            string previousInput = input;
            while (true)
            {
                input = innerRouteOptions.Replace(previousInput, longestBranchEvaluator);
                if (input == previousInput)
                {
                    break;
                }
                previousInput = input;
            }
            return input.Length - 2;
        }
    }
}
