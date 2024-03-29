﻿// Puzzle solution by Nonparoxysmic
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
    internal class Day18 : Day
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
            foreach (string line in inputLines)
            {
                if (   !Regex.IsMatch(line, @"^snd ([a-z]|-?\d+)$")
                    && !Regex.IsMatch(line, @"^rcv ([a-z]|-?\d+)$")
                    && !Regex.IsMatch(line, @"^set [a-z] ([a-z]|-?\d+)$")
                    && !Regex.IsMatch(line, @"^add [a-z] ([a-z]|-?\d+)$")
                    && !Regex.IsMatch(line, @"^mul [a-z] ([a-z]|-?\d+)$")
                    && !Regex.IsMatch(line, @"^mod [a-z] ([a-z]|-?\d+)$")
                    && !Regex.IsMatch(line, @"^jgz ([a-z]|-?\d+) ([a-z]|-?\d+)$"))
                {
                    output.WriteError($"Invalid line in input: \"{line}\"", sw);
                    return output;
                }
            }

            Day18Program partOneProgram = new Day18Program(inputLines);
            long partOneAnswer = partOneProgram.PartOneAnswer();

            Day18Program programZero = new Day18Program(inputLines, 0);
            Day18Program programOne = new Day18Program(inputLines, 1);
            programZero.DuetProgram = programOne;
            programOne.DuetProgram = programZero;
            int partTwoAnswer = RunDuet(programZero, programOne);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int RunDuet(Day18Program programZero, Day18Program programOne)
        {
            while (!programZero.IsWaiting || !programOne.IsWaiting)
            {
                programZero.RunNextCommand();
                programOne.RunNextCommand();
                if (!programZero.IsRunning && !programOne.IsRunning) { break; }
            }
            return programOne.ValuesSent;
        }

        private class Day18Program
        {
            internal Day18Program DuetProgram { get; set; }
            internal bool IsRunning { get; set; }
            internal bool IsWaiting { get; set; }
            internal Queue<long> ValueQueue { get; }
            internal int ValuesSent { get; set; }

            readonly Day18Instruction[] instructions;
            readonly Dictionary<char, long> registers = new Dictionary<char, long>();
            long currentInstruction = 0;
            long playedSound;

            public Day18Program(string[] program, int programID = -1)
            {
                if (programID >= 0)
                {
                    registers.Add('p', programID);
                    IsRunning = true;
                    IsWaiting = false;
                    ValueQueue = new Queue<long>();
                    ValuesSent = 0;
                }
                List<Day18Instruction> instructionList = new List<Day18Instruction>();
                foreach (string line in program)
                {
                    Day18Instruction newInstruction = new Day18Instruction();
                    AddParameters(ref newInstruction, line[4..].Split());
                    switch (line[0..3])
                    {
                        case "snd":
                            if (programID < 0)
                            {
                                newInstruction.Command = Sound;
                                break;
                            }
                            newInstruction.Command = Send;
                            break;
                        case "rcv":
                            if (programID < 0)
                            {
                                newInstruction.Command = Recover;
                                break;
                            }
                            newInstruction.Command = Receive;
                            break;
                        case "set":
                            newInstruction.Command = Set;
                            break;
                        case "add":
                            newInstruction.Command = Add;
                            break;
                        case "mul":
                            newInstruction.Command = Multiply;
                            break;
                        case "mod":
                            newInstruction.Command = Modulo;
                            break;
                        case "jgz":
                            newInstruction.Command = Jump;
                            break;
                    }
                    instructionList.Add(newInstruction);
                }
                instructions = instructionList.ToArray();
            }

            private void AddParameters(ref Day18Instruction instruction, string[] parameters)
            {
                if (int.TryParse(parameters[0], out int parameter1))
                {
                    instruction.Parameter1 = new Day18Parameter(parameter1);
                }
                else
                {
                    instruction.Parameter1 = new Day18Parameter(parameters[0][0]);
                }
                if (parameters.Length == 1) { return; }
                if (int.TryParse(parameters[1], out int parameter2))
                {
                    instruction.Parameter2 = new Day18Parameter(parameter2);
                }
                else
                {
                    instruction.Parameter2 = new Day18Parameter(parameters[1][0]);
                }
            }

            private long GetValue(Day18Parameter parameter)
            {
                if (parameter.IsRegister)
                {
                    if (registers.ContainsKey(parameter.Register))
                    {
                        return registers[parameter.Register];
                    }
                    else return 0;
                }
                else
                {
                    return parameter.Value;
                }
            }

            internal long PartOneAnswer()
            {
                while (0 <= currentInstruction && currentInstruction < instructions.Length)
                {
                    if (instructions[currentInstruction].Command == Recover)
                    {
                        long x = GetValue(instructions[currentInstruction].Parameter1);
                        if (x != 0)
                        {
                            return playedSound;
                        }
                    }
                    instructions[currentInstruction].ExecuteCommand();
                    currentInstruction++;
                }
                return int.MinValue;
            }

            internal void RunNextCommand()
            {
                if (currentInstruction < 0 || currentInstruction >= instructions.Length)
                {
                    IsRunning = false;
                    return;
                }
                if (instructions[currentInstruction].Command == Receive
                    && ValueQueue.Count == 0)
                {
                    IsWaiting = true;
                    return;
                }
                IsWaiting = false;
                instructions[currentInstruction].ExecuteCommand();
                currentInstruction++;
            }

            private void Sound(Day18Parameter parameter, Day18Parameter _)
            {
                playedSound = GetValue(parameter);
            }

            private void Recover(Day18Parameter parameter, Day18Parameter _) { }

            private void Send(Day18Parameter parameter, Day18Parameter _)
            {
                DuetProgram.ValueQueue.Enqueue(GetValue(parameter));
                ValuesSent++;
            }

            private void Receive(Day18Parameter parameter, Day18Parameter _)
            {
                if (parameter.IsRegister)
                {
                    if (!registers.ContainsKey(parameter.Register))
                    {
                        registers.Add(parameter.Register, ValueQueue.Dequeue());
                    }
                    else
                    {
                        registers[parameter.Register] = ValueQueue.Dequeue();
                    }
                }
            }

            private void Set(Day18Parameter parameter1, Day18Parameter parameter2)
            {
                if (registers.ContainsKey(parameter1.Register))
                {
                    registers[parameter1.Register] = GetValue(parameter2);
                }
                else
                {
                    registers.Add(parameter1.Register, GetValue(parameter2));
                }
            }

            private void Add(Day18Parameter parameter1, Day18Parameter parameter2)
            {
                if (registers.ContainsKey(parameter1.Register))
                {
                    registers[parameter1.Register] += GetValue(parameter2);
                }
                else
                {
                    registers.Add(parameter1.Register, GetValue(parameter2));
                }
            }

            private void Multiply(Day18Parameter parameter1, Day18Parameter parameter2)
            {
                if (registers.ContainsKey(parameter1.Register))
                {
                    registers[parameter1.Register] *= GetValue(parameter2);
                }
            }

            private void Modulo(Day18Parameter parameter1, Day18Parameter parameter2)
            {
                if (registers.ContainsKey(parameter1.Register))
                {
                    registers[parameter1.Register] %= GetValue(parameter2);
                }
            }

            private void Jump(Day18Parameter parameter1, Day18Parameter parameter2)
            {
                if (GetValue(parameter1) > 0)
                {
                    currentInstruction += GetValue(parameter2) - 1;
                }
            }
        }

        private class Day18Parameter
        {
            internal bool IsRegister { get; set; }
            internal char Register { get; set; }
            internal int Value { get; set; }

            public Day18Parameter(char register)
            {
                IsRegister = true;
                Register = register;
            }

            public Day18Parameter(int value)
            {
                IsRegister = false;
                Value = value;
            }

            public override string ToString()
            {
                if (IsRegister)
                {
                    return Register.ToString();
                }
                return Value.ToString();
            }
        }

        private class Day18Instruction
        {
            internal Action<Day18Parameter, Day18Parameter> Command { get; set; }
            internal Day18Parameter Parameter1 { get; set; }
            internal Day18Parameter Parameter2 { get; set; }

            internal void ExecuteCommand()
            {
                Command(Parameter1, Parameter2);
            }
        }
    }
}
