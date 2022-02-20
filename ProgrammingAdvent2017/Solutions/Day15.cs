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

            Day15Generator genA = new Day15Generator(16807, int.Parse(inputLines[0][24..]));
            Day15Generator genB = new Day15Generator(48271, int.Parse(inputLines[1][24..]));

            int matches = 0;
            for (int i = 0; i < 40_000_000; i++)
            {
                if (Judge(genA.Generate(), genB.Generate()))
                {
                    matches++;
                }
            }

            sw.Stop();
            output.WriteAnswers(matches, null, sw);
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
    }
}
