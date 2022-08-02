// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class DayTemplate : Day
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
            //string[] inputLines = input.ToLines();

            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }
    }
}
