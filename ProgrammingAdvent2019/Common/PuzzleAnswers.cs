// ProgrammingAdvent2019 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Diagnostics;
using ProgrammingAdvent2019.Solutions;

namespace ProgrammingAdvent2019.Common;

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
        PartOneAnswer = "Part One solution not yet implemented.";
        PartTwoAnswer = "Part Two solution not yet implemented.";
        ElapsedMilliseconds = long.MinValue / 2;
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
        IsError = true;
        ErrorMessage = "ERROR: " + message;
        return this;
    }

    public PuzzleAnswers WriteDayNumber(Day day)
    {
        string className = day.GetType().ToString();
        if (className.Length >= 5 && int.TryParse(className[^2..^0], out int dayNumber))
        {
            DayNumber = dayNumber;
        }
        return this;
    }

    public override string ToString()
    {
        string output;
        if (IsError)
        {
            output = $"Day {DayNumber:00} {ErrorMessage}";
        }
        else
        {
            output = $"Day {DayNumber:00} Part One: {PartOneAnswer}{Environment.NewLine}Day {DayNumber:00} Part Two: {PartTwoAnswer}";
        }
        if (ElapsedMilliseconds > 0)
        {
            output += $"{Environment.NewLine}Day {DayNumber:00} Time: {ElapsedMilliseconds} ms";
        }
        return output;
    }
}
