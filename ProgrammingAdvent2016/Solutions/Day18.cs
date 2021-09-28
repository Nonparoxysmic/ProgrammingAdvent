// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day18 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            int partOneSolution = NumberOfSafeTiles(input, 40);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            int partTwoSolution = NumberOfSafeTiles(input, 400000);
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int NumberOfSafeTiles(string input, int rows)
        {
            var safeTiles = new bool[input.Length, rows];
            for (int i = 0; i < input.Length; i++)
            {
                safeTiles[i, 0] = input[i] == '.';
            }

            for (int row = 1; row < rows; row++)
            {
                for (int col = 0; col < input.Length; col++)
                {
                    bool a = col < 1 || safeTiles[col - 1, row - 1];
                    bool b = safeTiles[col, row - 1];
                    bool c = col > input.Length - 2 || safeTiles[col + 1, row - 1];
                    if (a && b && c) { safeTiles[col, row] = true; }
                    if (a && !b && c) { safeTiles[col, row] = true; }
                    if (!a && b && !c) { safeTiles[col, row] = true; }
                    if (!a && !b && !c) { safeTiles[col, row] = true; }
                }
            }

            int output = 0;
            foreach (bool b in safeTiles)
            {
                if (b) { output++; }
            }
            return output;
        }
    }
}
