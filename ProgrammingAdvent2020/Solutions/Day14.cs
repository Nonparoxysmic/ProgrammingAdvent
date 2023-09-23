// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day14 : Day
{
    private static readonly string MASK = "(mask = (?<bitmask>[01X]{36}))";
    private static readonly string WRITE = "(mem\\[(?<address>[0-9]{1,11})] = (?<data>[0-9]{1,11}))";

    private static readonly Regex _validLine = new($"^({MASK}|{WRITE})$");

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

        Instruction[] instructions = ReadInput(input);
        Program program = new(instructions);
        program.Execute();
        ulong partOneAnswer = program.SumMemory();

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static Instruction[] ReadInput(string[] input)
    {
        Instruction[] instructions = new Instruction[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            Match match = _validLine.Match(input[i]);
            if (input[i][1] == 'a')
            {
                instructions[i] = new(match.Groups["bitmask"].Value);
                continue;
            }
            ulong address = ulong.Parse(match.Groups["address"].Value);
            ulong dataValue = ulong.Parse(match.Groups["data"].Value);
            instructions[i] = new(address, dataValue);
        }
        return instructions;
    }

    private class Instruction
    {
        public bool SetBitmask { get; private set; }
        public ulong MaskOnes { get; private set; }
        public ulong MaskZeroes { get; private set; }
        public ulong Address { get; private set; }
        public ulong DataValue { get; private set; }

        public Instruction(string bitmask)
        {
            SetBitmask = true;
            MaskOnes = 0;
            MaskZeroes = ~0UL;
            for (int i = 0; i < bitmask.Length; i++)
            {
                char c = bitmask[^(i + 1)];
                if (c == '1')
                {
                    MaskOnes |= (1UL << i);
                }
                if (c == '0')
                {
                    MaskZeroes &= ~(1UL << i);
                }
            }
        }

        public Instruction(ulong address, ulong dataValue)
        {
            Address = address;
            DataValue = dataValue;
        }
    }

    private class Program
    {
        private readonly Instruction[] _instructions;
        private readonly Dictionary<ulong, ulong> _memory = new();

        private ulong _maskOnes = 0;
        private ulong _maskZeroes = ~0UL;

        public Program(Instruction[] instructions)
        {
            _instructions = instructions;
        }

        public void Execute()
        {
            foreach (Instruction instruction in _instructions)
            {
                if (instruction.SetBitmask)
                {
                    _maskOnes = instruction.MaskOnes;
                    _maskZeroes = instruction.MaskZeroes;
                    continue;
                }
                ulong maskedData = (instruction.DataValue | _maskOnes) & _maskZeroes;
                _memory[instruction.Address] = maskedData;
            }
        }

        public ulong SumMemory()
        {
            ulong sum = 0;
            foreach (ulong data in _memory.Values)
            {
                sum += data;
            }
            return sum;
        }
    }
}
