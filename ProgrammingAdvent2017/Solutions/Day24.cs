// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day24 : Day
    {
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
            List<Day24Component> componentsList = new List<Day24Component>();
            foreach (string line in inputLines)
            {
                var match = Regex.Match(line, @"^(\d+)/(\d+)$");
                if (!match.Success)
                {
                    output.WriteError($"Invalid line in input: \"{line}\"", sw);
                    return output;
                }
                int number1 = int.Parse(match.Groups[1].Value);
                int number2 = int.Parse(match.Groups[2].Value);
                componentsList.Add(new Day24Component(number1, number2));
            }
            Day24Component[] components = componentsList.ToArray();

            int partOneAnswer = StrongestBridgeStrength(components, 0);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private int StrongestBridgeStrength(Day24Component[] components, int connectionValue)
        {
            int strongest = 0;
            for (int i = 0; i < components.Length; i++)
            {
                Day24Component c = components[i];
                if (c != null && c.CanConnect(connectionValue))
                {
                    components[i] = null;
                    int freeEnd = c.FreeEndValue(connectionValue);
                    strongest = Math.Max(strongest,
                        StrongestBridgeStrength(components, freeEnd) + c.Strength);
                    components[i] = c;
                }
            }
            return strongest;
        }

        private class Day24Component
        {
            internal int Item1 { get; set; }
            internal int Item2 { get; set; }
            internal int Strength { get => Item1 + Item2; }

            public Day24Component(int a, int b)
            {
                Item1 = a;
                Item2 = b;
            }

            internal bool CanConnect(int a)
            {
                return Item1 == a || Item2 == a;
            }

            internal int FreeEndValue(int connectedEndValue)
            {
                return connectedEndValue == Item1
                    ? Item2
                    : Item1;
            }
        }
    }
}
