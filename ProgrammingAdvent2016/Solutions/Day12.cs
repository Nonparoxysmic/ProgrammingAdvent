// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day12 : Day
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

            int[] registers = new int[4];
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (inputLines[i].Length < 5) continue;
                string[] terms = inputLines[i].Split();
                if (terms.Length < 2) continue;
                switch (terms[0])
                {
                    case "cpy":
                        if (terms.Length < 3) continue;
                        if (int.TryParse(terms[1], out int value))
                        {
                            if (!"abcd".Contains(terms[2][0].ToString())) continue;
                            registers["abcd".IndexOf(terms[2][0])] = value;
                        }
                        else
                        {
                            if (!"abcd".Contains(terms[1][0].ToString())) continue;
                            if (!"abcd".Contains(terms[2][0].ToString())) continue;
                            registers["abcd".IndexOf(terms[2][0])] 
                                = registers["abcd".IndexOf(terms[1][0])];
                        }
                        break;
                    case "inc":
                        if (!"abcd".Contains(terms[1][0].ToString())) continue;
                        registers["abcd".IndexOf(terms[1][0])]++;
                        break;
                    case "dec":
                        if (!"abcd".Contains(terms[1][0].ToString())) continue;
                        registers["abcd".IndexOf(terms[1][0])]--;
                        break;
                    case "jnz":
                        if (terms.Length < 3) continue;
                        if (int.TryParse(terms[1], out int doJump) && doJump != 0)
                        {
                            if (int.TryParse(terms[2], out int jump))
                            {
                                i += jump - 1;
                            }
                        }
                        else if ("abcd".Contains(terms[1][0].ToString()))
                        {
                            if (registers["abcd".IndexOf(terms[1][0])] 
                                != 0 && int.TryParse(terms[2], out int jump))
                            {
                                i += jump - 1;
                            }
                        }
                        break;
                }
            }
            solution.WriteSolution(1, registers[0], stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
