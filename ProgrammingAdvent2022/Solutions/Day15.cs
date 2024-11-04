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
        Vector2Int distressBeaconPosition = DistressBeaconPosition(sensors, row * 2);
        long tuningFrequency = distressBeaconPosition.X * 4_000_000L + distressBeaconPosition.Y;

        return result.WriteAnswers(singleRowPositionsWithoutBeacons, tuningFrequency);
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

    private static Vector2Int DistressBeaconPosition(List<Sensor> sensors, int coordLimit)
    {
        if (TrySensorPairs(sensors, coordLimit, out Vector2Int answer))
        {
            return answer;
        }
        //for (int y = coordLimit; y >= 0; y--)
        //{
        //    int x = FirstPossiblePosition(sensors, y);
        //    if (0 <= x && x <= coordLimit)
        //    {
        //        return new Vector2Int(x, y);
        //    }
        //}
        return Vector2Int.Zero;
    }

    private static bool TrySensorPairs(List<Sensor> sensors, int coordLimit, out Vector2Int answer)
    {
        List<(int, int, int)> pairIndices = [];
        for (int i = 0; i < sensors.Count - 1; i++)
        {
            for (int j = i + 1; j < sensors.Count; j++)
            {
                int distance = (sensors[i].Position - sensors[j].Position).TaxicabMagnitude();
                int targetDistance = sensors[i].Radius + sensors[j].Radius + 2;
                pairIndices.Add((distance - targetDistance, i, j));
            }
        }
        List<(int, int, int)> matches = pairIndices.Where(t => t.Item1 == 0).ToList();
        if (matches.Count > 1)
        {
            for (int i = 0; i <= matches.Count - 1; i++)
            {
                for (int j = i + 1; j < matches.Count; j++)
                {
                    if (HasValidY(matches[i], matches[j], sensors, out int y))
                    {
                        for (int dy = 0; dy < coordLimit; dy++)
                        {
                            if (0 <= y + dy && y + dy <= coordLimit)
                            {
                                int x = FirstPossiblePosition(sensors, y + dy);
                                if (0 <= x && x <= coordLimit)
                                {
                                    answer = new Vector2Int(x, y + dy);
                                    return true;
                                }
                            }
                            if (0 <= y - dy && y - dy <= coordLimit)
                            {
                                int x = FirstPossiblePosition(sensors, y - dy);
                                if (0 <= x && x <= coordLimit)
                                {
                                    answer = new Vector2Int(x, y - dy);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        answer = Vector2Int.Zero;
        return false;
    }

    private static bool HasValidY((int, int, int) match1, (int, int, int) match2,
        List<Sensor> sensors, out int y)
    {
        y = 0;
        Sensor sensor0 = sensors[match1.Item2];
        Sensor sensor1 = sensors[match1.Item3];
        Sensor sensor2 = sensors[match2.Item2];
        Sensor sensor3 = sensors[match2.Item3];
        if (sensor0 == sensor2 || sensor0 == sensor3 || sensor1 == sensor2 || sensor1 == sensor3)
        {
            return false;
        }
        if (sensor0.Position.Y == sensor1.Position.Y)
        {
            y = sensor0.Position.Y;
            return true;
        }
        if (sensor2.Position.Y == sensor3.Position.Y)
        {
            y = sensor2.Position.Y;
            return true;
        }
        if (sensor0.Position.X == sensor1.Position.X)
        {
            Sensor lowest = sensor0.Position.Y < sensor1.Position.Y ? sensor0 : sensor1;
            y = lowest.Position.Y + lowest.Radius + 1;
            return true;
        }
        if (sensor2.Position.X == sensor3.Position.X)
        {
            Sensor lowest = sensor2.Position.Y < sensor3.Position.Y ? sensor2 : sensor3;
            y = lowest.Position.Y + lowest.Radius + 1;
            return true;
        }

        y = MoreMath.Average(sensor0.Position.Y, sensor1.Position.Y, sensor2.Position.Y, sensor3.Position.Y);
        return true;
    }

    private static int FirstPossiblePosition(List<Sensor> sensors, int row)
    {
        List<(int, int)> exclusionZones = [];
        foreach (Sensor sensor in sensors)
        {
            if (sensor.HasExclusionZone(row, out int xMin, out int xMax))
            {
                exclusionZones.Add((xMin, xMax));
            }
        }
        List<(int, int)> activeRanges = [];
        foreach ((int, int) range in exclusionZones)
        {
            List<(int, int)> touchingRanges = activeRanges.Where(r => RangesTouch(r, range)).ToList();
            if (touchingRanges.Count == 0)
            {
                activeRanges.Add(range);
                continue;
            }
            (int, int) merged = range;
            foreach ((int, int) connection in touchingRanges)
            {
                merged = MergeRanges(merged, connection);
                activeRanges.Remove(connection);
            }
            activeRanges.Add(merged);
        }
        return activeRanges[0].Item2 + 1;
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

    private static bool RangesTouch((int xMin, int xMax) a, (int xMin, int xMax) b)
    {
        if (RangesIntersect(a, b))
        {
            return true;
        }
        return a.xMax + 1 == b.xMin || b.xMax + 1 == a.xMin;
    }

    private static (int, int) MergeRanges((int xMin, int xMax) a, (int xMin, int xMax) b)
    {
        return (Math.Min(a.xMin, b.xMin), Math.Max(a.xMax, b.xMax));
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
