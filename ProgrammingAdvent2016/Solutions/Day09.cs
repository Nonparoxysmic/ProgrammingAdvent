// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day09 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            input = input.RemoveWhitespace();
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            StringBuilder decompressed = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    // Start of a marker found.
                    int endPos = -1;
                    for (int j = i; j < input.Length; j++)
                    {
                        if (input[j] == ')')
                        {
                            // End of marker found.
                            endPos = j;
                            break;
                        }
                    }
                    if (endPos == -1 || endPos - i < 4)
                    {
                        // Not a valid marker.
                        decompressed.Clear();
                        decompressed.Append("ERROR");
                        break;
                    }
                    string[] markerValues = input.Substring(i + 1, endPos - i - 1).Split('x');
                    if (markerValues.Length != 2 || !int.TryParse(markerValues[0], out int charNum) || !int.TryParse(markerValues[1], out int repeats))
                    {
                        // Not a valid marker.
                        decompressed.Clear();
                        decompressed.Append("ERROR");
                        break;
                    }
                    string data = input.Substring(endPos + 1, charNum);
                    for (int k = 0; k < repeats; k++)
                    {
                        decompressed.Append(data);
                    }
                    i = endPos + charNum;
                }
                else decompressed.Append(input[i]);
            }
            solution.WriteSolution(1, decompressed.Length, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
