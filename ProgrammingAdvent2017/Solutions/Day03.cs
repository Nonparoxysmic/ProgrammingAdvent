// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day03 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (!int.TryParse(input.Trim(), out int inputValue))
            {
                output.WriteAnswers("ERROR: Input is not an integer.", "N/A",
                            sw.ElapsedMilliseconds);
                return output;
            }
            if (inputValue < 1)
            {
                output.WriteAnswers("ERROR: Input is less than one.", "N/A",
                            sw.ElapsedMilliseconds);
                return output;
            }


            sw.Stop();
            output.WriteAnswers(null, null, sw.ElapsedMilliseconds);
            return output;
        }
    }
}
