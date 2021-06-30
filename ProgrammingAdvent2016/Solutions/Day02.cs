using System;
using System.IO;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    public static class Day02
    {
        public static void SetSolutionText(TextBox partOneTextBox, TextBox partTwoTextBox)
        {
            string[] input;
            try
            {
                input = File.ReadAllLines(@"InputFiles\InputDay02Part1.txt");
            }
            catch
            {
                partOneTextBox.Text = "ERROR: Unable to read input file.";
                return;
            }

            // 1 2 3
            // 4 5 6
            // 7 8 9
            int row = 1;
            int col = 1;
            string partOneAnswer = "";
            foreach (string line in input)
            {
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
            partOneTextBox.Text = partOneAnswer;

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
            foreach (string line in input)
            {
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
            partTwoTextBox.Text = partTwoAnswer;
        }
    }
}
