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

            int partOneSolution = RunProgram(inputLines, new int[4]);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            int partTwoSolution = RunProgram(inputLines, new int[] { 0, 0, 1, 0 });
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int RunProgram(string[] inputLines, int[] registers)
        {
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
                            if (97 <= terms[2][0] && terms[2][0] <= 100)
                            {
                                registers[terms[2][0] - 97] = value;
                            }
                        }
                        else
                        {
                            if (97 <= terms[1][0] && terms[1][0] <= 100
                                && 97 <= terms[2][0] && terms[2][0] <= 100)
                            {
                                registers[terms[2][0] - 97] = registers[terms[1][0] - 97];
                            }
                        }
                        break;
                    case "inc":
                        if (97 <= terms[1][0] && terms[1][0] <= 100)
                        {
                            registers[terms[1][0] - 97]++;
                        }
                        break;
                    case "dec":
                        if (97 <= terms[1][0] && terms[1][0] <= 100)
                        {
                            registers[terms[1][0] - 97]--;
                        }
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
                        else if (97 <= terms[1][0] && terms[1][0] <= 100)
                        {
                            if (registers[terms[1][0] - 97]
                                != 0 && int.TryParse(terms[2], out int jump))
                            {
                                i += jump - 1;
                            }
                        }
                        break;
                }
            }
            return registers[0];
        }
    }
}
