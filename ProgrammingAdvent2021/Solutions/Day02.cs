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
        int position = 0, depth1 = 0, depth2 = 0, aim = 0;
        foreach (string line in input)
        {
            Match match = GeneratedRegex.ValidDay02InputLine.Match(line);
            int X = int.Parse(match.Groups["Magnitude"].Value);
            switch (line[0])
            {
                case 'f':
                    position += X;
                    depth2 += X * aim;
                    break;
                case 'd':
                    depth1 += X;
                    aim += X;
                    break;
                case 'u':
                    depth1 -= X;
                    aim -= X;
                    break;
                default:
                    break;
            }
        }
        return ($"{position * depth1}", $"{position * depth2}");
    }
}
