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

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        // Part One
        Permutatable<int> settingPermutations = new(new int[] { 0, 1, 2, 3, 4 });
        int highestSignal = int.MinValue;
        foreach (int[] permutation in settingPermutations)
        {
            if (!RunProgramSeries(inputLines[0], permutation, out int result, out string error))
            {
                return output.WriteError($"Part One: A program encountered an error. {error}");
            }
            highestSignal = Math.Max(highestSignal, result);
        }
        // Part Two
        settingPermutations = new(new int[] { 5, 6, 7, 8, 9 });
        int highestFeedbackSignal = int.MinValue;
        foreach (int[] permutation in settingPermutations)
        {
            if (!RunFeedbackSeries(inputLines[0], permutation, out int result, out string error))
            {
                return output.WriteError($"Part Two: A program encountered an error. {error}");
            }
            highestFeedbackSignal = Math.Max(highestFeedbackSignal, result);
        }

        return output.WriteAnswers(highestSignal, highestFeedbackSignal);
    }

    private static bool RunProgramSeries(string intcode, int[] settings, out int result, out string error)
    {
        result = 0;
        error = string.Empty;
        Day07Program[] programs = new Day07Program[settings.Length];
        for (int i = 0; i < programs.Length; i++)
        {
            programs[i] = new(intcode);
            programs[i].EnqueueInput(settings[i]);
        }
        programs[0].EnqueueInput(0);
        for (int i = 0; i < programs.Length; i++)
        {
            while (programs[i].Tick()) { }
            if (programs[i].Status == Day07Program.ProgramStatus.Error)
            {
                error = programs[i].Error;
                return false;
            }
            if (i == programs.Length - 1)
            {
                if (programs[i].Status == Day07Program.ProgramStatus.Halted)
                {
                    if (programs[i].OutputAvailable())
                    {
                        result = programs[i].DequeueOutput();
                        return true;
                    }
                    else
                    {
                        error = "Final program halted without output.";
                        return false;
                    }
                }
                else if (programs[i].OutputAvailable())
                {
                    result = programs[i].DequeueOutput();
                    return true;
                }
                else
                {
                    error = "Final program failed to complete.";
                    return false;
                }
            }
            else if (programs[i].OutputAvailable())
            {
                programs[i + 1].EnqueueInput(programs[i].DequeueOutput());
            }
        }
        error = "Unknown error.";
        return false;
    }

    private static bool RunFeedbackSeries(string intcode, int[] settings, out int result, out string error)
    {
        result = 0;
        error = string.Empty;
        Day07Program[] programs = new Day07Program[settings.Length];
        for (int i = 0; i < programs.Length; i++)
        {
            programs[i] = new(intcode);
            programs[i].EnqueueInput(settings[i]);
        }
        programs[0].EnqueueInput(0);
        int timeout = 0;
        while (timeout++ < 1_000)
        {
            for (int i = 0; i < programs.Length; i++)
            {
                while (programs[i].Tick()) { }
                if (programs[i].Status == Day07Program.ProgramStatus.Error)
                {
                    error = programs[i].Error;
                    return false;
                }
                if (i == programs.Length - 1)
                {
                    if (programs[i].Status == Day07Program.ProgramStatus.Halted)
                    {
                        if (programs[i].OutputAvailable())
                        {
                            result = programs[i].DequeueOutput();
                            return true;
                        }
                        else
                        {
                            error = "Final program halted without output.";
                            return false;
                        }
                    }
                    else if (programs[i].OutputAvailable())
                    {
                        programs[0].EnqueueInput(programs[i].DequeueOutput());
                    }
                }
                else if (programs[i].OutputAvailable())
                {
                    programs[i + 1].EnqueueInput(programs[i].DequeueOutput());
                }
            }
        }
        error = "Feedback loop ran too long.";
        return false;
    }

    private class Day07Program
    {
        private readonly string[] _program;
        private int _instructionPointer;
        private readonly Queue<int> _inputs = new();
        private readonly Queue<int> _outputs = new();
        private int _ticks;

        public ProgramStatus Status { get; private set; } = ProgramStatus.Running;
        public string Error { get; private set; } = string.Empty;

        public Day07Program(string intcode)
        {
            _program = intcode.Split(',');
        }

        public void EnqueueInput(int input)
        {
            _inputs.Enqueue(input);
            if (Status == ProgramStatus.Waiting)
            {
                Status = ProgramStatus.Running;
            }
        }

        public bool OutputAvailable()
        {
            return _outputs.Count > 0;
        }

        public int DequeueOutput()
        {
            return _outputs.Dequeue();
        }

        public bool Tick()
        {
            if (_ticks++ > 10_000)
            {
                Status = ProgramStatus.Error;
                Error = "Program ran too long.";
                return false;
            }
            if (Status == ProgramStatus.Error || 
                Status == ProgramStatus.Waiting ||
                Status == ProgramStatus.Halted)
            {
                return false;
            }
            if (_instructionPointer < 0 || _instructionPointer >= _program.Length)
            {
                Status = ProgramStatus.Error;
                Error = "Instruction pointer went out of bounds.";
                return false;
            }
            if (_program[_instructionPointer][0] == '-')
            {
                Status = ProgramStatus.Error;
                Error = $"Encountered a negative instruction code at position {_instructionPointer}.";
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
            if (opcode == 99)
            {
                Status = ProgramStatus.Halted;
                return true;
            }
            if (opcode < 1 || opcode > 8)
            {
                Status = ProgramStatus.Error;
                Error = $"Invalid opcode {opcode} at position {_instructionPointer}.";
                return false;
            }
            if (opcode == 3 && _inputs.Count == 0)
            {
                Status = ProgramStatus.Waiting;
                return false;
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
                Status = ProgramStatus.Error;
                Error = $"Instruction with insufficent parameters at position {_instructionPointer}.";
                return false;
            }
            int[] parameterPositions = new int[stepSize - 1];
            for (int i = 0; i < parameterPositions.Length; i++)
            {
                if (parameterModes[i] == 0)
                {
                    parameterPositions[i] = int.Parse(_program[_instructionPointer + 1 + i]);
                    if (parameterPositions[i] < 0 || parameterPositions[i] >= _program.Length)
                    {
                        Status = ProgramStatus.Error;
                        Error = $"Parameter at position {_instructionPointer + 1 + i} points out of bounds.";
                        return false;
                    }
                }
                else
                {
                    parameterPositions[i] = _instructionPointer + 1 + i;
                }
            }
            DoInstruction(opcode, parameterPositions);
            _instructionPointer += stepSize;
            return true;
        }

        private void DoInstruction(int opcode, int[] parameterPositions)
        {
            int A = int.Parse(_program[parameterPositions[0]]);
            int B = parameterPositions.Length > 1 ? int.Parse(_program[parameterPositions[1]]) : 0;
            switch (opcode)
            {
                case 1:
                    _program[parameterPositions[2]] = (A + B).ToString();
                    break;
                case 2:
                    _program[parameterPositions[2]] = (A * B).ToString();
                    break;
                case 3:
                    _program[parameterPositions[0]] = _inputs.Dequeue().ToString();
                    break;
                case 4:
                    _outputs.Enqueue(A);
                    break;
                case 5:
                    if (A != 0)
                    {
                        _instructionPointer = B - 3;
                    }
                    break;
                case 6:
                    if (A == 0)
                    {
                        _instructionPointer = B - 3;
                    }
                    break;
                case 7:
                    _program[parameterPositions[2]] = A < B ? "1" : "0";
                    break;
                case 8:
                    _program[parameterPositions[2]] = A == B ? "1" : "0";
                    break;
            }
        }

        public enum ProgramStatus
        {
            Error = -1,
            Waiting,
            Running,
            Halted
        }
    }
}
