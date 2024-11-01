// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day15 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^Sensor at x=(?<x>-?[0-9]{1,7}), y=(?<y>-?[0-9]{1,7}): closest beacon is at x=(?<beaconX>-?[0-9]{1,7}), y=(?<beaconY>-?[0-9]{1,7})$")]
    private static partial Regex ValidInputLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int row = exampleModifier is null ? 2_000_000 : 10;
        List<Sensor> sensors = ParseInput(input);
        int singleRowPositionsWithoutBeacons = PositionsWithoutBeacons(sensors, row);

        return result.WriteAnswers(singleRowPositionsWithoutBeacons, null);
    }

    private static List<Sensor> ParseInput(string[] input)
    {
        List<Sensor> sensors = [];
        foreach (string line in input)
        {
            Match match = ValidInputLine.Match(line);
            if (match.Success)
            {
                int x = int.Parse(match.Groups["x"].Value);
                int y = int.Parse(match.Groups["y"].Value);
                int beaconX = int.Parse(match.Groups["beaconX"].Value);
                int beaconY = int.Parse(match.Groups["beaconY"].Value);
                sensors.Add(new Sensor(x, y, beaconX, beaconY));
            }
        }
        return sensors;
    }

    private static int PositionsWithoutBeacons(List<Sensor> sensors, int row)
    {
        HashSet<int> beaconsInRow = [];
        List<(int, int)> exclusionZones = [];
        foreach (Sensor sensor in sensors)
        {
            if (sensor.HasExclusionZone(row, out int xMin, out int xMax))
            {
                exclusionZones.Add((xMin, xMax));
            }
            if (sensor.ClosestBeacon.Y == row)
            {
                beaconsInRow.Add(sensor.ClosestBeacon.X);
            }
        }
        List<(int, int)> activeRanges = [];
        foreach ((int, int) range in exclusionZones)
        {
            List<(int, int)> intersectingRanges = activeRanges.Where(r => RangesIntersect(r, range)).ToList();
            foreach ((int, int) intersection in intersectingRanges)
            {
                activeRanges.Remove(intersection);
                if (Contains(intersection, range))
                {
                    continue;
                }
                foreach ((int, int) external in Exclude(range, intersection))
                {
                    activeRanges.Add(external);
                }
            }
            activeRanges.Add(range);
        }
        int count = 0;
        foreach ((int xMin, int xMax) in activeRanges)
        {
            count += xMax - xMin + 1;
            foreach (int beaconX in beaconsInRow)
            {
                if (xMin <= beaconX && beaconX <= xMax)
                {
                    count--;
                }
            }
        }
        return count;
    }

    private static bool RangesIntersect((int xMin, int xMax) a, (int xMin, int xMax) b)
    {
        return (a.xMin <= b.xMin && b.xMin <= a.xMax)
            || (a.xMin <= b.xMax && b.xMax <= a.xMax)
            || (b.xMin <= a.xMin && a.xMin <= b.xMax)
            || (b.xMin <= a.xMax && a.xMax <= b.xMax);
    }

    private static bool Contains((int xMin, int xMax) inner, (int xMin, int xMax) outer)
    {
        return outer.xMin <= inner.xMin && inner.xMax <= outer.xMax;
    }

    public static IEnumerable<(int, int)> Exclude((int xMin, int xMax) exclude, (int xMin, int xMax) range)
    {
        if (exclude.xMin > range.xMin)
            yield return (range.xMin, exclude.xMin - 1);
        if (exclude.xMax < range.xMax)
            yield return(exclude.xMax + 1, range.xMax);
    }

    private class Sensor
    {
        public Vector2Int Position { get; }
        public Vector2Int ClosestBeacon { get; }
        public int Radius { get; }

        public Sensor(int posX, int posY, int beaconX, int beaconY)
        {
            Position = new Vector2Int(posX, posY);
            ClosestBeacon = new Vector2Int(beaconX, beaconY);
            Radius = (ClosestBeacon - Position).TaxicabMagnitude();
        }

        public bool HasExclusionZone(int row, out int xMin, out int xMax)
        {
            int zoneRadius = Radius - Math.Abs(row - Position.Y);
            if (zoneRadius < 0)
            {
                xMin = 0;
                xMax = 0;
                return false;
            }
            xMin = Position.X - zoneRadius;
            xMax = Position.X + zoneRadius;
            return true;
        }
    }
}
