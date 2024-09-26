// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day20 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        if (input.Length < 3 || input[0].Length != 512)
        {
            return ("Invalid input", "n/a");
        }
        bool[] algorithm = input[0].Select(c => c == '#').ToArray();
        int defaultPixel = algorithm[0] ? 1 : 0; 
        Image initialImage = new(input);
        Image buffer = new();
        Enhance(initialImage, buffer, algorithm);
        Enhance(buffer, initialImage, algorithm, defaultPixel);
        int partOneAnswer = initialImage.PixelsLit();
        return ($"{partOneAnswer}", "n/a");
    }

    private static void Enhance(Image input, Image output, bool[] algorithm, int dp = 0)
    {
        input.DefaultPixel = dp;
        output.Clear();
        for (int y = input.Ymin - 1; y <= input.Ymax + 1; y++)
        {
            for (int x = input.Xmin - 1; x <= input.Xmax + 1; x++)
            {
                int index = input.GetPixel(x - 1, y - 1) << 8
                    | input.GetPixel(x, y - 1) << 7
                    | input.GetPixel(x + 1, y - 1) << 6
                    | input.GetPixel(x - 1, y) << 5
                    | input.GetPixel(x, y) << 4
                    | input.GetPixel(x + 1, y) << 3
                    | input.GetPixel(x - 1, y + 1) << 2
                    | input.GetPixel(x, y + 1) << 1
                    | input.GetPixel(x + 1, y + 1) << 0;
                if (algorithm[index])
                {
                    output.AddPixel(x, y);
                }
            }
        }
    }

    private class Image
    {
        public int Xmin { get; private set; } = int.MaxValue;
        public int Ymin { get; private set; } = int.MaxValue;
        public int Xmax { get; private set; } = int.MinValue;
        public int Ymax { get; private set; } = int.MinValue;
        public int DefaultPixel { get; set; }

        private readonly HashSet<(int, int)> _lightPixels = [];

        public Image() { }

        public Image(string[] input)
        {
            int width = input[2].Length;
            int height = input.Length - 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (input[y + 2][x] == '#')
                    {
                        AddPixel(x, y);
                    }
                }
            }
        }

        public void AddPixel(int x, int y)
        {
            _lightPixels.Add((x, y));
            Xmin = Math.Min(Xmin, x);
            Ymin = Math.Min(Ymin, y);
            Xmax = Math.Max(Xmax, x);
            Ymax = Math.Max(Ymax, y);
        }

        public int GetPixel(int x, int y)
        {
            if (x < Xmin || x > Xmax || y < Ymin || y > Ymax)
            {
                return DefaultPixel;
            }
            return _lightPixels.Contains((x, y)) ? 1 : 0;
        }

        public void Clear() => _lightPixels.Clear();

        public int PixelsLit() => _lightPixels.Count;

        public bool PixelIsLit(int x, int y) => _lightPixels.Contains((x, y));
    }
}
