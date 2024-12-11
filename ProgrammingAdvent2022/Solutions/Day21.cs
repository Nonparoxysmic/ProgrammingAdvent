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

        decimal rootMonkeyNumber = Monkey.AllMonkeys["root"].YelledNumber();

        Monkey.UpdateRootJob();

        decimal yourNumber = FindYourNumber();

        return result.WriteAnswers(rootMonkeyNumber, yourNumber);
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

    private static ulong FindYourNumber()
    {
        Monkey humn = Monkey.AllMonkeys["humn"];
        Monkey rootA = Monkey.AllMonkeys["root"].GetMonkeyA();
        Monkey rootB = Monkey.AllMonkeys["root"].GetMonkeyB();
        (decimal target, Monkey test) = Parameters(humn, rootA, rootB);
        humn.Number = 0;
        decimal prevNum = 0;
        decimal prevResult = test.YelledNumber();
        decimal result;
        humn.Number = 100;
        while (true)
        {
            result = test.YelledNumber();
            if (result == target)
            {
                return (ulong)humn.Number;
            }
            humn.Number += (target - result) / (result - prevResult) * (humn.Number - prevNum);
        }
    }

    private static (decimal, Monkey) Parameters(Monkey humn, Monkey rootA, Monkey rootB)
    {
        humn.Number = 151;
        decimal a1 = rootA.YelledNumber();
        decimal b1 = rootB.YelledNumber();
        humn.Number = 0;
        decimal a2 = rootA.YelledNumber();
        decimal b2 = rootB.YelledNumber();
        if (a1 == a2 && b1 != b2)
        {
            return (a1, rootB);
        }
        if (b1 == b2 && a1 != a2)
        {
            return (b1, rootA);
        }
        throw new NotImplementedException();
    }

    private class Monkey
    {
        public static Dictionary<string, Monkey> AllMonkeys = [];

        public string Name { get; }
        public decimal? Number { get; set; }
        public Func<string, string, decimal> Job { get; private set; }
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
                    _ => throw new InvalidOperationException(),
                };
            }
            AllMonkeys.Add(Name, this);
        }

        public static void UpdateRootJob()
        {
            AllMonkeys["root"].Job = (a, b) =>
            {
                return AllMonkeys[a].YelledNumber() == AllMonkeys[b].YelledNumber() ? 1 : 0;
            };
        }

        public decimal YelledNumber()
        {
            if (Number is not null)
            {
                return (decimal)Number;
            }
            if (MonkeyA is not null && MonkeyB is not null)
            {
                return Job(MonkeyA, MonkeyB);
            }
            throw new InvalidOperationException();
        }

        public Monkey GetMonkeyA()
        {
            if (MonkeyA is not null)
            {
                return AllMonkeys[MonkeyA];
            }
            throw new InvalidOperationException();
        }

        public Monkey GetMonkeyB()
        {
            if (MonkeyB is not null)
            {
                return AllMonkeys[MonkeyB];
            }
            throw new InvalidOperationException();
        }
    }
}
