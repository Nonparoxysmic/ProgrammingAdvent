// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day21 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^[a-z]{4}: ([0-9]{1,4}|[a-z]{4} [+*/-] [a-z]{4})$")]
    private static partial Regex ValidInputLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        ParseInput(input);

        long rootMonkeyNumber = Monkey.AllMonkeys["root"].YelledNumber();

        return result.WriteAnswers(rootMonkeyNumber, null);
    }

    private static void ParseInput(string[] input)
    {
        Monkey.AllMonkeys.Clear();
        foreach (string line in input)
        {
            Match match = ValidInputLine.Match(line);
            if (match.Success)
            {
                _ = new Monkey(line);
            }
        }
    }

    private class Monkey
    {
        public static Dictionary<string, Monkey> AllMonkeys = [];

        public string Name { get; }
        public long? Number { get; }
        public Func<string, string, long> Job { get; }
        public string? MonkeyA { get; }
        public string? MonkeyB { get; }

        public Monkey(string data)
        {
            Name = data[0..4];
            if (int.TryParse(data[6..], out int number))
            {
                Number = number;
                Job = (a, b) => number;
            }
            else
            {
                MonkeyA = data[6..10];
                MonkeyB = data[13..17];
                Job = data[11] switch
                {
                    '+' => (a, b) => AllMonkeys[a].YelledNumber() + AllMonkeys[b].YelledNumber(),
                    '-' => (a, b) => AllMonkeys[a].YelledNumber() - AllMonkeys[b].YelledNumber(),
                    '*' => (a, b) => AllMonkeys[a].YelledNumber() * AllMonkeys[b].YelledNumber(),
                    '/' => (a, b) => AllMonkeys[a].YelledNumber() / AllMonkeys[b].YelledNumber(),
                    _ => (a, b) => -1
                };
            }
            AllMonkeys.Add(Name, this);
        }

        public long YelledNumber()
        {
            if (Number is not null)
            {
                return (long)Number;
            }
            if (MonkeyA is not null && MonkeyB is not null)
            {
                return Job(MonkeyA, MonkeyB);
            }
            return -1;
        }
    }
}
