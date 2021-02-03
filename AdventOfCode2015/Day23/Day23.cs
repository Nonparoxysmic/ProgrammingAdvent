// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;

namespace AdventOfCode2015
{
    static class Day23
    {
        public static void Solve()
        {
            string input1Path = @"Day23\Puzzle\Input1.txt";
            string[] instructions = null;
            try
            {
                instructions = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            int registerA = 0;
            int registerB = 0;
            int currentInstructionIndex = 0;
            while (currentInstructionIndex >= 0 && currentInstructionIndex < instructions.Length)
            {
                string currentInstruction = instructions[currentInstructionIndex];
                if (currentInstruction.Length < 5)
                {
                    Print.PrintErrorAndExit("Day 23: Invalid instruction \"" + currentInstruction + "\" in input.");
                }

                bool invalidInstruction = false;
                switch (currentInstruction.Substring(0, 4))
                {
                    case "hlf ":
                        if (currentInstruction[4] == 'a') registerA /= 2;
                        else if (currentInstruction[4] == 'b') registerB /= 2;
                        else invalidInstruction = true;
                        currentInstructionIndex++;
                        break;
                    case "tpl ":
                        if (currentInstruction[4] == 'a') registerA *= 3;
                        else if (currentInstruction[4] == 'b') registerB *= 3;
                        else invalidInstruction = true;
                        currentInstructionIndex++;
                        break;
                    case "inc ":
                        if (currentInstruction[4] == 'a') registerA++;
                        else if (currentInstruction[4] == 'b') registerB++;
                        else invalidInstruction = true;
                        currentInstructionIndex++;
                        break;
                    case "jmp ":
                        if (int.TryParse(currentInstruction.Substring(4), out int jmpOffset))
                        {
                            currentInstructionIndex += jmpOffset;
                        }
                        else invalidInstruction = true;
                        break;
                    case "jie ":
                        if (currentInstruction.Length < 9)
                        {
                            invalidInstruction = true;
                            break;
                        }
                        if (int.TryParse(currentInstruction.Substring(7), out int jieOffset))
                        {
                            int comparisonValue;
                            if (currentInstruction[4] == 'a') comparisonValue = registerA;
                            else if (currentInstruction[4] == 'b') comparisonValue = registerB;
                            else
                            {
                                invalidInstruction = true;
                                break;
                            }
                            if (comparisonValue % 2 == 0) currentInstructionIndex += jieOffset;
                            else currentInstructionIndex++;
                        }
                        else invalidInstruction = true;
                        break;
                    case "jio ":
                        if (currentInstruction.Length < 9)
                        {
                            invalidInstruction = true;
                            break;
                        }
                        if (int.TryParse(currentInstruction.Substring(7), out int jioOffset))
                        {
                            int comparisonValue;
                            if (currentInstruction[4] == 'a') comparisonValue = registerA;
                            else if (currentInstruction[4] == 'b') comparisonValue = registerB;
                            else
                            {
                                invalidInstruction = true;
                                break;
                            }
                            if (comparisonValue == 1) currentInstructionIndex += jioOffset;
                            else currentInstructionIndex++;
                        }
                        else invalidInstruction = true;
                        break;
                    default:
                        invalidInstruction = true;
                        break;
                }
                if (invalidInstruction) Print.PrintErrorAndExit("Day 23: Invalid instruction \"" + currentInstruction + "\" in input.");
            }

            Console.WriteLine("Day 23 Part One Answer: " + registerB);
        }
    }
}
