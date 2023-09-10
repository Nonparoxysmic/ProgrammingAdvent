// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day07 : Day
{
    private static readonly string ADJECTIVE = "(bright|clear|dark|dim|dotted|drab|dull|"
        + "faded|light|mirrored|muted|pale|plaid|posh|shiny|striped|vibrant|wavy)";
    private static readonly string COLOR = "(aqua|beige|black|blue|bronze|brown|chartreuse|"
        + "coral|crimson|cyan|fuchsia|gold|gray|green|indigo|lavender|lime|magenta|maroon|olive|"
        + "orange|plum|purple|red|salmon|silver|tan|teal|tomato|turquoise|violet|white|yellow)";
    private static readonly string ADJECTIVE_COLOR = $"({ADJECTIVE} {COLOR})";
    private static readonly string LIST_ITEM = $"([1-9] {ADJECTIVE_COLOR} bags?, )";
    private static readonly string LIST_END = $"([1-9] {ADJECTIVE_COLOR} bags?\\.)";
    private static readonly string LIST = $"({LIST_ITEM}{{0,3}}{LIST_END})";

    private static readonly Regex _validLine = new($"^{ADJECTIVE_COLOR} bags contain (no other bags\\.|{LIST})$");
    private static readonly Regex _lineBag = new($"^{ADJECTIVE_COLOR}(?= bags contain )");
    private static readonly Regex _bagContents = new($"(?<quantity>[1-9]) (?<color>{ADJECTIVE_COLOR})(?= bag)");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        Dictionary<string, int> colors = GetColors(input);
        Dictionary<int, Rule> rules = GetRules(input, colors);
        int yourBag = colors["shiny gold"];
        Dictionary<int, bool> containsYourBag = ContainsYourBag(rules, yourBag);
        int partOneAnswer = containsYourBag.Count(r => r.Value);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static Dictionary<string, int> GetColors(string[] input)
    {
        Dictionary<string, int> colors = new();
        for (int i = 0; i < input.Length; i++)
        {
            colors.Add(_lineBag.Match(input[i]).Value, i);
        }
        return colors;
    }

    private static Dictionary<int, Rule> GetRules(string[] input, Dictionary<string, int> colors)
    {
        Dictionary<int, Rule> rules = new();
        foreach (string line in input)
        {
            int bagColor = colors[_lineBag.Match(line).Value];
            Rule rule = new(bagColor);
            MatchCollection matchCollection = _bagContents.Matches(line);
            foreach (Match m in matchCollection.Cast<Match>())
            {
                int quantity = int.Parse(m.Groups["quantity"].Value);
                int color = colors[m.Groups["color"].Value];
                rule.Add(color, quantity);
            }
            rules.Add(bagColor, rule);
        }
        return rules;
    }

    private static Dictionary<int, bool> ContainsYourBag(Dictionary<int, Rule> rules, int yourBag)
    {
        Dictionary<int, bool> output = new()
        {
            { yourBag, false }
        };
        for (int i = 0; i < rules.Count; i++)
        {
            if (output.ContainsKey(i))
            {
                continue;
            }
            output[i] = ContainsYourBag(i, rules, yourBag, output);
        }
        return output;
    }

    private static bool ContainsYourBag
        (
            int bag,
            Dictionary<int, Rule> rules,
            int yourBag,
            Dictionary<int, bool> output
        )
    {
        if (bag == yourBag)
        {
            return false;
        }
        if (output.ContainsKey(bag))
        {
            return output[bag];
        }
        int[] contents = rules[bag].ContentColors();
        foreach (int containedBag in contents)
        {
            if (containedBag == yourBag)
            {
                output[bag] = true;
                return true;
            }
            if (ContainsYourBag(containedBag, rules, yourBag, output))
            {
                output[bag] = true;
                return true;
            }
        }
        output[bag] = false;
        return false;
    }

    private class Rule
    {
        public int Color { get; private set; }

        private readonly Dictionary<int, int> _colorQuantities = new();

        public Rule(int color)
        {
            Color = color;
        }

        public void Add(int color, int quantity)
        {
            _colorQuantities[color] = quantity;
        }

        public int[] ContentColors()
        {
            return _colorQuantities.Keys.ToArray();
        }
    }
}
