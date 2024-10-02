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
                Region region = new(xMin, xMax, yMin, yMax, zMin, zMax, toggle);
                partOneSteps.Add(region);
                partTwoSteps.Add(region);
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

        long partOneAnswer = ExecuteSteps(partOneSteps);
        long partTwoAnswer = ExecuteSteps(partTwoSteps);

        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static long ExecuteSteps(List<Region> steps)
    {
        List<Region> activeCubes = [];
        foreach (Region step in steps)
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
        return activeCubes.Sum(r => r.Volume());
    }

    private readonly struct Region(long xMin, long xMax, long yMin, long yMax,
        long zMin, long zMax, long state = 1)
    {
        public long XMin { get; } = xMin;
        public long XMax { get; } = xMax;
        public long YMin { get; } = yMin;
        public long YMax { get; } = yMax;
        public long ZMin { get; } = zMin;
        public long ZMax { get; } = zMax;
        public long State { get; } = state;

        public long Volume()
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
