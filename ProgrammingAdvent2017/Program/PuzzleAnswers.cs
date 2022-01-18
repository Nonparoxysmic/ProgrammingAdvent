// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

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

        internal void WriteAnswers(string partOne, string partTwo, long milliseconds)
        {
            if (partOne != null) { PartOneAnswer = partOne; }
            if (partTwo != null) { PartTwoAnswer = partTwo; }
            ElapsedMilliseconds = milliseconds;
        }
    }
}
