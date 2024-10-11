// ProgrammingAdvent2022 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Solutions;
using System.Diagnostics;

namespace ProgrammingAdvent2022.Common;

internal class PuzzleAnswers
{
    public int DayNumber { get; private set; }
    public string PartOneAnswer { get; private set; }
    public string PartTwoAnswer { get; private set; }
    public long ElapsedMilliseconds { get; private set; }
    public bool IsError { get; private set; }
    public string ErrorMessage { get; private set; }

    public PuzzleAnswers()
    {
        DayNumber = -1;
        PartOneAnswer = "Solution not yet implemented.";
        PartTwoAnswer = "Solution not yet implemented.";
        ElapsedMilliseconds = -1;
        ErrorMessage = string.Empty;
    }

    public PuzzleAnswers WriteAnswers(object? partOne, object? partTwo)
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
        return this;
    }

    public PuzzleAnswers WriteTime(Stopwatch stopwatch) => WriteTime(stopwatch.ElapsedMilliseconds);

    public PuzzleAnswers WriteTime(long milliseconds)
    {
        if (milliseconds >= 0)
        {
            ElapsedMilliseconds = milliseconds + 1;
        }
        return this;
    }

    public PuzzleAnswers WriteError(string message)
    {
        if (IsError)
        {
            ErrorMessage += $" AND {message}";
            return this;
        }
        IsError = true;
        ErrorMessage = message;
        return this;
    }

    public PuzzleAnswers WriteDayNumber(Day day)
    {
        string className = day.GetType().ToString();
        if (className.Length >= 5 && int.TryParse(className[^2..^0], out int dayNumber))
        {
            DayNumber = dayNumber;
            return this;
        }
        return WriteError($"Invalid class name \"{className}\"");
    }
}
