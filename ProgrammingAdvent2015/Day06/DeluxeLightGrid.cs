// Made for Advent of Code 2015, Day 6 puzzle
// https://adventofcode.com/2015/day/6
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;

namespace ProgrammingAdvent2015
{
    class DeluxeLightGrid : LightGrid
    {
        readonly int[,] lights;

        public DeluxeLightGrid()
        {
            lights = new int[1000, 1000];
        }

        public int SumBrightness()
        {
            int sum = 0;
            foreach (int light in lights)
            {
                sum += light;
            }
            return sum;
        }

        public override void ToggleRectangle(int x1, int x2, int y1, int y2)
        {
            if ((x1 > x2) || (y1 > y2)) throw new ArgumentOutOfRangeException();
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    lights[x, y] += 2;
                }
            }
        }

        public override void SetRectangle(int x1, int x2, int y1, int y2, bool value)
        {
            if ((x1 > x2) || (y1 > y2)) throw new ArgumentOutOfRangeException();
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    //lights[x, y] = value;
                    if (value) lights[x, y]++;
                    else if (lights[x, y] > 0) lights[x, y]--;
                }
            }
        }
    }
}
