// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day06 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.Trim().ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            int messageLength = inputLines[0].Length;
            int[][] letterFrequencies = new int[messageLength][];
            for (int i = 0; i < messageLength; i++)
            {
                letterFrequencies[i] = new int[26];
            }
            foreach (string line in inputLines)
            {
                if (line.Length != messageLength) continue;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] >= 97 && line[i] <= 122)
                    {
                        letterFrequencies[i][line[i] - 97]++;
                    }
                    else
                    {
                        solution.WriteSolution(1, "ERROR: Invalid character '" + line[i] + "' in input.", stopwatch.ElapsedMilliseconds);
                        return solution;
                    }
                }
            }
            StringBuilder partOneMessage = new StringBuilder();
            StringBuilder partTwoMessage = new StringBuilder();
            for (int i = 0; i < messageLength; i++)
            {
                int mostCommon = letterFrequencies[i].Max();
                int leastCommon = int.MaxValue;
                foreach (int value in letterFrequencies[i])
                {
                    if (value == 0) continue;
                    leastCommon = Math.Min(leastCommon, value);
                }
                char mostCommonChar = '_';
                char leastCommonChar = '_';
                for (int j = 0; j < 26; j++)
                {
                    if (letterFrequencies[i][j] == mostCommon)
                    {
                        mostCommonChar = (char)(j + 97);
                    }
                    if (letterFrequencies[i][j] == leastCommon)
                    {
                        leastCommonChar = (char)(j + 97);
                    }
                }
                partOneMessage.Append(mostCommonChar);
                partTwoMessage.Append(leastCommonChar);
            }
            solution.WriteSolution(1, partOneMessage.ToString(), stopwatch.ElapsedMilliseconds);
            solution.WriteSolution(2, partTwoMessage.ToString(), solution.PartOneMilliseconds());

            stopwatch.Reset();
            return solution;
        }
    }
}
