// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day05 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            int[] inputValues = new int[inputLines.Length];
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (!int.TryParse(inputLines[i], out inputValues[i]))
                {
                    output.WriteError($"Input line \"{inputLines[i]}\" is not an integer.", sw);
                    return output;
                }
            }

            int steps = StepsToExit((int[])inputValues.Clone());

            sw.Stop();
            output.WriteAnswers(steps, null, sw);
            return output;
        }

        private int StepsToExit(int[] offsets)
        {
            int steps = 0;
            for (int i = 0; 0 <= i && i < offsets.Length; steps++)
            {
                int currentOffset = offsets[i]++;
                i += currentOffset;
            }
            return steps;
        }
    }
}
