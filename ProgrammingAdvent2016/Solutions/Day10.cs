// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProgrammingAdvent2016
{
    public class Day10 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            Dictionary<int, Day10Bot> bots = new Dictionary<int, Day10Bot>();
            Dictionary<int, List<int>> outputs = new Dictionary<int, List<int>>();
            Queue<int> startingChips = new Queue<int>();
            List<int> partOneSolution = new List<int>();
            foreach (string line in inputLines)
            {
                string[] terms = line.Split();
                if (terms.Length == 12)
                {
                    if (int.TryParse(terms[1], out int botNumber) && int.TryParse(terms[6], out int lowNumber) && int.TryParse(terms[11], out int highNumber))
                    {
                        if (bots.ContainsKey(botNumber))
                        {
                            solution.WriteSolution(1, "ERROR: duplicate bot number: " + botNumber, stopwatch.ElapsedMilliseconds);
                            return solution;
                        }
                        OutputTarget lowTarget = terms[5] == "bot" ? OutputTarget.Bot : OutputTarget.OutputBin;
                        OutputTarget highTarget = terms[10] == "bot" ? OutputTarget.Bot : OutputTarget.OutputBin;
                        if (lowTarget == OutputTarget.OutputBin && !outputs.ContainsKey(lowNumber)) outputs.Add(lowNumber, new List<int>());
                        if (highTarget == OutputTarget.OutputBin && !outputs.ContainsKey(highNumber)) outputs.Add(highNumber, new List<int>());
                        bots.Add(botNumber, new Day10Bot(botNumber, lowTarget, lowNumber, highTarget, highNumber));
                    }
                }
                else if (terms.Length == 6)
                {
                    if (int.TryParse(terms[1], out int chipValue) && int.TryParse(terms[5], out int botNumber))
                    {
                        startingChips.Enqueue(botNumber);
                        startingChips.Enqueue(chipValue);
                    }
                }
            }
            while (startingChips.Count > 1)
            {
                if (bots.ContainsKey(startingChips.Peek()))
                {
                    int botNumber = startingChips.Dequeue();
                    int chipValue = startingChips.Dequeue();
                    bots[botNumber].AddChip(chipValue, bots, outputs, partOneSolution);
                }
                else
                {
                    solution.WriteSolution(1, "ERROR: value for invalid bot: " + startingChips.Peek(), stopwatch.ElapsedMilliseconds);
                    return solution;
                }
            }
            if (partOneSolution.Count > 0)
            {
                solution.WriteSolution(1, partOneSolution[0], stopwatch.ElapsedMilliseconds);
            }
            else
            {
                solution.WriteSolution(1, "ERROR: no solution found.", stopwatch.ElapsedMilliseconds);
            }

            stopwatch.Reset();
            return solution;
        }
    }

    class Day10Bot
    {
        public OutputTarget lowTarget;
        public int lowNumber;
        public OutputTarget highTarget;
        public int highNumber;

        public List<int> chips;
        readonly int botNumber;

        public Day10Bot(int botNumber, OutputTarget lowTarget, int lowNumber, OutputTarget highTarget, int highNumber)
        {
            this.lowTarget = lowTarget;
            this.lowNumber = lowNumber;
            this.highTarget = highTarget;
            this.highNumber = highNumber;
            chips = new List<int>();
            this.botNumber = botNumber;
        }

        public void AddChip(int value, Dictionary<int, Day10Bot> bots, Dictionary<int, List<int>> outputs, List<int> partOneSolution)
        {
            chips.Add(value);
            if (chips.Count == 2)
            {
                if (chips.Min() == 17 && chips.Max() == 61)
                {
                    partOneSolution.Add(botNumber);
                }
                if (lowTarget == OutputTarget.Bot && bots.ContainsKey(lowNumber))
                {
                    bots[lowNumber].AddChip(chips.Min(), bots, outputs, partOneSolution);
                }
                else if (lowTarget == OutputTarget.OutputBin && outputs.ContainsKey(lowNumber))
                {
                    outputs[lowNumber].Add(chips.Min());
                }
                if (highTarget == OutputTarget.Bot && bots.ContainsKey(highNumber))
                {
                    bots[highNumber].AddChip(chips.Max(), bots, outputs, partOneSolution);
                }
                else if (highTarget == OutputTarget.OutputBin && outputs.ContainsKey(highNumber))
                {
                    outputs[highNumber].Add(chips.Max());
                }
                chips.Clear();
            }
        }
    }

    enum OutputTarget
    {
        OutputBin,
        Bot
    }
}
