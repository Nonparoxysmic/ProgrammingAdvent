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

            int possibleTriangles = 0;
            foreach (string line in input)
            {
                string[] inputs = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (inputs.Length != 3) continue;
                int sum = 0;
                int max = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (!int.TryParse(inputs[i], out int value))
                    {
                        partOneTextBox.Text = "ERROR: Invalid data \"" + inputs[i] + "\" in input.";
                        return;
                    }
                    sum += value;
                    max = Math.Max(max, value);
                }
                if (sum - max > max) possibleTriangles++;
            }
            partOneTextBox.Text = possibleTriangles.ToString();

            partTwoTextBox.Text = "Part Two Solution Not Yet Implemented";
        }
    }
}
