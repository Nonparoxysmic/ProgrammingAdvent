// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day02 : Day
{
    private static readonly Regex _validCharacters = new("^[0-9,]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines.FirstOrDefault(s => s.Length > 0) is null)
        {
            errorMessage = "No input.";
            return false;
        }
        string input = inputLines.First(s => s.Length > 0);
        if (!_validCharacters.IsMatch(input))
        {
            errorMessage = $"Input \"{input.Left(20, true)}\" contains invalid characters.";
            return false;
        }
        string[] terms = input.Split(',');
        if (terms.Length < 4)
        {
            errorMessage = "Insufficient input.";
            return false;
        }
        foreach (string term in terms)
        {
            if (!int.TryParse(term, out int _))
            {
                errorMessage = $"Term \"{term.Left(20, true)}\" cannot be parsed as an Int32.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        int[] input = inputLines.First(s => s.Length > 0).Split(',').Select(s => int.Parse(s)).ToArray();
        input[1] = 12;
        input[2] = 2;
        if (!RunProgram(input, out string errorMessage))
        {
            output.WriteError(errorMessage);
            return output;
        }
        return output.WriteAnswers(input[0], null);
    }

    private static bool RunProgram(int[] input, out string errorMessage)
    {
        int position = 0;
        while (true)
        {
            if (position >= input.Length)
            {
                errorMessage = "Program counter went out of bounds.";
                return false;
            }
            if (input[position] == 99)
            {
                errorMessage = string.Empty;
                return true;
            }
            if (position > input.Length - 4)
            {
                errorMessage = "Unknown error.";
                return false;
            }
            int pos1 = input[position + 1];
            int pos2 = input[position + 2];
            int pos3 = input[position + 3];
            if (pos1 < 0 || pos1 >= input.Length ||
                pos2 < 0 || pos2 >= input.Length ||
                pos3 < 0 || pos3 >= input.Length)
            {
                errorMessage = "Encountered invalid position data.";
                return false;
            }
            if (input[position] == 1)
            {
                input[pos3] = input[pos1] + input[pos2];
            }
            else if (input[position] == 2)
            {
                input[pos3] = input[pos1] * input[pos2];
            }
            else
            {
                errorMessage = "Encountered an unknown opcode.";
                return false;
            }
            position += 4;
        }
    }
}
