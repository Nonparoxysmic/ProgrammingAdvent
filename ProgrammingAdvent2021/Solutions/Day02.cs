// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day02 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int position = 0, depth = 0;
        foreach (string line in input)
        {
            Match match = GeneratedRegex.ValidDay02InputLine.Match(line);
            int distance = int.Parse(match.Groups["Magnitude"].Value);
            switch (line[0])
            {
                case 'f':
                    position += distance;
                    break;
                case 'd':
                    depth += distance;
                    break;
                case 'u':
                    depth -= distance;
                    break;
                default:
                    break;
            }
        }
        return ($"{position * depth}", "n/a");
    }
}
