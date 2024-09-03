// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day12 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        HashSet<string> caveNames = [];
        List<(string, string)> connections = [];
        foreach (string line in input)
        {
            Match match = GeneratedRegex.ValidDay12InputLine.Match(line);
            if (!match.Success)
            {
                return ("Invalid line in input.", "n/a");
            }
            string cave0 = match.Groups["cave0"].Value;
            string cave1 = match.Groups["cave1"].Value;
            caveNames.Add(cave0);
            caveNames.Add(cave1);
            connections.Add((cave0, cave1));
        }
        if (!caveNames.Contains("start"))
        {
            return ("Error: no start cave.", "n/a");
        }
        if (!caveNames.Contains("end"))
        {
            return ("Error: no end cave.", "n/a");
        }
        Dictionary<string, Cave> caves = [];
        foreach (string name in caveNames)
        {
            caves.Add(name, new Cave(name));
        }
        foreach ((string cave0, string cave1) in connections)
        {
            if (caves[cave0].IsBigCave && caves[cave1].IsBigCave)
            {
                return ("Cave network contains an endless loop.", "n/a");
            }
            caves[cave0].Neighbors.Add(caves[cave1]);
            caves[cave1].Neighbors.Add(caves[cave0]);
        }

        // TODO: finish this.

        return ("Solution not yet implemented", "n/a");
    }

    private class Cave(string name)
    {
        public string Name { get; } = name;

        public HashSet<Cave> Neighbors { get; } = [];

        public bool IsBigCave { get; } = char.IsUpper(name[0]);
    }
}
