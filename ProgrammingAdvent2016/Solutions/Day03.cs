using System;
using System.IO;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    public static class Day03
    {
        public static void SetSolutionText(TextBox partOneTextBox, TextBox partTwoTextBox)
        {
            string[] input;
            try
            {
                input = File.ReadAllLines(@"InputFiles\InputDay03Part1.txt");
            }
            catch
            {
                partOneTextBox.Text = "ERROR: Unable to read input file.";
                return;
            }

            foreach (string line in input)
            {
                string[] inputs = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (inputs.Length != 3) continue;
                int[] values = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    if (!int.TryParse(inputs[i], out values[i]))
                    {
                        
                    }
                }
            }
            partOneTextBox.Text = "Part One Solution Not Yet Implemented";

            partTwoTextBox.Text = "Part Two Solution Not Yet Implemented";
        }
    }
}
