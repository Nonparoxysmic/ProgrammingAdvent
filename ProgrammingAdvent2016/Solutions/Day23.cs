// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day23 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            var programLines = new List<Instruction>();
            foreach (string line in inputLines)
            {
                if (line.Length == 0)
                {
                    solution.WriteSolution(1, "ERROR: Empty line in input.", 0);
                    return solution;
                }
                string[] terms = line.Split();
                if (terms[0] == "cpy" && terms.Length == 3)
                {
                    if ((int.TryParse(terms[1], out int _) || "abcd".Contains(terms[1]))
                        && "abcd".Contains(terms[2]))
                    {
                        programLines.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "inc" && terms.Length == 2)
                {
                    if ("abcd".Contains(terms[1]))
                    {
                        programLines.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "dec" && terms.Length == 2)
                {
                    if ("abcd".Contains(terms[1]))
                    {
                        programLines.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "jnz" && terms.Length == 3)
                {
                    if ((int.TryParse(terms[1], out int _) || "abcd".Contains(terms[1]))
                        && (int.TryParse(terms[2], out int _) || "abcd".Contains(terms[2])))
                    {
                        programLines.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "tgl" && terms.Length == 2)
                {
                    if (int.TryParse(terms[1], out int _) || "abcd".Contains(terms[1]))
                    {
                        programLines.Add(new Instruction(terms));
                        continue;
                    }
                }
                solution.WriteSolution(1, "ERROR: Invalid line in input: \"" + line + "\"", 0);
                return solution;
            }

            long partOneSolution = RunProgram(programLines.ToArray(), new long[] { 7, 0, 0, 0 });
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        static long RunProgram(Instruction[] instructions, long[] registers)
        {
            var registerArgument = new Regex(@"^[abcd]$");
            for (int i = 0; i < instructions.Length; i++)
            {
                switch (instructions[i].type)
                {
                    case InstructionType.cpy:
                        if (!registerArgument.IsMatch(instructions[i].arguments[1]))
                        {
                            break;
                        }
                        int cpyTargetRegister = "abcd".IndexOf(instructions[i].arguments[1][0]);
                        if (int.TryParse(instructions[i].arguments[0], out int cpyValue))
                        {
                            registers[cpyTargetRegister] = cpyValue;
                        }
                        else
                        {
                            registers[cpyTargetRegister] = registers["abcd".IndexOf(instructions[i].arguments[0][0])];
                        }
                        break;
                    case InstructionType.inc:
                        if (!registerArgument.IsMatch(instructions[i].arguments[0]))
                        {
                            break;
                        }
                        int incTargetRegister = "abcd".IndexOf(instructions[i].arguments[0][0]);
                        registers[incTargetRegister]++;
                        break;
                    case InstructionType.dec:
                        if (!registerArgument.IsMatch(instructions[i].arguments[0]))
                        {
                            break;
                        }
                        int decTargetRegister = "abcd".IndexOf(instructions[i].arguments[0][0]);
                        registers[decTargetRegister]--;
                        break;
                    case InstructionType.jnz:
                        long x, y;
                        if (!long.TryParse(instructions[i].arguments[0], out x))
                        {
                            x = registers["abcd".IndexOf(instructions[i].arguments[0][0])];
                        }
                        if (x == 0)
                        {
                            break;
                        }
                        if (!long.TryParse(instructions[i].arguments[1], out y))
                        {
                            y = registers["abcd".IndexOf(instructions[i].arguments[1][0])];
                        }
                        i += (int)y - 1;
                        break;
                    case InstructionType.tgl:
                        long steps;
                        if (!long.TryParse(instructions[i].arguments[0], out steps))
                        {
                            steps = registers["abcd".IndexOf(instructions[i].arguments[0][0])];
                        }
                        if ((i + steps < 0) || (i + steps >= instructions.Length))
                        {
                            break;
                        }
                        switch (instructions[i + steps].type)
                        {
                            case InstructionType.inc:
                                instructions[i + steps].type = InstructionType.dec;
                                break;
                            case InstructionType.dec:
                            case InstructionType.tgl:
                                instructions[i + steps].type = InstructionType.inc;
                                break;
                            case InstructionType.jnz:
                                instructions[i + steps].type = InstructionType.cpy;
                                break;
                            case InstructionType.cpy:
                                instructions[i + steps].type = InstructionType.jnz;
                                break;
                        }
                        break;
                }
                if (i < 0) { i = 0; }
            }
            return registers[0];
        }
    }

    class Instruction
    {
        public InstructionType type;
        public string[] arguments;

        public Instruction(string[] instruction)
        {
            type = (InstructionType)Enum.Parse(typeof(InstructionType), instruction[0]);
            arguments = new string[instruction.Length - 1];
            for (int i = 1; i < instruction.Length; i++)
            {
                arguments[i - 1] = instruction[i];
            }
        }
    }

    enum InstructionType
    {
        cpy,
        inc,
        dec,
        jnz,
        tgl
    }
}
