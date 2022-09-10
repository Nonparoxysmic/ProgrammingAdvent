// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProgrammingAdvent2018.Program
{
    public class SimpleBitmap
    {
        public int Width { get; }
        public int Height { get; }

        private readonly byte[] pixelData;

        public SimpleBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            pixelData = new byte[4 * width * height];
        }

        public SimpleBitmap(int width, int height, Color color)
        {
            Width = width;
            Height = height;
            pixelData = new byte[4 * width * height];
            for (int index = 0; index < pixelData.Length - 3; index += 4)
            {
                pixelData[index + 3] = color.A;
                pixelData[index + 2] = color.R;
                pixelData[index + 1] = color.G;
                pixelData[index] = color.B;
            }
        }

        public BitmapSource ToBitmapSource()
        {
            BitmapSource output = BitmapSource.Create(
                Width,
                Height,
                96,
                96,
                PixelFormats.Bgra32,
                null,
                pixelData,
                4 * Width);
            output.Freeze();
            return output;
        }

        public Color GetPixel(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException();
            }
            int index = ((Width * y) + x) * 4;
            return Color.FromArgb(
                pixelData[index + 3],
                pixelData[index + 2],
                pixelData[index + 1],
                pixelData[index]);
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }
            int index = ((Width * y) + x) * 4;
            pixelData[index + 3] = color.A;
            pixelData[index + 2] = color.R;
            pixelData[index + 1] = color.G;
            pixelData[index] = color.B;
        }

        public static SimpleBitmap Scale(SimpleBitmap input, int scale)
        {
            if (scale < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (scale == 1)
            {
                return input;
            }
            SimpleBitmap output = new SimpleBitmap(input.Width * scale, input.Height * scale);
            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color color = input.GetPixel(x, y);
                    for (int Δy = 0; Δy < scale; Δy++)
                    {
                        for (int Δx = 0; Δx < scale; Δx++)
                        {
                            output.SetPixel((scale * x) + Δx, (scale * y) + Δy, color);
                        }
                    }
                }
            }
            return output;
        }
    }
}
