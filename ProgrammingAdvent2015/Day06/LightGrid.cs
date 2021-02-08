// Made for Advent of Code 2015, Day 6 puzzle
// https://adventofcode.com/2015/day/6
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;

namespace ProgrammingAdvent2015
{
    abstract class LightGrid
    {
        public abstract void ToggleRectangle(int x1, int x2, int y1, int y2);

        public abstract void SetRectangle(int x1, int x2, int y1, int y2, bool value);
    }
}
