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
            
            if (input.Length < 3 || input.Length % 3 != 0)
            {
                partTwoTextBox.Text = "ERROR: Invalid input.";
                return;
            }
            possibleTriangles = 0;
            for (int i = 0; i < input.Length; i += 3)
            {
                string[][] dataArrays = new string[3][];
                dataArrays[0] = input[i].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                dataArrays[1] = input[i + 1].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                dataArrays[2] = input[i + 2].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (dataArrays[0].Length != 3 || dataArrays[1].Length != 3 || dataArrays[2].Length != 3) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (!int.TryParse(dataArrays[0][j], out int value0) ||
                        !int.TryParse(dataArrays[1][j], out int value1) ||
                        !int.TryParse(dataArrays[2][j], out int value2))
                    {
                        partTwoTextBox.Text = "ERROR: Invalid data in input.";
                        return;
                    }
                    int sum = value0 + value1 + value2;
                    int max = Math.Max(value0, Math.Max(value1, value2));
                    if (sum - max > max) possibleTriangles++;
                }
            }
            partTwoTextBox.Text = possibleTriangles.ToString();
        }
    }
}
