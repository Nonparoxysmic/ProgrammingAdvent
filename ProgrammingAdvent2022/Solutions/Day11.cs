// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day11 : Day
{
    private static Monkey[] AllMonkeys = [];

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        AllMonkeys = ParseMonkeys(input);
        ProcessRounds(20);
        int monkeyBusiness = AllMonkeys.OrderByDescending(m => m.Inspections)
            .Take(2).Aggregate(1, (product, monkey) => product * monkey.Inspections);

        return result.WriteAnswers(monkeyBusiness, null);
    }

    private static Monkey[] ParseMonkeys(string[] input)
    {
        List<Monkey> monkeys = [];
        int current = 0;
        for (int i = 0; i < input.Length - 5; i += 7)
        {
            if (input[i].Length >= 9 && int.TryParse(input[i][7..^1], out int id) && id == current)
            {
                string[] terms = input[i + 1].Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);
                List<int> items = [];
                for (int j = 2; j < terms.Length; j++)
                {
                    if (!int.TryParse(terms[j], out int item))
                    {
                        return [.. monkeys];
                    }
                    items.Add(item);
                }

                if (input[i + 2].Length < 26 || (input[i + 2][23] != '+' && input[i + 2][23] != '*'))
                {
                    return [.. monkeys];
                }
                Func<int, int> operation;
                if (input[i + 2][23..] == "* old")
                {
                    operation = x => x * x;
                }
                else if (!int.TryParse(input[i + 2][25..], out int modifier))
                {
                    return [.. monkeys];
                }
                else if (input[i + 2][23] == '*')
                {
                    operation = x => x * modifier;
                }
                else
                {
                    operation = x => x + modifier;
                }
                if (input[i + 3].Length < 22 | !int.TryParse(input[i + 3][21..], out int divisor))
                {
                    return [.. monkeys];
                }
                bool Test(int x) => x % divisor == 0;
                if (input[i + 4].Length < 30 || !int.TryParse(input[i + 4][29..], out int truePass))
                {
                    return [.. monkeys];
                }
                if (input[i + 5].Length < 31 || !int.TryParse(input[i + 5][30..], out int falsePass))
                {
                    return [.. monkeys];
                }
                Monkey m = new(id, operation, Test, truePass, falsePass);
                foreach (int item in items)
                {
                    m.Items.Enqueue(item);
                }
                monkeys.Add(m);
            }
            else
            {
                return [.. monkeys];
            }
            current++;
        }
        return [.. monkeys];
    }

    private static void ProcessRounds(int rounds)
    {
        for (int i = 0; i < rounds; i++)
        {
            for (int j = 0; j < AllMonkeys.Length; j++)
            {
                AllMonkeys[j].TakeTurn();
            }
        }
    }

    private class Monkey(int id, Func<int, int> op, Predicate<int> test, int passT, int passF)
    {
        public int ID { get; } = id;
        public int Inspections { get; private set; }
        public Queue<int> Items { get; } = [];
        public Func<int, int> Operation { get; } = op;
        public Predicate<int> Test { get; } = test;
        public int TruePass { get; } = passT;
        public int FalsePass { get; } = passF;

        public void TakeTurn()
        {
            while (Items.Count > 0)
            {
                int current = Items.Dequeue();
                Inspections++;
                current = Operation(current);
                current /= 3;
                if (Test(current))
                {
                    AllMonkeys[TruePass].Items.Enqueue(current);
                }
                else
                {
                    AllMonkeys[FalsePass].Items.Enqueue(current);
                }
            }
        }
    }
}
