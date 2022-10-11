// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;

namespace ProgrammingAdvent2018.Program
{
    class MapArray<T>
    {
        private static readonly MapArray<T> _empty = new MapArray<T>(0, 0, 0, (0, 0));
        public static MapArray<T> Empty { get => _empty; }

        public (int X, int Y) Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Border { get; private set; }

        private readonly T[,] array;

        public MapArray(int width, int height, int borderThickness, (int X, int Y) position)
        {
            if (width < 0 || height < 0 || borderThickness < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Width = width;
            Height = height;
            Border = borderThickness;
            array = new T[Width + 2 * Border, Height + 2 * Border];
            Position = position;
        }

        public MapArray(int width, int height) : this(width, height, 1, (0, 0)) { }

        public MapArray(int width, int height, int borderThickness, T borderValue, (int X, int Y) position)
            : this(width, height, borderThickness, position)
        {
            if (Border < 1)
            {
                return;
            }
            int totalWidth = array.GetLength(0);
            int totalHeight = array.GetLength(1);
            for (int i = 0; i < Border; i++)
            {
                for (int x = 0; x < totalWidth; x++)
                {
                    array[x, i] = borderValue;
                    array[x, totalHeight - 1 - i] = borderValue;
                }
            }
            for (int y = Border; y < totalHeight - Border; y++)
            {
                for (int i = 0; i < Border; i++)
                {
                    array[i, y] = borderValue;
                    array[totalWidth - 1 - i, y] = borderValue;
                }
            }
        }

        public void Fill(T fill)
        {
            int totalWidth = array.GetLength(0);
            int totalHeight = array.GetLength(1);
            for (int y = Border; y < totalHeight - Border; y++)
            {
                for (int x = Border; x < totalWidth - Border; x++)
                {
                    array[x, y] = fill;
                }
            }
        }

        public T this[int x, int y]
        {
            get => GetValue(x, y);
            set => SetValue(x, y, value);
        }

        private T GetValue(int x, int y)
        {
            return array[x + Border - Position.X, y + Border - Position.Y];
        }

        private void SetValue(int x, int y, T setValue)
        {
            array[x + Border - Position.X, y + Border - Position.Y] = setValue;
        }
    }
}
