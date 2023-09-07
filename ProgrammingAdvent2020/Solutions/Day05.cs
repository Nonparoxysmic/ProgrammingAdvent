// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day05 : Day
{
    private static readonly Regex _validLine = new("^[FB]{7}[LR]{3}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        int maxSeatID = 0;
        foreach (string pass in input)
        {
            maxSeatID = Math.Max(maxSeatID, SeatID(BoardingPassRowAndColumn(pass)));
        }

        return output.WriteAnswers(maxSeatID, null);
    }

    private static (int, int) BoardingPassRowAndColumn(string boardingPass)
    {
        int row = 0, col = 0;
        int power = 1;
        for (int i = 6; i >= 0; i--)
        {
            if (boardingPass[i] == 'B')
            {
                row += power;
            }
            power *= 2;
        }
        power = 1;
        for (int i = 9; i >= 7; i--)
        {
            if (boardingPass[i] == 'R')
            {
                col += power;
            }
            power *= 2;
        }
        return (row, col);
    }

    private static int SeatID((int row, int column) pass) => SeatID(pass.row, pass.column);

    private static int SeatID(int row, int column)
    {
        return row * 8 + column;
    }
}
