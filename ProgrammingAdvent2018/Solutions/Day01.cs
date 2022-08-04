// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day01 : Day
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

            int[] changes = new int[inputLines.Length];
            int sum = 0;
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (int.TryParse(inputLines[i], out int result))
                {
                    changes[i] = result;
                    sum += result;
                }
                else
                {
                    output.WriteError($"Cannot parse \"{inputLines[i]}\" as an integer.", sw);
                    return output;
                }
            }

            HashSet<int> frequenciesSeen = new HashSet<int> { 0 };
            int frequency = 0;
            for (int i = 0; i < int.MaxValue; i++)
            {
                frequency += changes[i % changes.Length];
                if (!frequenciesSeen.Add(frequency)) { break; }
            }

            sw.Stop();
            output.WriteAnswers(sum, frequency, sw);
            return output;
        }
    }
}
