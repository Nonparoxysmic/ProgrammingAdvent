// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day08 : Day
{
    private static readonly Regex _validLine = new("^(?<op>acc|jmp|nop) (?<arg>[+-][0-9]{1,4})$");

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

        Instruction[] instructions = new Instruction[input.Length];
        for (int i = 0; i < instructions.Length; i++)
        {
            Match match = _validLine.Match(input[i]);
            char op = match.Groups["op"].Value[0];
            int arg = int.Parse(match.Groups["arg"].Value);
            instructions[i] = new(op, arg);
        }

        int programCounter = 0, accumulator = 0;
        HashSet<int> performedInstructions = new();
        while (0 <= programCounter && programCounter < instructions.Length)
        {
            if (!performedInstructions.Add(programCounter))
            {
                break;
            }
            Instruction current = instructions[programCounter];
            if (current.Operation == 'a')
            {
                accumulator += current.Argument;
            }
            else if (current.Operation == 'j')
            {
                programCounter += current.Argument - 1;
            }
            programCounter++;
        }
        int partOneAnswer = accumulator;

        return output.WriteAnswers(partOneAnswer, null);
    }

    private class Instruction
    {
        public char Operation { get; private set; }
        public int Argument { get; private set; }

        public Instruction(char operation, int argument)
        {
            Operation = operation;
            Argument = argument;
        }
    }
}
