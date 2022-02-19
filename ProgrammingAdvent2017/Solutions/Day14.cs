// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day14 : Day
    {
        private readonly int[] bitsSet = new int[]
        {
            0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4
        };

        private readonly string[] squarePatterns = new string[]
        {
            "....", "...#", "..#.", "..##", ".#..", ".#.#", ".##.", ".###",
            "#...", "#..#", "#.#.", "#.##", "##..", "##.#", "###.", "####"
        };

        private readonly (int, int)[] directions = new (int, int)[]
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }

            int totalBitsSet = 0;
            char[,] grid = new char[128, 128];
            for (int row = 0; row < 128; row++)
            {
                string hash = Day10.KnotHash(input + "-" + row);
                for (int c = 0; c < hash.Length; c++)
                {
                    int nibble;
                    if (hash[c] > 57) { nibble = hash[c] - 87; }
                    else { nibble = hash[c] - 48; }
                    totalBitsSet += bitsSet[nibble];
                    for (int bit = 0; bit < 4; bit++)
                    {
                        int col = 4 * c + bit;
                        grid[col, row] = squarePatterns[nibble][bit];
                    }
                }
            }

            int regions = 0;
            for (int col = 0; col < 128; col++)
            {
                for (int row = 0; row < 128; row++)
                {
                    if (grid[col, row] == '#')
                    {
                        EliminateRegion(grid, col, row);
                        regions++;
                    }
                }
            }

            sw.Stop();
            output.WriteAnswers(totalBitsSet, regions, sw);
            return output;
        }

        private void EliminateRegion(char[,] grid, int col, int row)
        {
            grid[col, row] = 'X';
            foreach ((int, int) dir in directions)
            {
                int x = col + dir.Item1;
                int y = row + dir.Item2;
                if (x < 0 || y < 0 || x >= 128 || y >= 128)
                {
                    continue;
                }
                if (grid[x, y] == '#')
                {
                    EliminateRegion(grid, x, y);
                }
            }
        }
    }
}
