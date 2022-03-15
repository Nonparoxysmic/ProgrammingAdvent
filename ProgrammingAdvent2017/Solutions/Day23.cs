// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day23 : Day
    {
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
            List<Day23Instruction> instructionList = new List<Day23Instruction>();
            foreach (string line in inputLines)
            {
                if (!Regex.IsMatch(line, @"^set [a-h] ([a-h]|-?\d+)$") &&
                    !Regex.IsMatch(line, @"^sub [a-h] ([a-h]|-?\d+)$") &&
                    !Regex.IsMatch(line, @"^mul [a-h] ([a-h]|-?\d+)$") &&
                    !Regex.IsMatch(line, @"^jnz ([a-h]|-?\d+) ([a-h]|-?\d+)$"))
                {
                    output.WriteError($"Invalid line in input: \"{line}\"", sw);
                    return output;
                }
                string[] terms = line.Split();
                Day23Parameter parameter1, parameter2;
                parameter1 = int.TryParse(terms[1], out int value1)
                    ? new Day23Parameter(value1)
                    : new Day23Parameter(terms[1][0]);
                parameter2 = int.TryParse(terms[2], out int value2)
                    ? new Day23Parameter(value2)
                    : new Day23Parameter(terms[2][0]);
                instructionList.Add(new Day23Instruction(terms[0], parameter1, parameter2));
            }
            Day23Instruction[] instructions = instructionList.ToArray();

            Day23Program program = new Day23Program(instructions);
            long partOneAnswer = program.Run();

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private class Day23Program
        {
            private readonly Day23Instruction[] instructions;
            private readonly Dictionary<char, long> registers = new Dictionary<char, long>();
            private long currentInstruction = 0;

            public Day23Program(Day23Instruction[] instructions)
            {
                this.instructions = instructions;
                for (char c = 'a'; c <= 'h'; c++)
                {
                    registers.Add(c, 0);
                }
            }

            private long GetValue(Day23Parameter parameter)
            {
                return parameter.IsRegister
                    ? registers[parameter.Register]
                    : parameter.Value;
            }

            internal long Run()
            {
                int partOneAnswer = 0;
                while (0 <= currentInstruction && currentInstruction < instructions.Length)
                {
                    Day23Instruction current = instructions[currentInstruction];
                    switch (current.Command)
                    {
                        case "set":
                            registers[current.Parameter1.Register] = GetValue(current.Parameter2);
                            break;
                        case "sub":
                            registers[current.Parameter1.Register] -= GetValue(current.Parameter2);
                            break;
                        case "mul":
                            registers[current.Parameter1.Register] *= GetValue(current.Parameter2);
                            partOneAnswer++;
                            break;
                        case "jnz":
                            if (GetValue(current.Parameter1) != 0)
                            {
                                currentInstruction += GetValue(current.Parameter2) - 1;
                            }
                            break;
                        default:
                            break;
                    }
                    currentInstruction++;
                }
                return partOneAnswer;
            }
        }

        private class Day23Instruction
        {
            internal string Command { get; set; }
            internal Day23Parameter Parameter1 { get; set; }
            internal Day23Parameter Parameter2 { get; set; }

            public Day23Instruction(string command,
                Day23Parameter parameter1, Day23Parameter parameter2)
            {
                Command = command;
                Parameter1 = parameter1;
                Parameter2 = parameter2;
            }

            public override string ToString()
            {
                return Command + " " + Parameter1 + " " + Parameter2;
            }
        }

        private class Day23Parameter
        {
            internal bool IsRegister { get; set; }
            internal char Register { get; set; }
            internal int Value { get; set; }

            public Day23Parameter(char register)
            {
                IsRegister = true;
                Register = register;
            }

            public Day23Parameter(int value)
            {
                IsRegister = false;
                Value = value;
            }

            public override string ToString()
            {
                return IsRegister
                    ? Register.ToString()
                    : Value.ToString();
            }
        }
    }
}
