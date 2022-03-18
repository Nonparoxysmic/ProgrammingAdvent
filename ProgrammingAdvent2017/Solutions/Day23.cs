// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
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

            if (!TryOptimize(instructions, out long partTwoAnswer))
            {
                output.WriteAnswers(partOneAnswer, "ERROR: Unable to optimize program.", sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private bool TryOptimize(Day23Instruction[] instructions, out long result)
        {
            if (instructions.Length != 32
                || !Regex.IsMatch(instructions[0].ToString(), @"^set b \d+$")
                || instructions[1].ToString() != "set c b"
                || instructions[2].ToString() != "jnz a 2"
                || instructions[3].ToString() != "jnz 1 5"
                || instructions[4].ToString() != "mul b 100"
                || instructions[5].ToString() != "sub b -100000"
                || instructions[6].ToString() != "set c b"
                || instructions[7].ToString() != "sub c -17000"
                || instructions[8].ToString() != "set f 1"
                || instructions[9].ToString() != "set d 2"
                || instructions[10].ToString() != "set e 2"
                || instructions[11].ToString() != "set g d"
                || instructions[12].ToString() != "mul g e"
                || instructions[13].ToString() != "sub g b"
                || instructions[14].ToString() != "jnz g 2"
                || instructions[15].ToString() != "set f 0"
                || instructions[16].ToString() != "sub e -1"
                || instructions[17].ToString() != "set g e"
                || instructions[18].ToString() != "sub g b"
                || instructions[19].ToString() != "jnz g -8"
                || instructions[20].ToString() != "sub d -1"
                || instructions[21].ToString() != "set g d"
                || instructions[22].ToString() != "sub g b"
                || instructions[23].ToString() != "jnz g -13"
                || instructions[24].ToString() != "jnz f 2"
                || instructions[25].ToString() != "sub h -1"
                || instructions[26].ToString() != "set g b"
                || instructions[27].ToString() != "sub g c"
                || instructions[28].ToString() != "jnz g 2"
                || instructions[29].ToString() != "jnz 1 3"
                || instructions[30].ToString() != "sub b -17"
                || instructions[31].ToString() != "jnz 1 -23")
            {
                result = 0;
                return false;
            }

            int inputValue = instructions[0].Parameter2.Value;
            int b = 100 * inputValue + 100000;
            result = 0;
            for (int i = 0; i <= 1000; i++)
            {
                if (!IsPrime(b))
                {
                    result++;
                }
                b += 17;
            }
            return true;
        }

        internal static bool IsPrime(int number)
        {
            if (number < 2) { return false; }
            if (number < 100)
            {
                int[] first25Primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23,
                    29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
                foreach (int prime in first25Primes)
                {
                    if (number == prime) { return true; }
                }
                return false;
            }

            if (number % 2 == 0 || number % 5 == 0) { return false; }
            int boundary = (int)Math.Sqrt(number);
            for (int i = 3; i <= boundary; i += 10)
            {
                if (number % i == 0 ||
                    number % (i + 4) == 0 ||
                    number % (i + 6) == 0 ||
                    number % (i + 8) == 0)
                {
                    return false;
                }
            }
            return true;
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
