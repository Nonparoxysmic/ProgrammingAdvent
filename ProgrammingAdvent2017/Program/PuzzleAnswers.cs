// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

namespace ProgrammingAdvent2017.Program
{
    class PuzzleAnswers
    {
        public string PartOneAnswer { get; set; }
        public string PartTwoAnswer { get; set; }
        public long ElapsedMilliseconds { get; set; }

        public PuzzleAnswers()
        {
            PartOneAnswer = "Part One solution not yet implemented.";
            PartTwoAnswer = "Part Two solution not yet implemented.";
            ElapsedMilliseconds = -1;
        }

        public void WriteAnswers(string partOne, string partTwo, long milliseconds)
        {
            if (partOne != null) { PartOneAnswer = partOne; }
            if (partTwo != null) { PartTwoAnswer = partTwo; }
            ElapsedMilliseconds = milliseconds;
        }
    }
}
