// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day05 : Day
{
    private static readonly Regex _validCharacters = new("^[0-9,-]+$");

    private string[] _program = Array.Empty<string>();
    private int _instructionPointer;

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
        if (terms.Length < 3)
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

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        _program = inputLines[0].Split(',');
        if (!RunProgram(1, out int diagnosticCode1))
        {
            return output.WriteError($"Program execution failed. Error code {diagnosticCode1}.");
        }
        _program = inputLines[0].Split(',');
        if (!RunProgram(5, out int diagnosticCode5))
        {
            return output.WriteError($"Program execution failed. Error code {diagnosticCode5}.");
        }
        return output.WriteAnswers(diagnosticCode1, diagnosticCode5);
    }

    private bool RunProgram(int input, out int code)
    {
        code = 0;
        _instructionPointer = 0;
        while (_instructionPointer < _program.Length)
        {
            if (_program[_instructionPointer][0] == '-')
            {
                code = -45;
                return false;
            }
            int opcode;
            int[] parameterModes = new int[3];
            if (_program[_instructionPointer].Length <= 2)
            {
                opcode = int.Parse(_program[_instructionPointer]);
            }
            else
            {
                string term = _program[_instructionPointer];
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
            if (_instructionPointer > _program.Length - stepSize)
            {
                code = int.MinValue;
                return false;
            }
            switch (opcode)
            {
                case 99:
                    code = -99;
                    return false;
                case 1:
                case 2:
                case 7:
                case 8:
                    if (!DoMathInstruction(opcode, parameterModes, out int mathCode))
                    {
                        code = mathCode;
                        return false;
                    }
                    break;
                case 3:
                    if (!DoIOInstruction(opcode, parameterModes, out int inputCode, input))
                    {
                        code = inputCode;
                        return false;
                    }
                    break;
                case 4:
                    if (!DoIOInstruction(opcode, parameterModes, out int outputCode))
                    {
                        code = outputCode;
                        return false;
                    }
                    if ((_instructionPointer <= _program.Length - stepSize - 1
                        && _program[_instructionPointer + stepSize] == "99")
                        || input == 5)
                    {
                        code = outputCode;
                        return true;
                    }
                    else if (outputCode != 0)
                    {
                        code = -1000000 - _instructionPointer;
                        return true;
                    }
                    break;
                case 5:
                case 6:
                    if (!DoJumpInstruction(opcode, parameterModes, out int jumpCode))
                    {
                        code = jumpCode;
                        return false;
                    }
                    break;
                default:
                    code = -400;
                    return false;
            }
            _instructionPointer += stepSize;
        }
        code = -1;
        return false;
    }

    private bool DoMathInstruction(int opcode, int[] parameterModes, out int output)
    {
        try
        {
            output = 0;
            int A = int.Parse(_program[_instructionPointer + 1]);
            if (parameterModes[0] == 0)
            {
                A = int.Parse(_program[A]);
            }
            int B = int.Parse(_program[_instructionPointer + 2]);
            if (parameterModes[1] == 0)
            {
                B = int.Parse(_program[B]);
            }
            int C = int.Parse(_program[_instructionPointer + 3]);
            if (opcode == 1)
            {
                _program[C] = (A + B).ToString();
                return true;
            }
            else if (opcode == 2)
            {
                _program[C] = (A * B).ToString();
                return true;
            }
            else if (opcode == 7)
            {
                if (A < B)
                {
                    _program[C] = "1";
                }
                else
                {
                    _program[C] = "0";
                }
                return true;
            }
            else if (opcode == 8)
            {
                if (A == B)
                {
                    _program[C] = "1";
                }
                else
                {
                    _program[C] = "0";
                }
                return true;
            }
            else
            {
                output = -200 - opcode;
                return false;
            }
        }
        catch
        {
            output = -100 - opcode;
            return false;
        }
    }

    private bool DoIOInstruction(int opcode, int[] parameterModes, out int output, int input = 0)
    {
        try
        {
            output = 0;
            int A = int.Parse(_program[_instructionPointer + 1]);
            if (opcode == 3)
            {
                _program[A] = input.ToString();
                return true;
            }
            else if (opcode == 4)
            {
                if (parameterModes[0] == 0)
                {
                    A = int.Parse(_program[A]);
                    output = A;
                    return true;
                }
                else
                {
                    output = A;
                    return true;
                }
            }
            else
            {
                output = -200 - opcode;
                return false;
            }
        }
        catch
        {
            output = -100 - opcode;
            return false;
        }
    }

    private bool DoJumpInstruction(int opcode, int[] parameterModes, out int output)
    {
        try
        {
            output = 0;
            int A = int.Parse(_program[_instructionPointer + 1]);
            if (parameterModes[0] == 0)
            {
                A = int.Parse(_program[A]);
            }
            int B = int.Parse(_program[_instructionPointer + 2]);
            if (parameterModes[1] == 0)
            {
                B = int.Parse(_program[B]);
            }
            if (opcode == 5)
            {
                if (A != 0)
                {
                    _instructionPointer = B - 3;
                }
                return true;
            }
            else if (opcode == 6)
            {
                if (A == 0)
                {
                    _instructionPointer = B - 3;
                }
                return true;
            }
            else
            {
                output = -200 - opcode;
                return false;
            }
        }
        catch
        {
            output = -100 - opcode;
            return false;
        }
    }
}
