// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day12 : Day
    {
        private readonly Regex validInitialState = new Regex(@"^initial state: ([#\.]+)$");
        private readonly Regex validRule = new Regex(@"^[#\.]{5} => [#\.]$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 2)
            {
                output.WriteError("Insufficient input.", sw);
                return output;
            }

            Match initialStateMatch = validInitialState.Match(inputLines[0]);
            if (!initialStateMatch.Success || !inputLines[0].Contains('#'))
            {
                output.WriteError("Invalid first line in input.", sw);
                return output;
            }
            string initialState = initialStateMatch.Groups[1].Value;
            PotCollection pots = new PotCollection();
            for (int i = 0; i < initialState.Length; i++)
            {
                if (initialState[i] == '#')
                {
                    pots.AddLast(1);
                }
                else
                {
                    pots.AddLast(0);
                }
            }

            SortedDictionary<int, int> rules = new SortedDictionary<int, int>();
            for (int i = 1; i < inputLines.Length; i++)
            {
                if (i == 1 && inputLines[i] == "")
                {
                    continue;
                }
                Match ruleMatch = validRule.Match(inputLines[i]);
                if (!ruleMatch.Success || inputLines[i] == "..... => #")
                {
                    output.WriteError($"Invalid line in input \"{inputLines[i]}\".", sw);
                    return output;
                }
                int ruleInput = 0;
                for (int j = 0; j < 5; j++)
                {
                    if (ruleMatch.Value[j] == '#')
                    {
                        ruleInput |= (1 << (4 - j));
                    }
                }
                int ruleOutput = ruleMatch.Value[9] == '#' ? 1 : 0;
                rules.Add(ruleInput, ruleOutput);
            }
            for (int i = 0; i < 32; i++)
            {
                if (!rules.ContainsKey(i))
                {
                    rules.Add(i, 0);
                }
            }

            for (int i = 0; i < 20; i++)
            {
                PotCollection nextGen = new PotCollection(pots.LowestPotNumber - 2);
                foreach (byte pattern in pots.NextGenPatterns())
                {
                    nextGen.AddLast(rules[pattern]);
                }
                nextGen.Trim();
                pots = nextGen;
            }

            int partOneAnswer = pots.PartOneSum();

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        class PotCollection
        {
            public int Count { get => pots.Count; }
            public int LowestPotNumber { get; private set; }

            private readonly LinkedList<int> pots;

            public PotCollection()
            {
                pots = new LinkedList<int>();
                LowestPotNumber = 0;
            }

            public PotCollection(int firstPot)
            {
                pots = new LinkedList<int>();
                LowestPotNumber = firstPot;
            }

            public LinkedListNode<int> AddLast(int n)
            {
                return pots.AddLast(n);
            }

            public int PartOneSum()
            {
                int sum = 0;
                int index = LowestPotNumber;
                foreach (int pot in pots)
                {
                    if (pot == 1)
                    {
                        sum += index;
                    }
                    index++;
                }
                return sum;
            }

            public void Trim()
            {
                if (pots.Count < 2)
                {
                    return;
                }
                while (pots.First.Value == 0 && pots.Count > 1)
                {
                    pots.RemoveFirst();
                    LowestPotNumber++;
                }
                while (pots.Last.Value == 0 && pots.Count > 1)
                {
                    pots.RemoveLast();
                }
            }

            public IEnumerable<int> NextGenPatterns()
            {
                LinkedListNode<int> currentNode = pots.First;
                int currentPattern = 0;
                for (int i = LowestPotNumber - 4; i < LowestPotNumber + Count; i++)
                {
                    currentPattern = (currentPattern << 1) & 0b0001_1110;
                    if (currentNode != null)
                    {
                        currentPattern |= currentNode.Value;
                        currentNode = currentNode.Next;
                    }
                    yield return currentPattern;
                }
                yield break;
            }
        }
    }
}
