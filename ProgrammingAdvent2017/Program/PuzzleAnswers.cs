// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;

namespace ProgrammingAdvent2017.Program
{
    internal class PuzzleAnswers
    {
        internal string PartOneAnswer { get; set; }
        internal string PartTwoAnswer { get; set; }
        internal long ElapsedMilliseconds { get; set; }

        internal PuzzleAnswers()
        {
            PartOneAnswer = "Part One solution not yet implemented.";
            PartTwoAnswer = "Part Two solution not yet implemented.";
            ElapsedMilliseconds = -1;
        }

        internal void WriteAnswers(object partOne, object partTwo, Stopwatch sw)
        {
            if (partOne != null) { PartOneAnswer = partOne.ToString(); }
            if (partTwo != null) { PartTwoAnswer = partTwo.ToString(); }
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
        }

        internal void WriteError(string message, Stopwatch sw)
        {
            PartOneAnswer = "ERROR: " + message;
            PartTwoAnswer = "N/A";
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
        }
    }
}
