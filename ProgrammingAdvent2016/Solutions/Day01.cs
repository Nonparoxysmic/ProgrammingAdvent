// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day01 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            stopwatch.Start();
            
            string[] instructions = input.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int direction = 0;
            int x = 0;
            int y = 0;
            foreach (string instruction in instructions)
            {
                if (instruction[0] == 'R') direction++;
                else if (instruction[0] == 'L') direction--;
                else continue;
                if (direction < 0) direction += 4;
                direction %= 4;
                if (int.TryParse(instruction.Substring(1), out int steps))
                {
                    switch (direction)
                    {
                        case 0:
                            y += steps;
                            break;
                        case 1:
                            x += steps;
                            break;
                        case 2:
                            y -= steps;
                            break;
                        case 3:
                            x -= steps;
                            break;
                    }
                }
            }
            solution.WriteSolution(1, (Math.Abs(x) + Math.Abs(y)).ToString(), stopwatch.ElapsedMilliseconds);

            List<long> intersections = new List<long> { 0 };
            direction = 0;
            x = 0;
            y = 0;
            foreach (string instruction in instructions)
            {
                if (instruction[0] == 'R') direction++;
                else if (instruction[0] == 'L') direction--;
                else continue;
                if (direction < 0) direction += 4;
                direction %= 4;
                if (int.TryParse(instruction.Substring(1), out int steps))
                {
                    while (steps > 0)
                    {
                        steps--;
                        switch (direction)
                        {
                            case 0:
                                y += 1;
                                break;
                            case 1:
                                x += 1;
                                break;
                            case 2:
                                y -= 1;
                                break;
                            case 3:
                                x -= 1;
                                break;
                        }
                        long intersection = (long)x << 32 | (uint)y;
                        if (intersections.Contains(intersection))
                        {
                            solution.WriteSolution(2, (Math.Abs(x) + Math.Abs(y)).ToString(), stopwatch.ElapsedMilliseconds);
                            return solution;
                        }
                        else
                        {
                            intersections.Add(intersection);
                        }
                    }
                }
            }
            solution.WriteSolution(2, "No Solution Found", stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
