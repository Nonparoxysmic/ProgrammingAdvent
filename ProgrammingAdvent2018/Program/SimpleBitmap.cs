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
            int index = (Width * y + x) * 4;
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
            int index = (Width * y + x) * 4;
            pixelData[index + 3] = color.A;
            pixelData[index + 2] = color.R;
            pixelData[index + 1] = color.G;
            pixelData[index] = color.B;
        }
    }
}
