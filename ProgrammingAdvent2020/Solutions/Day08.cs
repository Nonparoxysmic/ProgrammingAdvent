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
        while (0 <= programCounter && programCounter < instructions.Length)
        {
            Instruction current = instructions[programCounter];
            if (current.InTheLoop)
            {
                break;
            }
            current.InTheLoop = true;
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

        List<int> possibleSwaps = SwapsToTry(instructions, out int? answer);
        if (answer is not null)
        {
            possibleSwaps.Clear();
            possibleSwaps.Add((int)answer);
        }
        if (possibleSwaps.Count == 0)
        {
            return output.WriteAnswers(partOneAnswer, "No answer found.");
        }
        int? partTwoAnswer = PartTwoAnswer(possibleSwaps, instructions);
        if (partTwoAnswer is null)
        {
            return output.WriteAnswers(partOneAnswer, "No answer found.");
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static List<int> SwapsToTry(Instruction[] instructions, out int? answer)
    {
        answer = null;
        List<int> options = new();
        bool jumpCouldSwap = true;
        for (int i = instructions.Length - 1; i >= 0; i--)
        {
            Instruction current = instructions[i];
            if (!current.InTheLoop)
            {
                jumpCouldSwap = true;
                continue;
            }
            if (current.Operation == 'n')
            {
                int target = i + current.Argument;
                if (target < 0 || target > instructions.Length)
                {
                    continue;
                }
                if (target == instructions.Length)
                {
                    answer = i;
                    return options;
                }
                if (!instructions[target].InTheLoop)
                {
                    options.Add(i);
                }
            }
            else if (current.Operation == 'j')
            {
                if (!jumpCouldSwap)
                {
                    continue;
                }
                jumpCouldSwap = false;
                options.Add(i);
            }
        }
        return options;
    }

    private static int? PartTwoAnswer(List<int> possibleSwaps, Instruction[] instructions)
    {
        foreach (int option in possibleSwaps)
        {
            instructions[option].Swap();
            if (TryRunProgram(instructions, out int accumulator))
            {
                return accumulator;
            }
            instructions[option].Swap();
        }
        return null;
    }

    private static bool TryRunProgram(Instruction[] instructions, out int accumulator)
    {
        int programCounter = 0;
        accumulator = 0;
        HashSet<int> performedInstructions = new();
        while (0 <= programCounter && programCounter <= instructions.Length)
        {
            if (programCounter == instructions.Length)
            {
                return true;
            }
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
        return false;
    }

    private class Instruction
    {
        public char Operation { get; private set; }
        public int Argument { get; private set; }
        public bool InTheLoop { get; set; }

        public Instruction(char operation, int argument)
        {
            Operation = operation;
            Argument = argument;
        }

        public void Swap()
        {
            if (Operation == 'n')
            {
                Operation = 'j';
            }
            else if (Operation == 'j')
            {
                Operation = 'n';
            }
        }
    }
}
