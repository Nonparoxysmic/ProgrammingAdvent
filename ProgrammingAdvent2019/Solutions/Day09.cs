// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day09 : Day
{
    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (!ValidateIntcodeInput(inputLines, out errorMessage))
        {
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        // Part One
        Day09Program program = new(inputLines[0]);
        program.EnqueueInput(1);
        while (program.Tick()) { }
        if (program.Status == Day09Program.ProgramStatus.Error)
        {
            return output.WriteError("Part One: " + program.Error);
        }
        if (program.Status == Day09Program.ProgramStatus.Waiting)
        {
            return output.WriteError("Part One: Program is stuck waiting for input.");
        }
        if (program.OutputCount == 0)
        {
            return output.WriteError("Part One: Program produced no output.");
        }
        if (program.OutputCount > 1)
        {
            List<long> codes = new();
            while (program.OutputCount > 0)
            {
                codes.Add(program.DequeueOutput());
            }
            return output.WriteError("Part One: Program has output error codes: " + string.Join(", ", codes));
        }
        long partOneAnswer = program.DequeueOutput();

        // Part Two
        program = new(inputLines[0]);
        program.EnqueueInput(2);
        while (program.Tick()) { }
        if (program.Status == Day09Program.ProgramStatus.Error)
        {
            return output.WriteError("Part Two: " + program.Error);
        }
        if (program.Status == Day09Program.ProgramStatus.Waiting)
        {
            return output.WriteError("Part Two: Program is stuck waiting for input.");
        }
        if (program.OutputCount == 0)
        {
            return output.WriteError("Part Two: Program produced no output.");
        }
        if (program.OutputCount > 1)
        {
            List<long> codes = new();
            while (program.OutputCount > 0)
            {
                codes.Add(program.DequeueOutput());
            }
            return output.WriteError("Part Two: Program has output error codes: " + string.Join(", ", codes));
        }
        long partTwoAnswer = program.DequeueOutput();

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    public class Day09Program
    {
        private readonly string[] _program;
        private long _instructionPointer;
        private long _relativeBase;
        private readonly Queue<long> _inputs = new();
        private readonly Queue<long> _outputs = new();
        private int _ticks;
        private readonly Dictionary<long, long> _memory = new();

        public ProgramStatus Status { get; private set; } = ProgramStatus.Running;
        public string Error { get; private set; } = string.Empty;

        public Day09Program(string intcode)
        {
            _program = intcode.Split(',');
        }

        public void EnqueueInput(long input)
        {
            _inputs.Enqueue(input);
            if (Status == ProgramStatus.Waiting)
            {
                Status = ProgramStatus.Running;
            }
        }

        public int OutputCount => _outputs.Count;

        public long DequeueOutput()
        {
            return _outputs.Dequeue();
        }

        private long ReadMemory(long position)
        {
            if (position < 0 || position >= _program.Length)
            {
                if (_memory.ContainsKey(position))
                {
                    return _memory[position];
                }
                return 0;
            }
            return long.Parse(_program[position]);
        }

        private void WriteMemory(long position, long value)
        {
            if (position < 0 || position >= _program.Length)
            {
                _memory[position] = value;
                return;
            }
            _program[position] = value.ToString();
        }

        public bool Tick()
        {
            if (_ticks++ > 2_000_000)
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
            long opcode;
            int[] parameterModes = new int[3];
            if (_program[_instructionPointer].Length <= 2)
            {
                opcode = ReadMemory(_instructionPointer);
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
            if (opcode < 1 || opcode > 9)
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
                9 => 2,
                _ => 0
            };
            if (_instructionPointer > _program.Length - stepSize)
            {
                Status = ProgramStatus.Error;
                Error = $"Instruction with insufficent parameters at position {_instructionPointer}.";
                return false;
            }
            long[] parameterPositions = new long[stepSize - 1];
            for (int i = 0; i < parameterPositions.Length; i++)
            {
                if (parameterModes[i] == 0 || parameterModes[i] == 2)
                {
                    parameterPositions[i] = ReadMemory(_instructionPointer + 1 + i);
                    if (parameterModes[i] == 2)
                    {
                        parameterPositions[i] += _relativeBase;
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

        private void DoInstruction(long opcode, long[] parameterPositions)
        {
            long A = ReadMemory(parameterPositions[0]);
            long B = parameterPositions.Length > 1 ? ReadMemory(parameterPositions[1]) : 0;
            switch (opcode)
            {
                case 1:
                    WriteMemory(parameterPositions[2], A + B);
                    break;
                case 2:
                    WriteMemory(parameterPositions[2], A * B);
                    break;
                case 3:
                    WriteMemory(parameterPositions[0], _inputs.Dequeue());
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
                    WriteMemory(parameterPositions[2], A < B ? 1 : 0);
                    break;
                case 8:
                    WriteMemory(parameterPositions[2], A == B ? 1 : 0);
                    break;
                case 9:
                    _relativeBase += A;
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
