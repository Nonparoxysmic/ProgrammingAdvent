// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

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

            Day15Generator genA = new Day15Generator
                (
                    Day15Generator.GeneratorType.GeneratorA,
                    int.Parse(inputLines[0][24..])
                );
            Day15Generator genB = new Day15Generator
                (
                    Day15Generator.GeneratorType.GeneratorB,
                    int.Parse(inputLines[1][24..])
                );

            int matchesPartOne = 0;
            for (int i = 0; i < 40_000_000; i++)
            {
                if (Judge(genA.Generate(), genB.Generate()))
                {
                    matchesPartOne++;
                }
            }

            genA.Reset();
            genB.Reset();

            int matchesPartTwo = 0;
            for (int i = 0; i < 5_000_000; i++)
            {
                if (Judge(genA.PickyGenerate(), genB.PickyGenerate()))
                {
                    matchesPartTwo++;
                }
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
        readonly int pickyNumber;
        readonly int startingValue;
        readonly ulong factor;
        ulong previous;

        internal Day15Generator(GeneratorType type, int startingValue)
        {
            this.startingValue = startingValue;
            previous = (ulong)startingValue;
            switch (type)
            {
                case GeneratorType.GeneratorA:
                    factor = 16807;
                    pickyNumber = 3;
                    break;
                case GeneratorType.GeneratorB:
                    factor = 48271;
                    pickyNumber = 7;
                    break;
            }
        }

        internal void Reset()
        {
            previous = (ulong)startingValue;
        }

        internal int Generate()
        {
            previous = (factor * previous) % int.MaxValue;
            return (int)previous;
        }

        internal int PickyGenerate()
        {
            int output;
            do
            {
                output = Generate();
            } while ((output & pickyNumber) > 0);
            return output;
        }

        internal enum GeneratorType
        {
            GeneratorA,
            GeneratorB
        }
    }
}
