// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;

namespace ProgrammingAdvent2018.Program
{
    public struct Color
    {
        public byte A { get; }
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public static Color Black { get => new Color(255, 0, 0, 0); }
        public static Color White { get => new Color(255, 255, 255, 255); }

        private Color(int alpha, int red, int green, int blue)
        {
            A = (byte)alpha;
            R = (byte)red;
            G = (byte)green;
            B = (byte)blue;
        }

        public static Color FromArgb(int red, int green, int blue)
        {
            if (red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
            {
                throw new ArgumentException();
            }
            return new Color(255, red, green, blue);
        }

        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            if (alpha < 0 || alpha > 255 || red < 0 || red > 255
                || green < 0 || green > 255 || blue < 0 || blue > 255)
            {
                throw new ArgumentException();
            }
            return new Color(alpha, red, green, blue);
        }
    }
}
