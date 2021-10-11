// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            var instructions = new List<Instruction>();
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
                        instructions.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "inc" && terms.Length == 2)
                {
                    if ("abcd".Contains(terms[1]))
                    {
                        instructions.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "dec" && terms.Length == 2)
                {
                    if ("abcd".Contains(terms[1]))
                    {
                        instructions.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "jnz" && terms.Length == 3)
                {
                    if ((int.TryParse(terms[1], out int _) || "abcd".Contains(terms[1]))
                        && (int.TryParse(terms[2], out int _) || "abcd".Contains(terms[2])))
                    {
                        instructions.Add(new Instruction(terms));
                        continue;
                    }
                }
                else if (terms[0] == "tgl" && terms.Length == 2)
                {
                    if (int.TryParse(terms[1], out int _) || "abcd".Contains(terms[1]))
                    {
                        instructions.Add(new Instruction(terms));
                        continue;
                    }
                }
                solution.WriteSolution(1, "ERROR: Invalid line in input: \"" + line + "\"", 0);
                return solution;
            }

            

            stopwatch.Reset();
            return solution;
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
