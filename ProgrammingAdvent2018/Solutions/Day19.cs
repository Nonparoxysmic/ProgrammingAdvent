// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day19 : Day
    {
        private static readonly string[] opcodes = new string[] { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr" };

        private readonly Regex instructionLine = new Regex(@"^((addr|banr|borr|eqrr|gtrr|mulr) [0-5] [0-5]|(addi|bani|bori|eqri|gtri|muli|setr) [0-5] [0-9]{1,9}|(eqir|gtir) [0-9]{1,9} [0-5]|seti [0-9]{1,9} [0-9]{1,9}) [0-5]$");
        private readonly Regex instructionPointerLine = new Regex(@"^#ip ([0-5])$");

        private int instructionPointer;
        private int instructionPointerBinding;
        private List<(int opcode, int A, int B, int C)> instructions;
        private int[] registers;

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 2)
            {
                output.WriteError("Insufficient input.", sw);
                return output;
            }

            Match instructionPointerMatch = instructionPointerLine.Match(inputLines[0]);
            if (!instructionPointerMatch.Success)
            {
                output.WriteError($"Invalid instruction pointer binding \"{inputLines[0]}\".", sw);
                return output;
            }
            instructionPointerBinding = int.Parse(instructionPointerMatch.Groups[1].Value);
            if (instructionPointerBinding != 4)
            {
                output.WriteError($"Instruction pointer binding of {instructionPointerBinding} not supported.", sw);
                return output;
            }
            instructions = new List<(int, int, int, int)>();
            for (int i = 1; i < inputLines.Length; i++)
            {
                Match instructionMatch = instructionLine.Match(inputLines[i]);
                if (!instructionMatch.Success)
                {
                    output.WriteError($"Invalid instruction \"{inputLines[i]}\".", sw);
                    return output;
                }
                string[] terms = inputLines[i].Split();
                int opcode = Array.IndexOf(opcodes, terms[0]);
                int A = int.Parse(terms[1]);
                int B = int.Parse(terms[2]);
                int C = int.Parse(terms[3]);
                instructions.Add((opcode, A, B, C));
            }

            instructionPointer = 0;
            registers = new int[6];
            while (0 <= instructionPointer && instructionPointer < instructions.Count)
            {
                PerformInstruction();
            }
            int partOneAnswer = registers[0];

            instructionPointer = 0;
            registers = new int[6];
            registers[0] = 1;
            while (0 <= instructionPointer && instructionPointer < instructions.Count)
            {
                PerformInstruction();
            }
            int partTwoAnswer = registers[0];

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private void PerformInstruction()
        {
            if (instructionPointer == 3)
            {
                // Bypass the loop.
                if (registers[2] % registers[1] == 0)
                {
                    registers[0] += registers[1];
                }
                registers[3] = registers[2] + 1;
                registers[4] = 11;
                registers[5] = 1;
                instructionPointer = 12;
                return;
            }

            registers[instructionPointerBinding] = instructionPointer;
            (int opcode, int A, int B, int C) = instructions[instructionPointer];
            switch (opcode)
            {
                case 0:
                    // addr
                    registers[C] = registers[A] + registers[B];
                    break;
                case 1:
                    // addi
                    registers[C] = registers[A] + B;
                    break;
                case 2:
                    // mulr
                    registers[C] = registers[A] * registers[B];
                    break;
                case 3:
                    // muli
                    registers[C] = registers[A] * B;
                    break;
                case 4:
                    // banr
                    registers[C] = registers[A] & registers[B];
                    break;
                case 5:
                    // bani
                    registers[C] = registers[A] & B;
                    break;
                case 6:
                    // borr
                    registers[C] = registers[A] | registers[B];
                    break;
                case 7:
                    // bori
                    registers[C] = registers[A] | B;
                    break;
                case 8:
                    // setr
                    registers[C] = registers[A];
                    break;
                case 9:
                    // seti
                    registers[C] = A;
                    break;
                case 10:
                    // gtir
                    registers[C] = A > registers[B] ? 1 : 0;
                    break;
                case 11:
                    // gtri
                    registers[C] = registers[A] > B ? 1 : 0;
                    break;
                case 12:
                    // gtrr
                    registers[C] = registers[A] > registers[B] ? 1 : 0;
                    break;
                case 13:
                    // eqir
                    registers[C] = A == registers[B] ? 1 : 0;
                    break;
                case 14:
                    // eqri
                    registers[C] = registers[A] == B ? 1 : 0;
                    break;
                case 15:
                    // eqrr
                    registers[C] = registers[A] == registers[B] ? 1 : 0;
                    break;
            }
            instructionPointer = registers[instructionPointerBinding];
            instructionPointer++;
        }
    }
}
