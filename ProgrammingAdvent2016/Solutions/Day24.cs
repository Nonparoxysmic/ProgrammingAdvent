// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Diagnostics;
using System.Drawing;

namespace ProgrammingAdvent2016
{
    public class Day24 : Day
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

            var map = new Bitmap(inputLines[0].Length, inputLines.Length);
            for (int y = 0; y < inputLines.Length; y++)
            {
                var line = inputLines[y];
                if (line.Length != inputLines[0].Length)
                {
                    solution.WriteSolution(1, "ERROR: Inconsistent line length in input.", 0);
                    break;
                }
                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '#':
                            map.SetPixel(x, y, Color.Black);
                            break;
                        case '.':
                            map.SetPixel(x, y, Color.White);
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            map.SetPixel(x, y, Color.Red);
                            break;
                        default:
                            solution.WriteSolution(1, "ERROR: Invalid character '" + line[x] + "' in input.", 0);
                            break;
                    }
                }
            }

            SaveMapToDesktop(map, "DAY24INPUT");

            stopwatch.Reset();
            return solution;
        }

        void SaveMapToDesktop(Bitmap bitmap, string filename)
        {
            int scaleFactor = 5;
            var output = new Bitmap(bitmap.Width * scaleFactor, bitmap.Height * scaleFactor);
            var g = Graphics.FromImage(output);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.DrawImage(bitmap, 0, 0, output.Width, output.Height);
            g.Dispose();
            output.Save(Environment.GetEnvironmentVariable("HOMEDRIVE")
                + Environment.GetEnvironmentVariable("HOMEPATH")
                + @"\Desktop\" + filename + ".png");
        }
    }
}
