using System;
using System.Collections.Generic;
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
            partOneTextBox.Text = (Math.Abs(x) + Math.Abs(y)).ToString();

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
                            partTwoTextBox.Text = (Math.Abs(x) + Math.Abs(y)).ToString();
                            return;
                        }
                        else
                        {
                            intersections.Add(intersection);
                        }
                    }
                }
            }
            partTwoTextBox.Text = "No Solution Found";
        }
    }
}
