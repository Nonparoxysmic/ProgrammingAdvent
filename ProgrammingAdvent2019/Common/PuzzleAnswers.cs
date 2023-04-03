// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Diagnostics;

namespace ProgrammingAdvent2019.Common;

internal class PuzzleAnswers
{
    public string PartOneAnswer { get; private set; }
    public string PartTwoAnswer { get; private set; }
    public long ElapsedMilliseconds { get; private set; }
    public bool IsError { get; private set; }
    public string ErrorMessage { get; private set; }

    public PuzzleAnswers()
    {
        PartOneAnswer = "Part One solution not yet implemented.";
        PartTwoAnswer = "Part Two solution not yet implemented.";
        ElapsedMilliseconds = long.MinValue / 2;
        ErrorMessage = string.Empty;
    }

    public void WriteAnswers(object partOne, object partTwo)
    {
        if (partOne is not null)
        {
            if (partOne is string stringParam)
            {
                PartOneAnswer = stringParam;
            }
            else if (partOne is char[] charArray)
            {
                PartOneAnswer = string.Join(null, charArray);
            }
            else
            {
                string? str = partOne.ToString();
                if (str is not null)
                {
                    PartOneAnswer = str;
                }
            }
        }
        if (partTwo is not null)
        {
            if (partTwo is string stringParam)
            {
                PartTwoAnswer = stringParam;
            }
            else if (partTwo is char[] charArray)
            {
                PartTwoAnswer = string.Join(null, charArray);
            }
            else
            {
                string? str = partTwo.ToString();
                if (str is not null)
                {
                    PartTwoAnswer = str;
                }
            }
        }
    }

    public void WriteTime(Stopwatch stopwatch) => WriteTime(stopwatch.ElapsedMilliseconds);

    public void WriteTime(long milliseconds)
    {
        if (milliseconds >= 0)
        {
            ElapsedMilliseconds = milliseconds;
        }
    }

    public void WriteError(string message)
    {
        IsError = true;
        ErrorMessage = "ERROR: " + message;
    }
}
