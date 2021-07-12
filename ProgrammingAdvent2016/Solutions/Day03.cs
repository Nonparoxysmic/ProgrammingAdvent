﻿using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public static class Day03
    {
        public static PuzzleSolution Solution(string input)
        {
            string[] inputLines = input.ToLines();
            PuzzleSolution solution = new PuzzleSolution();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int possibleTriangles = 0;
            foreach (string line in inputLines)
            {
                string[] inputs = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (inputs.Length != 3) continue;
                int sum = 0;
                int max = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (!int.TryParse(inputs[i], out int value))
                    {
                        solution.WriteSolution(1, "ERROR: Invalid data \"" + inputs[i] + "\" in input.", stopwatch.ElapsedMilliseconds);
                        return solution;
                    }
                    sum += value;
                    max = Math.Max(max, value);
                }
                if (sum - max > max) possibleTriangles++;
            }
            solution.WriteSolution(1, possibleTriangles.ToString(), stopwatch.ElapsedMilliseconds);

            if (inputLines.Length < 3 || inputLines.Length % 3 != 0)
            {
                solution.WriteSolution(2, "ERROR: Invalid input length.", stopwatch.ElapsedMilliseconds);
                return solution;
            }
            possibleTriangles = 0;
            for (int i = 0; i < inputLines.Length; i += 3)
            {
                string[][] dataArrays = new string[3][];
                dataArrays[0] = inputLines[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                dataArrays[1] = inputLines[i + 1].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                dataArrays[2] = inputLines[i + 2].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (dataArrays[0].Length != 3 || dataArrays[1].Length != 3 || dataArrays[2].Length != 3) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (!int.TryParse(dataArrays[0][j], out int value0) ||
                        !int.TryParse(dataArrays[1][j], out int value1) ||
                        !int.TryParse(dataArrays[2][j], out int value2))
                    {
                        solution.WriteSolution(2, "ERROR: Invalid data in input.", stopwatch.ElapsedMilliseconds);
                        return solution;
                    }
                    int sum = value0 + value1 + value2;
                    int max = Math.Max(value0, Math.Max(value1, value2));
                    if (sum - max > max) possibleTriangles++;
                }
            }
            solution.WriteSolution(2, possibleTriangles.ToString(), stopwatch.ElapsedMilliseconds);
            return solution;
        }
    }
}
