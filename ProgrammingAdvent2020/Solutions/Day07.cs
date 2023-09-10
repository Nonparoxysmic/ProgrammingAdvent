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
        int partOneAnswer = ContainsYourBag(rules, yourBag);
        int partTwoAnswer = YourBagContains(rules, yourBag);

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
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
                if (!colors.ContainsKey(m.Groups["color"].Value))
                {
                    rule.Add(-1, quantity);
                    continue;
                }
                int color = colors[m.Groups["color"].Value];
                rule.Add(color, quantity);
            }
            rules.Add(bagColor, rule);
        }
        return rules;
    }

    private static int ContainsYourBag(Dictionary<int, Rule> rules, int yourBag)
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
        return output.Count(r => r.Value);
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
        if (!rules.ContainsKey(bag))
        {
            return false;
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

    private int YourBagContains(Dictionary<int, Rule> rules, int yourBag)
    {
        if (!rules.ContainsKey(yourBag))
        {
            return 0;
        }
        int sum = 0;
        foreach ((int color, int quantity) in rules[yourBag].ColorQuantities)
        {
            sum += quantity * (1 + BagContains_Recursive(color, rules, yourBag));
        }
        return sum;
    }

    private int BagContains_Recursive(int bag, Dictionary<int, Rule> rules, int yourBag)
    {
        if (bag == yourBag || !rules.ContainsKey(bag))
        {
            return 0;
        }
        int sum = 0;
        foreach ((int color, int quantity) in rules[bag].ColorQuantities)
        {
            sum += quantity * (1 + BagContains_Recursive(color, rules, bag));
        }
        return sum;
    }

    private class Rule
    {
        public int Color { get; private set; }
        public Dictionary<int, int> ColorQuantities { get; private set; } = new();

        public Rule(int color)
        {
            Color = color;
        }

        public void Add(int color, int quantity)
        {
            ColorQuantities[color] = quantity;
        }

        public int[] ContentColors()
        {
            return ColorQuantities.Keys.ToArray();
        }
    }
}
