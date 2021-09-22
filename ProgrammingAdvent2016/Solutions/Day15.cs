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
    public class Day15 : Day
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

            var validLineRegex = new Regex(
                @"(^)(Disc #)(\d+)( has )(\d+)( positions; at time=0, it is at position )(\d+)(\.)");
            var number = new List<int> { 1 };
            var starting = new List<int> { 0 };
            for (int i = 0; i < inputLines.Length; i++)
            {
                var line = inputLines[i];
                if (validLineRegex.IsMatch(line))
                {
                    var discNumber = int.Parse(Regex.Match(line, 
                        @"(?<=Disc #)(\d+)(?= has )").Value);
                    if (discNumber != i + 1)
                    {
                        solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                        return solution;
                    }
                    number.Add(int.Parse(Regex.Match(line, 
                        @"(?<= has )(\d+)(?= positions; at time=)").Value));
                    starting.Add(int.Parse(Regex.Match(line, 
                        @"(?<= positions; at time=0, it is at position )(\d+)(?=\.)").Value));
                }
            }

            int partOneSolution = CalculateSolution(number.ToArray(), starting.ToArray());
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            number.Add(11);
            starting.Add(0);

            int partTwoSolution = CalculateSolution(number.ToArray(), starting.ToArray());
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int CalculateSolution(int[] numberOfPositions, int[] startingPosition)
        {
            int time = 0;
            while (true)
            {
                int lcm = 1;
                for (int i = 0; i < numberOfPositions.Length; i++)
                {
                    int collisionTime = time + i;
                    int collisionPosition = (startingPosition[i] + collisionTime) % numberOfPositions[i];
                    if (collisionPosition == 0)
                    {
                        if (i == numberOfPositions.Length - 1)
                        {
                            return time;
                        }
                        lcm = LCM(lcm, numberOfPositions[i]);
                    }
                    else
                    {
                        time += lcm;
                        break;
                    }
                }
            }
        }

        int LCM(int a, int b)
        {
            return a / GCD(a, b) * b;
        }

        int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                int max = Math.Max(a, b);
                int min = Math.Min(a, b);
                a = max % min;
                b = min;
            }
            return a + b;
        }
    }
}
