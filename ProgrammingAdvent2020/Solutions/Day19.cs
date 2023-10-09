// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day19 : Day
{
    private static readonly string SUBRULES = @"([0-9]{1,3}( [0-9]{1,3})*( \|( [0-9]{1,3})+)?)";

    private static readonly Regex _validRule = new($"^(?<rule>[0-9]{{1,3}}): (\"[ab]\"|{SUBRULES})$");
    private static readonly Regex _receivedMessage = new("^[ab]{1,99}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        int i = 0;
        List<int> rulesFound = new();
        for (; i < input.Length - 1; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
            Match ruleMatch = _validRule.Match(input[i]);
            if (!ruleMatch.Success)
            {
                errorMessage = $"Input line \"{input[i].Left(20, true)}\" is not a valid rule.";
                return false;
            }
            rulesFound.Add(int.Parse(ruleMatch.Groups["rule"].Value));
        }
        rulesFound.Sort();
        for (int j = 0; j < rulesFound.Count; j++)
        {
            if (rulesFound[j] != j)
            {
                errorMessage = "Input contains misnumbered rules.";
                return false;
            }
        }
        i++;
        for (; i < input.Length; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
            if (!_receivedMessage.IsMatch(input[i]))
            {
                errorMessage = $"Input line \"{input[i].Left(20, true)}\" is not a valid message.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        Dictionary<int, Rule> rules = new();
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                break;
            }
            int colon = line.IndexOf(':');
            int ruleNumber = int.Parse(line[..colon]);
            if (line[^2] == 'a' || line[^2] == 'b')
            {
                rules.Add(ruleNumber, new Rule(ruleNumber, line[^2]));
                continue;
            }
            int[][] subrules;
            int bar = line.IndexOf('|');
            if (bar < 0)
            {
                subrules = new int[1][];
                subrules[0] = line[(colon + 2)..].Split(' ').Select(s => int.Parse(s)).ToArray();
            }
            else
            {
                subrules = new int[2][];
                subrules[0] = line[(colon + 2)..(bar - 1)].Split(' ').Select(s => int.Parse(s)).ToArray();
                subrules[1] = line[(bar + 2)..].Split(' ').Select(s => int.Parse(s)).ToArray();
            }
            rules.Add(ruleNumber, new Rule(ruleNumber, subrules));
        }
        string[] messages = GetMessages(input);

        if (!rules.ContainsKey(42) || !rules.ContainsKey(31))
        {
            Regex exampleRegex = new($"^{ResolvePattern(0, rules)}$");
            int examplePartOne = messages.Count(m => exampleRegex.IsMatch(m));
            return output.WriteAnswers(examplePartOne, null);
        }

        string rule42 = ResolvePattern(42, rules);
        string rule31 = ResolvePattern(31, rules);
        Regex partOneRegex = new($"^{rule42}{rule42}{rule31}$");
        int partOneAnswer = messages.Count(m => partOneRegex.IsMatch(m));

        Regex regex42 = new(rule42);
        Regex regex31 = new(rule31);
        int partTwoAnswer = 0;
        foreach (string message in messages)
        {
            if (PartTwoMessageIsValid(message, regex42, regex31))
            {
                partTwoAnswer++;
            }
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static string[] GetMessages(string[] input)
    {
        int i = 0;
        for (; i < input.Length - 1; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
        }
        i++;
        int start = i;
        for (; i < input.Length; i++)
        {
            if (input[i].Length == 0)
            {
                break;
            }
        }
        int end = i;
        string[] messages = new string[end - start];
        Array.Copy(input, start, messages, 0, end - start);
        return messages;
    }

    private static string ResolvePattern(int ruleNumber, Dictionary<int, Rule> rules)
    {
        if (!rules.ContainsKey(ruleNumber))
        {
            return string.Empty;
        }
        Rule rule = rules[ruleNumber];
        if (rule.Pattern is not null)
        {
            return rule.Pattern;
        }
        if (rule.Subrules is null)
        {
            return string.Empty;
        }
        string subrules0 = ResolveSubrules(rule.Subrules[0], rules);
        if (rule.Subrules.Length == 2)
        {
            string subrules1 = ResolveSubrules(rule.Subrules[1], rules);
            rule.Pattern = $"({subrules0}|{subrules1})";
        }
        else
        {
            rule.Pattern = subrules0;
        }
        return rule.Pattern;
    }

    private static string ResolveSubrules(int[] ruleNumbers, Dictionary<int, Rule> rules)
    {
        IEnumerable<string> subpatterns = ruleNumbers.Select(n => ResolvePattern(n, rules));
        return string.Join(null, subpatterns);
    }

    private static bool PartTwoMessageIsValid(string message, Regex regex42, Regex regex31)
    {
        if (message.Length % 8 != 0)
        {
            return false;
        }
        int fortyTwos = 0, thirtyOnes = 0;
        int i = 0;
        for (; i < message.Length; i += 8)
        {
            if (regex42.IsMatch(message[i..(i + 8)]))
            {
                fortyTwos++;
            }
            else
            {
                break;
            }
        }
        for (; i < message.Length; i += 8)
        {
            if (regex31.IsMatch(message[i..(i + 8)]))
            {
                thirtyOnes++;
            }
            else
            {
                return false;
            }
        }
        return fortyTwos > thirtyOnes && thirtyOnes > 0;
    }

    private class Rule
    {
        public int Number { get; private set; }
        public string? Pattern { get; set; }
        public int[][]? Subrules { get; private set; }

        public Rule(int ruleNumber, char character)
        {
            Number = ruleNumber;
            Pattern = character.ToString();
            Subrules = null;
        }

        public Rule(int ruleNumber, int[][] subrules)
        {
            Number = ruleNumber;
            Pattern = null;
            Subrules = subrules;
        }
    }
}
