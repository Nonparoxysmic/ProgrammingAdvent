// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;

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

            solution.WriteSolution(1, DecompressedLength(input, false), stopwatch.ElapsedMilliseconds);

            solution.WriteSolution(2, DecompressedLength(input, true), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        public long DecompressedLength(string input, bool recursive)
        {
            long decompressedLength = 0;
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
                        decompressedLength = -1;
                        break;
                    }
                    string[] markerValues = input.Substring(i + 1, endPos - i - 1).Split('x');
                    if (markerValues.Length != 2 || !int.TryParse(markerValues[0], out int charNum) || !int.TryParse(markerValues[1], out int repeats))
                    {
                        // Not a valid marker.
                        decompressedLength = -1;
                        break;
                    }
                    if (recursive)
                    {
                        decompressedLength += DecompressedLength(input.Substring(endPos + 1, charNum), true) * repeats;
                    }
                    else decompressedLength += charNum * repeats;
                    i = endPos + charNum;
                }
                else decompressedLength++;
            }
            return decompressedLength;
        }
    }
}
