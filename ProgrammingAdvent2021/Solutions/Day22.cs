// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day22 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<Region> partOneSteps = [];
        List<Region> partTwoSteps = [];
        foreach (string line in input)
        {
            Match partOneMatch = GeneratedRegex.ValidDay22PartOneLine.Match(line);
            if (partOneMatch.Success)
            {
                int xMin = int.Parse(partOneMatch.Groups["xMin"].Value);
                int xMax = int.Parse(partOneMatch.Groups["xMax"].Value);
                int yMin = int.Parse(partOneMatch.Groups["yMin"].Value);
                int yMax = int.Parse(partOneMatch.Groups["yMax"].Value);
                int zMin = int.Parse(partOneMatch.Groups["zMin"].Value);
                int zMax = int.Parse(partOneMatch.Groups["zMax"].Value);
                int toggle = partOneMatch.Groups["toggle"].Value == "on" ? 1 : 0;
                partOneSteps.Add(new Region(xMin, xMax, yMin, yMax, zMin, zMax, toggle));
                continue;
            }
            Match partTwoMatch = GeneratedRegex.ValidDay22PartTwoLine.Match(line);
            if (partTwoMatch.Success)
            {
                int xMin = int.Parse(partTwoMatch.Groups["xMin"].Value);
                int xMax = int.Parse(partTwoMatch.Groups["xMax"].Value);
                int yMin = int.Parse(partTwoMatch.Groups["yMin"].Value);
                int yMax = int.Parse(partTwoMatch.Groups["yMax"].Value);
                int zMin = int.Parse(partTwoMatch.Groups["zMin"].Value);
                int zMax = int.Parse(partTwoMatch.Groups["zMax"].Value);
                int toggle = partTwoMatch.Groups["toggle"].Value == "on" ? 1 : 0;
                partTwoSteps.Add(new Region(xMin, xMax, yMin, yMax, zMin, zMax, toggle));
                continue;
            }
        }

        List<Region> activeCubes = [];
        foreach (Region step in partOneSteps)
        {
            List<Region> intersectingRegions = activeCubes.Where(r => r.Intersects(step)).ToList();
            foreach (Region intersection in intersectingRegions)
            {
                activeCubes.Remove(intersection);
                if (step.Contains(intersection))
                {
                    continue;
                }
                foreach (Region external in intersection.Exclude(step))
                {
                    activeCubes.Add(external);
                }
            }
            if (step.State == 1)
            {
                activeCubes.Add(step);
            }
        }
        int partOneAnswer = activeCubes.Sum(r => r.Volume());

        return ($"{partOneAnswer}", "n/a");
    }

    private readonly struct Region(int xMin, int xMax, int yMin, int yMax,
        int zMin, int zMax, int state = 1)
    {
        public int XMin { get; } = xMin;
        public int XMax { get; } = xMax;
        public int YMin { get; } = yMin;
        public int YMax { get; } = yMax;
        public int ZMin { get; } = zMin;
        public int ZMax { get; } = zMax;
        public int State { get; } = state;

        public int Volume()
        {
            return (XMax - XMin + 1) * (YMax - YMin + 1) * (ZMax - ZMin + 1);
        }

        public bool Intersects(Region other)
        {
            bool xOverlap = (XMin <= other.XMin && other.XMin <= XMax)
                || (XMin <= other.XMax && other.XMax <= XMax)
                || (other.XMin <= XMin && XMin <= other.XMax)
                || (other.XMin <= XMax && XMax <= other.XMax);
            bool yOverlap = (YMin <= other.YMin && other.YMin <= YMax)
                || (YMin <= other.YMax && other.YMax <= YMax)
                || (other.YMin <= YMin && YMin <= other.YMax)
                || (other.YMin <= YMax && YMax <= other.YMax);
            bool zOverlap = (ZMin <= other.ZMin && other.ZMin <= ZMax)
                || (ZMin <= other.ZMax && other.ZMax <= ZMax)
                || (other.ZMin <= ZMin && ZMin <= other.ZMax)
                || (other.ZMin <= ZMax && ZMax <= other.ZMax);
            return xOverlap && yOverlap && zOverlap;
        }

        public bool Contains(Region other)
        {
            return XMin <= other.XMin && other.XMax <= XMax
                && YMin <= other.YMin && other.YMax <= YMax
                && ZMin <= other.ZMin && other.ZMax <= ZMax;
        }

        public IEnumerable<Region> Exclude(Region other)
        {
            // AI-generated implementation with minor corrections
            if (other.XMin > XMin)
                yield return new Region(XMin, other.XMin - 1, YMin, YMax, ZMin, ZMax);
            if (other.XMax < XMax)
                yield return new Region(other.XMax + 1, XMax, YMin, YMax, ZMin, ZMax);
            if (other.YMin > YMin)
                yield return new Region(Math.Max(XMin, other.XMin), Math.Min(XMax, other.XMax), YMin, other.YMin - 1, ZMin, ZMax);
            if (other.YMax < YMax)
                yield return new Region(Math.Max(XMin, other.XMin), Math.Min(XMax, other.XMax), other.YMax + 1, YMax, ZMin, ZMax);
            if (other.ZMin > ZMin)
                yield return new Region(Math.Max(XMin, other.XMin), Math.Min(XMax, other.XMax), Math.Max(YMin, other.YMin), Math.Min(YMax, other.YMax), ZMin, other.ZMin - 1);
            if (other.ZMax < ZMax)
                yield return new Region(Math.Max(XMin, other.XMin), Math.Min(XMax, other.XMax), Math.Max(YMin, other.YMin), Math.Min(YMax, other.YMax), other.ZMax + 1, ZMax);
        }
    }
}
