// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day07 : Day
{
    private static readonly Regex _validCharacters = new("^[0-9,-]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!_validCharacters.IsMatch(inputLines[0]))
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains invalid characters.";
            return false;
        }
        string[] terms = inputLines[0].Split(',');
        if (terms.Length < 5)
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
        Permutatable<int> settingPermutations = new(new int[] { 0, 1, 2, 3, 4 });
        int highestSignal = int.MinValue;
        foreach (int[] permutation in settingPermutations)
        {
            if (!RunProgramSeries(inputLines[0], permutation, out int result))
            {
                return output.WriteError("");
            }
            highestSignal = Math.Max(highestSignal, result);
        }
        return output.WriteAnswers(highestSignal, null);
    }

    private static bool RunProgramSeries(string intcode, int[] sequence, out int output)
    {
        int signal = 0;
        foreach (int setting in sequence)
        {
            if (!RunProgram(intcode, setting, signal, out signal))
            {
                output = signal;
                return false;
            }
        }
        output = signal;
        return true;
    }

    private static bool RunProgram(string intcode, int input1, int input2, out int output)
    {
        output = 0;
        int input = input1;
        string[] program = intcode.Split(',');
        int instructionPointer = 0;
        while (0 <= instructionPointer && instructionPointer < program.Length)
        {
            if (program[instructionPointer][0] == '-')
            {
                output = -45;
                return false;
            }
            int opcode;
            int[] parameterModes = new int[3];
            if (program[instructionPointer].Length <= 2)
            {
                opcode = int.Parse(program[instructionPointer]);
            }
            else
            {
                string term = program[instructionPointer];
                opcode = 10 * (term[^2] - '0') + term[^1] - '0';
                for (int i = 0; i < 3 && i < term.Length - 2; i++)
                {
                    parameterModes[i] = term[term.Length - 3 - i] - '0';
                }
            }
            int stepSize = opcode switch
            {
                1 => 4,
                2 => 4,
                3 => 2,
                4 => 2,
                5 => 3,
                6 => 3,
                7 => 4,
                8 => 4,
                _ => 0
            };
            if (instructionPointer > program.Length - stepSize)
            {
                output = int.MinValue;
                return false;
            }
            int[] parameterPositions = new int[stepSize - 1];
            for (int i = 0; i < parameterPositions.Length; i++)
            {
                if (parameterModes[i] == 0)
                {
                    parameterPositions[i] = int.Parse(program[instructionPointer + 1 + i]);
                    if (parameterPositions[i] < 0 || parameterPositions[i] >= program.Length)
                    {
                        output = -8;
                        return false;
                    }
                }
                else
                {
                    parameterPositions[i] = instructionPointer + 1 + i;
                }
            }
            if (opcode == 99)
            {
                output = -99;
                return false;
            }
            if (opcode < 1 || opcode > 8)
            {
                output = -400;
                return false;
            }
            if (!DoInstruction(program, ref instructionPointer, opcode, parameterPositions, input, out int result))
            {
                output = result;
                return false;
            }
            if (opcode == 4)
            {
                output = result;
                return true;
            }
            if (opcode == 3)
            {
                input = input2;
            }
            instructionPointer += stepSize;
        }
        output = -1;
        return false;
    }

    private static bool DoInstruction(string[] program, ref int instructionPointer, int opcode, int[] parameterPositions, int input, out int result)
    {
        result = 0;
        int A = int.Parse(program[parameterPositions[0]]);
        int B = parameterPositions.Length > 1 ? int.Parse(program[parameterPositions[1]]) : 0;
        switch (opcode)
        {
            case 1:
                program[parameterPositions[2]] = (A + B).ToString();
                return true;
            case 2:
                program[parameterPositions[2]] = (A * B).ToString();
                return true;
            case 3:
                program[parameterPositions[0]] = input.ToString();
                return true;
            case 4:
                result = A;
                return true;
            case 5:
                if (A != 0)
                {
                    instructionPointer = B - 3;
                }
                return true;
            case 6:
                if (A == 0)
                {
                    instructionPointer = B - 3;
                }
                return true;
            case 7:
                program[parameterPositions[2]] = A < B ? "1" : "0";
                return true;
            case 8:
                program[parameterPositions[2]] = A == B ? "1" : "0";
                return true;
        }
        result = -100 - opcode;
        return false;
    }
}
