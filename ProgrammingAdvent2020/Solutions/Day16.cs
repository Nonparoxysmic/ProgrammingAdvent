// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Diagnostics.CodeAnalysis;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day16 : Day
{
    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (input.Length < 7)
        {
            errorMessage = "Input is too short.";
            return false;
        }
        InputParser inputParser = new(input);
        if (!inputParser.Success)
        {
            errorMessage = inputParser.Error;
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        InputParser inputParser = new(input);
        if (!inputParser.Success)
        {
            return output.WriteError(inputParser.Error);
        }
        int partOneAnswer = TicketScanningErrorRate(inputParser.Rules, inputParser.NearbyTickets);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static int TicketScanningErrorRate(Rule[] rules, Ticket[] nearbyTickets)
    {
        int sum = 0;
        foreach (Ticket ticket in nearbyTickets)
        {
            foreach (int value in ticket.FieldValues)
            {
                bool valueIsValid = rules.Aggregate(false, (acc, rule) => acc |= rule.ValueIsValid(value));
                if (!valueIsValid)
                {
                    sum += value;
                }
            }
        }
        return sum;
    }

    private class InputParser
    {
        private static readonly string[] _split = new string[] { " ", "or" };

        public bool Success { get; private set; }
        public string Error { get; private set; }

        public Rule[] Rules { get; private set; }
        public Ticket YourTicket { get; private set; }
        public Ticket[] NearbyTickets { get; private set; }

        public InputParser(string[] input)
        {
            List<Rule> rules = new();
            List<Ticket> nearbyTickets = new();
            // Read rules.
            int i = 0;
            for (; i < input.Length - 5; i++)
            {
                if (input[i].Length == 0)
                {
                    break;
                }
                if (!TryParseRule(input[i], out Rule? rule))
                {
                    ParsingError($"Invalid rule \"{input[i].Left(40, true)}\".");
                    return;
                }
                rules.Add(rule);
            }
            if (rules.Count == 0)
            {
                ParsingError("No rules were found.");
                return;
            }
            // Read your ticket.
            i++;
            if (input[i] != "your ticket:")
            {
                ParsingError($"Expected \"your ticket:\", found \"{input[i].Left(20, true)}\".");
                return;
            }
            i++;
            if (!TryParseTicket(input[i], rules.Count, out Ticket? yourTicket))
            {
                ParsingError($"Invalid ticket \"{input[i].Left(20, true)}\".");
                return;
            }
            YourTicket = yourTicket;
            // Read nearby tickets.
            i += 2;
            if (input[i] != "nearby tickets:")
            {
                ParsingError($"Expected \"nearby tickets:\", found \"{input[i].Left(20, true)}\".");
                return;
            }
            i++;
            for (; i < input.Length; i++)
            {
                if (input[i].Length == 0)
                {
                    break;
                }
                if (!TryParseTicket(input[i], rules.Count, out Ticket? ticket))
                {
                    ParsingError($"Invalid ticket \"{input[i].Left(20, true)}\".");
                    return;
                }
                nearbyTickets.Add(ticket);
            }
            if (nearbyTickets.Count == 0)
            {
                ParsingError($"No nearby tickets were found.");
                return;
            }
            Success = true;
            Error = string.Empty;
            Rules = rules.ToArray();
            NearbyTickets = nearbyTickets.ToArray();
        }

        private static bool TryParseRule(string line, [NotNullWhen(true)] out Rule? rule)
        {
            rule = null;
            int lengthOfFieldName = line.IndexOf(':');
            if (lengthOfFieldName < 0)
            {
                return false;
            }
            string name = line[..lengthOfFieldName].ToString();
            string[] terms = line[(lengthOfFieldName + 1)..].ToString()
                .Split(_split, StringSplitOptions.RemoveEmptyEntries);
            List<(int, int)> ranges = new();
            foreach (string term in terms)
            {
                if (int.TryParse(term, out int solo))
                {
                    ranges.Add((solo, solo));
                    continue;
                }
                int dash = term.IndexOf('-');
                if (dash < 0)
                {
                    return false;
                }
                if (!int.TryParse(term[..dash], out int first) |
                    !int.TryParse(term[(dash + 1)..], out int second))
                {
                    return false;
                }
                if (second < first)
                {
                    ranges.Add((second, first));
                }
                else
                {
                    ranges.Add((first, second));
                }
            }
            rule = new(name, ranges);
            return true;
        }

        private static bool TryParseTicket(string line, int fieldCount, [NotNullWhen(true)] out Ticket? ticket)
        {
            ticket = null;
            string[] terms = line.Split(',');
            if (terms.Length != fieldCount)
            {
                return false;
            }
            int[] fieldValues = new int[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                if (!int.TryParse(terms[i], out fieldValues[i]))
                {
                    return false;
                }
            }
            ticket = new(fieldValues);
            return true;
        }

        [MemberNotNull(nameof(Error), nameof(Rules), nameof(YourTicket), nameof(NearbyTickets))]
        private void ParsingError(string message)
        {
            Success = false;
            Error = message;
            Rules = Array.Empty<Rule>();
            YourTicket = Ticket.Empty;
            NearbyTickets = Array.Empty<Ticket>();
        }
    }

    private class Rule
    {
        public string Name { get; private set; }
        public (int, int)[] Ranges { get; private set; }

        public Rule(string name, List<(int, int)> ranges)
        {
            Name = name;
            Ranges = ranges.ToArray();
        }

        public bool ValueIsValid(int value)
        {
            foreach ((int, int) range in Ranges)
            {
                if (range.Item1 <= value && value <= range.Item2)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private class Ticket
    {
        public static readonly Ticket Empty = new(Array.Empty<int>());

        public int[] FieldValues { get; private set; }

        public Ticket(int[] fieldValues)
        {
            FieldValues = fieldValues;
        }
    }
}
