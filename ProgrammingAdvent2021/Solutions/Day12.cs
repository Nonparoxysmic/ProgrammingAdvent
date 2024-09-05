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
        int partOneAnswer = 0;
        bool canDoubleVisit = false;
        FindPathsToEnd(caves["start"], ref partOneAnswer, ref canDoubleVisit);
        int partTwoAnswer = 0;
        canDoubleVisit = true;
        FindPathsToEnd(caves["start"], ref partTwoAnswer, ref canDoubleVisit);
        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static void FindPathsToEnd(Cave cave, ref int count, ref bool canDoubleVisit)
    {
        if (cave.Name == "end")
        {
            count++;
            return;
        }
        if (cave.IsSmallCave)
        {
            cave.Visits++;
        }
        foreach (Cave neighbor in cave.Neighbors)
        {
            if (neighbor.Name == "start")
            {
                continue;
            }
            if (neighbor.Visits == 0)
            {
                FindPathsToEnd(neighbor, ref count, ref canDoubleVisit);
            }
            else if (canDoubleVisit)
            {
                canDoubleVisit = false;
                FindPathsToEnd(neighbor, ref count, ref canDoubleVisit);
                canDoubleVisit = true;
            }
        }
        if (cave.IsSmallCave)
        {
            cave.Visits--;
        }
    }

    private class Cave(string name)
    {
        public string Name { get; } = name;

        public HashSet<Cave> Neighbors { get; } = [];

        public bool IsBigCave { get; } = char.IsUpper(name[0]);

        public bool IsSmallCave { get => !IsBigCave; }

        public int Visits { get; set; }
    }
}
