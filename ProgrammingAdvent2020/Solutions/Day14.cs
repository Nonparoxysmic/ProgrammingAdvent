// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Numerics;
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

        bool partOneOnly = exampleModifier is not null;
        Instruction[] instructions = ReadInput(input, partOneOnly);

        Program program = new(instructions);
        program.Execute();
        ulong partOneAnswer = program.SumMemory();
        if (partOneOnly)
        {
            return output.WriteAnswers(partOneAnswer, "Part One only example");
        }

        Program2 program2 = new(instructions);
        program2.Execute();
        ulong partTwoAnswer = program2.SumMemory();

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static Instruction[] ReadInput(string[] input, bool partOneOnly)
    {
        Instruction[] instructions = new Instruction[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            Match match = _validLine.Match(input[i]);
            if (input[i][1] == 'a')
            {
                instructions[i] = new(match.Groups["bitmask"].Value, partOneOnly);
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
        public ulong NonfloatingBits { get; private set; }
        public ulong[] FloatingBitValues { get; private set; }

        public Instruction(string bitmask, bool partOneOnly)
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

            if (partOneOnly)
            {
                FloatingBitValues = Array.Empty<ulong>();
                return;
            }

            ulong floatingBits = ~MaskOnes & MaskZeroes & 0xFFFFFFFFF;
            NonfloatingBits = ~floatingBits;

            List<int> floatingIndices = new();
            for (int i = 0; i < 64; i++)
            {
                if (((floatingBits >> i) & 1) == 1)
                {
                    floatingIndices.Add(i);
                }
            }
            int possibilityCount = MathS.PowInt(2, BitOperations.PopCount(floatingBits));
            List<ulong> floatingBitValues = new(possibilityCount);
            for (ulong i = 0; i < (ulong)possibilityCount; i++)
            {
                floatingBitValues.Add(PositionBits(i, floatingIndices));
            }
            FloatingBitValues = floatingBitValues.ToArray();
        }

        public Instruction(ulong address, ulong dataValue)
        {
            Address = address;
            DataValue = dataValue;
            FloatingBitValues = Array.Empty<ulong>();
        }

        private static ulong PositionBits(ulong value, List<int> indices)
        {
            ulong result = 0;
            for (int i = 0; i < indices.Count; i++)
            {
                result |= ((value >> i) & 1) << indices[i];
            }
            return result;
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

    private class Program2
    {
        private readonly Instruction[] _instructions;
        private readonly Dictionary<ulong, ulong> _memory = new();

        private ulong _maskOnes = 0;
        private ulong _nonfloatingBits = ~0UL;
        private ulong[] _floatingBitValues;

        public Program2(Instruction[] instructions)
        {
            _instructions = instructions;
            _floatingBitValues = Array.Empty<ulong>();
        }

        public void Execute()
        {
            foreach (Instruction instruction in _instructions)
            {
                if (instruction.SetBitmask)
                {
                    _maskOnes = instruction.MaskOnes;
                    _nonfloatingBits = instruction.NonfloatingBits;
                    _floatingBitValues = instruction.FloatingBitValues;
                    continue;
                }
                foreach (ulong floatingValue in _floatingBitValues)
                {
                    ulong maskedAddress = (instruction.Address & _nonfloatingBits) 
                        | floatingValue | _maskOnes;
                    _memory[maskedAddress] = instruction.DataValue;
                }
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
