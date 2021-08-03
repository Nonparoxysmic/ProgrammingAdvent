// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Drawing;

namespace ProgrammingAdvent2016
{
    public class Day08 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            LittleScreen screen = new LittleScreen(50, 6);
            foreach (string line in inputLines)
            {
                string[] terms = line.Split();
                if (terms.Length < 2) continue;
                switch (terms[0])
                {
                    case "rect":
                        string[] values = terms[1].Split('x');
                        if (values.Length == 2 && int.TryParse(values[0], out int rectWidth) && int.TryParse(values[1], out int rectHeight))
                        {
                            screen.Rect(rectWidth, rectHeight);
                        }
                        break;
                    case "rotate":
                        if (terms.Length != 5 || terms[3] != "by" || terms[2].Length < 3) continue;
                        if (int.TryParse(terms[2].Substring(2), out int rowOrCol)
                            && int.TryParse(terms[4], out int steps))
                        {
                            if (terms[1] == "row")
                            {
                                screen.RotateRow(rowOrCol, steps);
                            }
                            else if (terms[1] == "column")
                            {
                                screen.RotateColumn(rowOrCol, steps);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            solution.WriteSolution(1, screen.PixelsLit().ToString(), stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }

    class LittleScreen
    {
        readonly int width;
        readonly int height;
        readonly bool[,] pixels;

        public LittleScreen(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new bool[width, height];
        }

        public void Rect(int wide, int tall)
        {
            for (int col = 0; col < wide; col++)
            {
                for (int row = 0; row < tall; row++)
                {
                    pixels[col, row] = true;
                }
            }
        }

        public void RotateRow(int row, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                RotateRow(row);
            }
        }

        void RotateRow(int row)
        {
            bool wrap = pixels[width - 1, row];
            for (int i = width - 1; i > 0; i--)
            {
                pixels[i, row] = pixels[i - 1, row];
            }
            pixels[0, row] = wrap;
        }

        public void RotateColumn(int col, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                RotateColumn(col);
            }
        }

        void RotateColumn(int col)
        {
            bool wrap = pixels[col, height - 1];
            for (int i = height - 1; i > 0; i--)
            {
                pixels[col, i] = pixels[col, i - 1];
            }
            pixels[col, 0] = wrap;
        }

        public int PixelsLit()
        {
            int count = 0;
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (pixels[col, row]) count++;
                }
            }
            return count;
        }

        public Bitmap ToBitmap()
        {
            Bitmap output = new Bitmap(width * 2 + 4, height * 2 + 4);
            using (Graphics g = Graphics.FromImage(output))
            {
                g.FillRectangle(new SolidBrush(Color.Lime), 0, 0, width * 2 + 4, height * 2 + 4);
            }
            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (pixels[col, row])
                    {
                        int baseCol = col * 2 + 2;
                        int baseRow = row * 2 + 2;
                        output.SetPixel(baseCol, baseRow, Color.Black);
                        output.SetPixel(baseCol + 1, baseRow, Color.Black);
                        output.SetPixel(baseCol, baseRow + 1, Color.Black);
                        output.SetPixel(baseCol + 1, baseRow + 1, Color.Black);
                    }
                }
            }
            return output;
        }
    }
}
