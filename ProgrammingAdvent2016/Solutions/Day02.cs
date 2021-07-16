using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class Day02
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public PuzzleSolution Solution()
        {
            return solution;
        }

        public PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            stopwatch.Start();

            // 1 2 3
            // 4 5 6
            // 7 8 9
            int row = 1;
            int col = 1;
            string partOneAnswer = "";
            foreach (string line in inputLines)
            {
                if (line.Length == 0) continue;
                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'U':
                            if (row > 0) row--;
                            break;
                        case 'D':
                            if (row < 2) row++;
                            break;
                        case 'L':
                            if (col > 0) col--;
                            break;
                        case 'R':
                            if (col < 2) col++;
                            break;
                    }
                }
                partOneAnswer += (row * 3 + col + 1);
            }
            solution.WriteSolution(1, partOneAnswer, stopwatch.ElapsedMilliseconds);

            //     1
            //   2 3 4
            // 5 6 7 8 9
            //   A B C
            //     D
            string[,] grid = new string[,] { {  "",  "", "5",  "",  "" },
                                             {  "", "2", "6", "A",  "" },
                                             { "1", "3", "7", "B", "D" },
                                             {  "", "4", "8", "C",  "" },
                                             {  "",  "", "9",  "",  "" } };
            col = 0;
            row = 2;
            string partTwoAnswer = "";
            foreach (string line in inputLines)
            {
                if (line.Length == 0) continue;
                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'U':
                            if (row > 0 && grid[col, row - 1] != "") row--;
                            break;
                        case 'D':
                            if (row < 4 && grid[col, row + 1] != "") row++;
                            break;
                        case 'L':
                            if (col > 0 && grid[col - 1, row] != "") col--;
                            break;
                        case 'R':
                            if (col < 4 && grid[col + 1, row] != "") col++;
                            break;
                    }
                }
                partTwoAnswer += grid[col, row];
            }
            solution.WriteSolution(2, partTwoAnswer, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
