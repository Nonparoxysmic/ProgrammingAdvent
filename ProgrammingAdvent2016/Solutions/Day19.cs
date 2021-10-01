// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day19 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Trim().Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            if (!uint.TryParse(input.Trim(), out uint elfCount))
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            stopwatch.Start();

            uint partOneSolution = PartOneQuick(elfCount);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            uint partTwoSolution = PartTwoQuick(elfCount);
            solution.WriteSolution(2, partTwoSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        //int PartOne(int elfCount)
        //{
        //    if (elfCount < 2) { return elfCount; }
        //    var elves = new Queue<int>();
        //    for (int i = 1; i <= elfCount; i++)
        //    {
        //        elves.Enqueue(i);
        //    }
        //    while (elves.Count > 1)
        //    {
        //        elves.Enqueue(elves.Dequeue());
        //        elves.Dequeue();
        //    }
        //    return elves.Dequeue();
        //}

        uint PartOneQuick(uint elfCount)
        {
            if (elfCount < 2) { return elfCount; }
            if (elfCount >= 0x80000000)
            {
                elfCount <<= 1;
                return ++elfCount;
            }

            uint msb = elfCount;
            for (int i = 1; i <= 16; i *= 2)
            {
                msb |= msb >> i;
            }
            msb = ++msb >> 1;

            elfCount &= ~msb;
            elfCount <<= 1;
            return ++elfCount;
        }

        //int PartTwo(int elfCount)
        //{
        //    if (elfCount < 2) { return elfCount; }
        //    var elves = new List<int>();
        //    for (int i = 1; i <= elfCount; i++)
        //    {
        //        elves.Add(i);
        //    }
        //    for (int elf = 1; true; elf++)
        //    {
        //        if (elf > elfCount) { elf = 1; }
        //        if (elves.Contains(elf))
        //        {
        //            int pos = elves.IndexOf(elf);
        //            int target = (pos + elves.Count / 2) % elves.Count;
        //            elves.RemoveAt(target);
        //            if (elves.Count == 1) { break; }
        //        }
        //    }
        //    return elves[0];
        //}

        uint PartTwoQuick(uint elfCount)
        {
            if (elfCount < 2) { return elfCount; }
            string ternary = UintToTernary(elfCount);
            if (Regex.Match(ternary, @"(?<=^)(10+)(?=$)").Success)
            {
                return elfCount;
            }
            if (ternary[0] == '1')
            {
                return elfCount - (uint)Math.Pow(3, ternary.Length - 1);
            }
            return 2 * elfCount - 3 * (uint)Math.Pow(3, ternary.Length - 1);
        }

        string UintToTernary(uint value)
        {
            char[] digits = new char[] { '0', '1', '2' };
            int i = 32;
            char[] buffer = new char[i];
            do
            {
                buffer[--i] = digits[value % 3];
                value /= 3;
            } while (value > 0);
            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);
            return new string(result);
        }
    }
}
