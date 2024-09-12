// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day14 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        string template = GeneratedRegex.ValidDay14Template.Match(input[0]).Value;
        Counter<string> pairQuantities = new();
        for (int i = 0; i < template.Length - 1; i++)
        {
            pairQuantities.Add(template[i..(i + 2)], 1);
        }
        Dictionary<string, (string, string)> rules = [];
        foreach (string line in input)
        {
            Match ruleMatch = GeneratedRegex.ValidDay14Rule.Match(line);
            if (ruleMatch.Success)
            {
                string pair = ruleMatch.Groups["pair"].Value;
                char insertion = ruleMatch.Groups["insertion"].Value[0];
                string output1 = string.Join(separator: null, pair[0], insertion);
                string output2 = string.Join(separator: null, insertion, pair[1]);
                rules.Add(pair, (output1, output2));
            }
        }

        for (int i = 0; i < 10; i++)
        {
            Update(pairQuantities, rules);
        }
        Counter<char> elementQuantities = Count(pairQuantities, template[0], template[^1]);
        long partOneAnswer = elementQuantities.Values.Max() - elementQuantities.Values.Min();

        for (int i = 10; i < 40; i++)
        {
            Update(pairQuantities, rules);
        }
        elementQuantities = Count(pairQuantities, template[0], template[^1]);
        long partTwoAnswer = elementQuantities.Values.Max() - elementQuantities.Values.Min();

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static void Update(Counter<string> pairQuantities,
        Dictionary<string, (string, string)> rules)
    {
        Counter<string> updates = new();
        foreach (string pair in pairQuantities.Keys)
        {
            if (rules.TryGetValue(pair, out (string, string) rule))
            {
                updates.Add(pair, -pairQuantities[pair]);
                updates.Add(rule.Item1, pairQuantities[pair]);
                updates.Add(rule.Item2, pairQuantities[pair]);
            }
        }
        foreach (string pair in updates.Keys)
        {
            pairQuantities.Add(pair, updates[pair]);
        }
    }

    private static Counter<char> Count(Counter<string> pairQuantities, char first, char last)
    {
        Counter<char> elementQuantities = new();
        foreach (string pair in pairQuantities.Keys)
        {
            elementQuantities.Add(pair[0], pairQuantities[pair]);
            elementQuantities.Add(pair[1], pairQuantities[pair]);
        }
        elementQuantities.Add(first, 1);
        elementQuantities.Add(last, 1);
        foreach (char element in elementQuantities.Keys)
        {
            elementQuantities[element] = elementQuantities[element] / 2;
        }
        return elementQuantities;
    }

    private class Counter<T> where T : notnull
    {
        private readonly Dictionary<T, long> _counts = [];

        public Dictionary<T, long>.KeyCollection Keys { get => _counts.Keys; }
        public Dictionary<T, long>.ValueCollection Values { get => _counts.Values; }

        public void Add(T item, long quantity)
        {
            if (!_counts.TryAdd(item, quantity))
            {
                _counts[item] += quantity;
            }
        }

        public long this[T key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        private long Get(T key)
        {
            if (_counts.TryGetValue(key, out long result))
            {
                return result;
            }
            return 0;
        }

        private void Set(T key, long value)
        {
            if (!_counts.TryAdd(key, value))
            {
                _counts[key] = value;
            }
        }
    }
}
