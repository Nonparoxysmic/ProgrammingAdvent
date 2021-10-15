// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ProgrammingAdvent2016
{
    public class Day24 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        Bitmap map;
        Dictionary<int, Coordinates> locations;

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

            map = new Bitmap(inputLines[0].Length, inputLines.Length);
            locations = new Dictionary<int, Coordinates>();
            for (int y = 0; y < inputLines.Length; y++)
            {
                var line = inputLines[y];
                if (line.Length != inputLines[0].Length)
                {
                    solution.WriteSolution(1, "ERROR: Inconsistent line length in input.", 0);
                    return solution;
                }
                for (int x = 0; x < line.Length; x++)
                {
                    char character = line[x];
                    if ((x == 0 || x == line.Length - 1)
                        && (y == 0 || y == inputLines.Length - 1)
                        && character != '#')
                    {
                        solution.WriteSolution(1, "ERROR: Invalid boundary walls.", 0);
                        return solution;
                    }
                    switch (character)
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
                            int pointNumber = int.Parse(character.ToString());
                            if (locations.ContainsKey(pointNumber))
                            {
                                solution.WriteSolution(1, "ERROR: Duplicate locations in input.", 0);
                                return solution;
                            }
                            locations.Add(pointNumber, new Coordinates(x, y));
                            map.SetPixel(x, y, Color.Red);
                            break;
                        default:
                            solution.WriteSolution(1, "ERROR: Invalid character '" + character + "' in input.", 0);
                            return solution;
                    }
                }
            }
            if (locations.Count < 3)
            {
                solution.WriteSolution(1, "ERROR: Insufficent locations in input.", 0);
                return solution;
            }
            for (int i = 0; i < locations.Count; i++)
            {
                if (!locations.ContainsKey(i))
                {
                    solution.WriteSolution(1, "ERROR: Misnumbered locations in input.", 0);
                    return solution;
                }
            }

            SaveMapToDesktop(map, "day-24-input-map");

            FillDeadEnds(map);

            SaveMapToDesktop(map, "day-24-map-fewer-dead-ends");

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

        void FillDeadEnds(Bitmap bitmap)
        {
            while (true)
            {
                if (FindDeadEnd(bitmap, out int x, out int y))
                {
                    FillDeadEnd(bitmap, x, y);
                }
                else
                {
                    break;
                }
            }
        }

        bool FindDeadEnd(Bitmap bitmap, out int deadEndX, out int deadEndY)
        {
            for (int y = 1; y < bitmap.Height - 1; y++)
            {
                for (int x = 1; x < bitmap.Width - 1; x++)
                {
                    if (IsDeadEnd(bitmap, x, y))
                    {
                        deadEndX = x;
                        deadEndY = y;
                        return true;
                    }
                }
            }
            deadEndX = -1;
            deadEndY = -1;
            return false;
        }

        void FillDeadEnd(Bitmap bitmap, int x, int y)
        {
            bitmap.SetPixel(x, y, Color.Black);
            if (IsDeadEnd(bitmap, x + 1, y))
            {
                FillDeadEnd(bitmap, x + 1, y);
            }
            else if (IsDeadEnd(bitmap, x - 1, y))
            {
                FillDeadEnd(bitmap, x - 1, y);
            }
            else if (IsDeadEnd(bitmap, x, y + 1))
            {
                FillDeadEnd(bitmap, x, y + 1);
            }
            else if (IsDeadEnd(bitmap, x, y - 1))
            {
                FillDeadEnd(bitmap, x, y - 1);
            }
        }

        bool IsDeadEnd(Bitmap bitmap, int x, int y)
        {
            if (bitmap.GetPixel(x, y).ToArgb() != Color.White.ToArgb()) { return false; }
            int walls = 0;
            if (bitmap.GetPixel(x + 1, y).ToArgb() == Color.Black.ToArgb()) { walls++; }
            if (bitmap.GetPixel(x - 1, y).ToArgb() == Color.Black.ToArgb()) { walls++; }
            if (bitmap.GetPixel(x, y + 1).ToArgb() == Color.Black.ToArgb()) { walls++; }
            if (bitmap.GetPixel(x, y - 1).ToArgb() == Color.Black.ToArgb()) { walls++; }
            if (walls >= 3) { return true; }
            return false;
        }
    }

    class Coordinates
    {
        public int x;
        public int y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
