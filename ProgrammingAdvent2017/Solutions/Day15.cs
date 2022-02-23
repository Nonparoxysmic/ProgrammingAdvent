// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day15 : Day
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
            if (inputLines.Length < 2
                || !Regex.IsMatch(inputLines[0], @"^Generator A starts with \d+$")
                || !Regex.IsMatch(inputLines[1], @"^Generator B starts with \d+$"))
            {
                output.WriteError("Invalid input.", sw);
                return output;
            }

            Day15Generator genA = new Day15Generator(16807, int.Parse(inputLines[0][24..]));
            Day15Generator genB = new Day15Generator(48271, int.Parse(inputLines[1][24..]));

            int matchesPartOne = 0;
            int matchesPartTwo = 0;
            int consideredPartTwo = 0;
            Queue<int> queueA = new Queue<int>();
            Queue<int> queueB = new Queue<int>();
            for (int i = 0; i < 40_000_000; i++)
            {
                int numA = genA.Generate();
                int numB = genB.Generate();
                if (Judge(numA, numB))
                {
                    matchesPartOne++;
                }
                if ((numA & 3) == 0)
                {
                    queueA.Enqueue(numA);
                }
                if ((numB & 7) == 0)
                {
                    queueB.Enqueue(numB);
                }
                if (queueA.Count > 1 && queueB.Count > 1)
                {
                    if (consideredPartTwo >= 5_000_000) { continue; }
                    if (Judge(queueA.Dequeue(), queueB.Dequeue()))
                    {
                        matchesPartTwo++;
                    }
                    consideredPartTwo++;
                }
            }
            while (consideredPartTwo < 5_000_000)
            {
                if (queueA.Count == 0)
                {
                    queueA.Enqueue(genA.PickyGenerate(3));
                }
                if (queueB.Count == 0)
                {
                    queueB.Enqueue(genB.PickyGenerate(7));
                }
                if (Judge(queueA.Dequeue(), queueB.Dequeue()))
                {
                    matchesPartTwo++;
                }
                consideredPartTwo++;
            }

            sw.Stop();
            output.WriteAnswers(matchesPartOne, matchesPartTwo, sw);
            return output;
        }

        private bool Judge(int a, int b)
        {
            if ((a & 0xFFFF) == (b & 0xFFFF))
            {
                return true;
            }
            return false;
        }
    }

    internal class Day15Generator
    {
        readonly ulong factor;
        ulong previous;

        internal Day15Generator(int factor, int startingValue)
        {
            this.factor = (ulong)factor;
            previous = (ulong)startingValue;
        }

        internal int Generate()
        {
            previous = (factor * previous) % int.MaxValue;
            return (int)previous;
        }

        internal int PickyGenerate(int number)
        {
            int output;
            do
            {
                output = Generate();
            } while ((output & number) > 0);
            return output;
        }
    }
}
