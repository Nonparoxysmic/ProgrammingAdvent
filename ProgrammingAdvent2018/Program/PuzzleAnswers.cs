// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;

namespace ProgrammingAdvent2018.Program
{
    internal class PuzzleAnswers
    {
        internal string PartOneAnswer { get; set; }
        internal string PartTwoAnswer { get; set; }
        internal long ElapsedMilliseconds { get; set; }
        internal SimpleBitmap PartOneBitmap { get; set; }

        internal PuzzleAnswers()
        {
            PartOneAnswer = "Part One solution not yet implemented.";
            PartTwoAnswer = "Part Two solution not yet implemented.";
            ElapsedMilliseconds = -1;
        }

        internal void WriteAnswers(object partOne, object partTwo, Stopwatch sw)
        {
            if (partOne != null)
            {
                if (partOne is char[] charArray)
                {
                    PartOneAnswer = string.Join(null, charArray);
                }
                else if (partOne is SimpleBitmap bitmap)
                {
                    PartOneAnswer = "See image.";
                    PartOneBitmap = bitmap;
                }
                else
                {
                    PartOneAnswer = partOne.ToString();
                }
            }
            if (partTwo != null)
            {
                if (partTwo is char[] charArray)
                {
                    PartTwoAnswer = string.Join(null, charArray);
                }
                else
                {
                    PartTwoAnswer = partTwo.ToString();
                }
            }
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
        }

        internal void WriteError(string message, Stopwatch sw)
        {
            PartOneAnswer = "ERROR: " + message;
            PartTwoAnswer = "N/A";
            ElapsedMilliseconds = sw.ElapsedMilliseconds;
        }

        internal static string MillisecondsToDisplayTime(long ms)
        {
            double seconds = ((ms / 10) + 1) / 100.0;
            return seconds.ToString("F2") + " seconds";
        }
    }
}
