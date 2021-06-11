using System;
using System.IO;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    public static class Day01
    {
        public static void SetSolutionText(TextBox partOneTextBox, TextBox partTwoTextBox)
        {
            string input;
            try
            {
                input = File.ReadAllText(@"InputFiles\InputDay01Part1.txt").Trim();
            }
            catch
            {
                partOneTextBox.Text = "ERROR: Unable to read input file.";
                return;
            }
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
            partOneTextBox.Text = (x + y).ToString();

            partTwoTextBox.Text = "Part 2 Solution Not Yet Implemented";
        }
    }
}
