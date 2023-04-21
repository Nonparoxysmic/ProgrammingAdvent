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
        if (!RunProgram(input, out int partOneAnswer, out string error1))
        {
            output.WriteError(error1);
            return output;
        }
        if (!PartTwoAnswer(input, out int partTwoAnswer, out string error2))
        {
            return output.WriteAnswers(partOneAnswer, error2);
        }
        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static bool RunProgram(int[] input, out int result, out string errorMessage)
    {
        result = -1;
        int[] program = new int[input.Length];
        Array.Copy(input, program, input.Length);
        int position = 0;
        while (true)
        {
            if (position >= program.Length)
            {
                errorMessage = "Program counter went out of bounds.";
                return false;
            }
            if (program[position] == 99)
            {
                errorMessage = string.Empty;
                result = program[0];
                return true;
            }
            if (position > program.Length - 4)
            {
                errorMessage = "Unknown error.";
                return false;
            }
            int pos1 = program[position + 1];
            int pos2 = program[position + 2];
            int pos3 = program[position + 3];
            if (pos1 < 0 || pos1 >= program.Length ||
                pos2 < 0 || pos2 >= program.Length ||
                pos3 < 0 || pos3 >= program.Length)
            {
                errorMessage = "Encountered invalid position data.";
                return false;
            }
            if (program[position] == 1)
            {
                program[pos3] = program[pos1] + program[pos2];
            }
            else if (program[position] == 2)
            {
                program[pos3] = program[pos1] * program[pos2];
            }
            else
            {
                errorMessage = "Encountered an unknown opcode.";
                return false;
            }
            position += 4;
        }
    }

    private static bool PartTwoAnswer(int[] input, out int answer, out string errorMessage)
    {
        for (int noun = 0; noun < 100; noun++)
        {
            for (int verb = 0; verb < 100; verb++)
            {
                input[1] = noun;
                input[2] = verb;
                if (RunProgram(input, out int result, out string _) && result == 19690720)
                {
                    errorMessage = string.Empty;
                    answer = 100 * noun + verb;
                    return true;
                }
            }
        }
        errorMessage = "No valid answer exists.";
        answer = -1;
        return false;
    }
}
