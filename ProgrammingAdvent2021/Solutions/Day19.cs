// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;
using ProgrammingAdvent2021.Utilities;

namespace ProgrammingAdvent2021.Solutions;

internal class Day19 : Day
{
    private static readonly MD5 md5 = MD5.Create();

    private static readonly (Vector3Int, Vector3Int)[] orientations =
    [
        (Vector3Int.UnitX, Vector3Int.UnitY),
        (Vector3Int.UnitX, -Vector3Int.UnitY),
        (Vector3Int.UnitX, Vector3Int.UnitZ),
        (Vector3Int.UnitX, -Vector3Int.UnitZ),
        (-Vector3Int.UnitX, Vector3Int.UnitY),
        (-Vector3Int.UnitX, -Vector3Int.UnitY),
        (-Vector3Int.UnitX, Vector3Int.UnitZ),
        (-Vector3Int.UnitX, -Vector3Int.UnitZ),
        (Vector3Int.UnitY, Vector3Int.UnitX),
        (Vector3Int.UnitY, -Vector3Int.UnitX),
        (Vector3Int.UnitY, Vector3Int.UnitZ),
        (Vector3Int.UnitY, -Vector3Int.UnitZ),
        (-Vector3Int.UnitY, Vector3Int.UnitX),
        (-Vector3Int.UnitY, -Vector3Int.UnitX),
        (-Vector3Int.UnitY, Vector3Int.UnitZ),
        (-Vector3Int.UnitY, -Vector3Int.UnitZ),
        (Vector3Int.UnitZ, Vector3Int.UnitX),
        (Vector3Int.UnitZ, -Vector3Int.UnitX),
        (Vector3Int.UnitZ, Vector3Int.UnitY),
        (Vector3Int.UnitZ, -Vector3Int.UnitY),
        (-Vector3Int.UnitZ, Vector3Int.UnitX),
        (-Vector3Int.UnitZ, -Vector3Int.UnitX),
        (-Vector3Int.UnitZ, Vector3Int.UnitY),
        (-Vector3Int.UnitZ, -Vector3Int.UnitY),
    ];

    protected override (string, string) CalculateAnswers(string[] input)
    {
        int currentScanner = -1;
        List<Scanner> scanners = [];
        foreach (string line in input)
        {
            Match scannerMatch = GeneratedRegex.ValidDay19Scanner.Match(line);
            if (scannerMatch.Success)
            {
                int newScanner = int.Parse(scannerMatch.Groups["scanner"].Value);
                if (currentScanner + 1 != newScanner)
                {
                    return ("Scanners out of order.", "n/a");
                }
                currentScanner = newScanner;
                scanners.Add(new Scanner(currentScanner));
                continue;
            }
            Match coordinatesMatch = GeneratedRegex.ValidDay19Coordinates.Match(line);
            if (coordinatesMatch.Success)
            {
                int x = int.Parse(coordinatesMatch.Groups["x"].Value);
                int y = int.Parse(coordinatesMatch.Groups["y"].Value);
                int z = int.Parse(coordinatesMatch.Groups["z"].Value);
                scanners[currentScanner].AddBeacon(x, y, z);
            }
        }
        foreach (Scanner scanner in scanners)
        {
            scanner.HashBeacons();
        }
        for (int i = 0; i < scanners.Count - 1; i++)
        {
            for (int j = i + 1; j < scanners.Count; j++)
            {
                int possibleOverlap = scanners[i].PossibleOverlap(scanners[j]);
                if (possibleOverlap >= 9)
                {
                    scanners[i].Connections.Add(scanners[j]);
                    scanners[j].Connections.Add(scanners[i]);
                }
            }
        }
        if (!BuildTree(scanners, scanners[0]))
        {
            return ("Solution not found", "n/a");
        }
        FindScannerPositions(scanners, scanners[0]);
        HashSet<Vector3Int> beacons = [];
        foreach (Scanner scanner in scanners)
        {
            foreach (Beacon b in scanner.Beacons)
            {
                beacons.Add(scanner.BeaconToGlobalCoords(b));
            }
        }
        int partOneAnswer = beacons.Count;
        return ($"{partOneAnswer}", "n/a");
    }

    private static int Distance(Beacon a, Beacon b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
    }

    private static int Hash(params int[] numbers)
    {
        byte[] bytes = new byte[numbers.Length * 4];
        for (int i = 0; i < numbers.Length; i++)
        {
            byte[] current = BitConverter.GetBytes(numbers[i]);
            Array.Copy(current, 0, bytes, i * 4, 4);
        }
        byte[] hash = md5.ComputeHash(bytes);
        return BitConverter.ToInt32(hash, 0);
    }

    private static bool BuildTree(List<Scanner> scanners, Scanner scanner, int depth = 0)
    {
        foreach (Scanner connection in scanner.Connections)
        {
            if (connection.ID == 0) { continue; }
            if (connection.Parent is null)
            {
                connection.Parent = scanner;
                scanner.Children.Add(connection);
            }
        }
        foreach (Scanner child in scanner.Children)
        {
            BuildTree(scanners, child, depth + 1);
        }
        if (depth == 0)
        {
            foreach (Scanner s in scanners)
            {
                if (s.Parent is null && s.ID != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static void FindScannerPositions(List<Scanner> scanners, Scanner scanner)
    {
        foreach (Scanner child in scanner.Children)
        {
            SetChildPosition(scanner, child);
            FindScannerPositions(scanners, child);
        }
    }

    private static void SetChildPosition(Scanner scanner, Scanner child)
    {
        List<(Beacon, Beacon)> commonBeacons = scanner.CommonBeacons(child);
        foreach ((Vector3Int X, Vector3Int Y) orientation in orientations)
        {
            child.LocalX = orientation.X;
            child.LocalY = orientation.Y;
            if (OrientationIsCorrect(scanner, child, commonBeacons, out Vector3Int? offset))
            {
                child.GlobalPosition = (Vector3Int)offset;
                return;
            }
        }
    }

    private static bool OrientationIsCorrect(Scanner scanner, Scanner child,
        List<(Beacon, Beacon)> commonBeacons, [NotNullWhen(true)] out Vector3Int? offset)
    {
        ListMC<Vector3Int> offsets = new();
        foreach ((Beacon a, Beacon b) in commonBeacons)
        {
            Vector3Int positionA = scanner.BeaconToGlobalCoords(a);
            Vector3Int positionB = child.BeaconToGlobalCoords(b);
            offsets.Add(positionA - positionB);
        }
        (Vector3Int mostCommonOffset, int frequency) = offsets.GetMostCommon();
        offset = mostCommonOffset;
        return frequency >= 6;
    }

    private static Vector3Int CrossProduct(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
    }

    private class Beacon(int x, int y, int z)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; } = z;
        public Vector3Int Position { get; } = new(x, y, z);
        public int Hash { get; set; }
    }

    private class Scanner(int id)
    {

        public int ID { get; } = id;
        public Scanner? Parent { get; set; }
        public List<Scanner> Children { get; } = [];
        public HashSet<Scanner> Connections { get; } = [];
        public List<Beacon> Beacons { get; } = [];

        private Vector3Int _localX = Vector3Int.UnitX;
        private Vector3Int _localY = Vector3Int.UnitY;
        private Vector3Int _localZ = Vector3Int.UnitZ;
        public Vector3Int LocalX
        {
            get => _localX;
            set
            {
                _localX = value;
                _localZ = CrossProduct(_localX, _localY);
            }
        }
        public Vector3Int LocalY
        {
            get => _localY;
            set
            {
                _localY = value;
                _localZ = CrossProduct(_localX, _localY);
            }
        }
        public Vector3Int LocalZ { get => _localZ; }
        public Vector3Int GlobalPosition { get; set; } = Vector3Int.Zero;

        public void AddBeacon(int x, int y, int z)
        {
            Beacons.Add(new Beacon(x, y, z));
        }

        public void HashBeacons()
        {
            Dictionary<(int, int), int> distances = [];
            for (int i = 0; i < Beacons.Count - 1; i++)
            {
                for (int j = i + 1; j < Beacons.Count; j++)
                {
                    distances.Add((i, j), Distance(Beacons[i], Beacons[j]));
                    distances.Add((j, i), Distance(Beacons[i], Beacons[j]));
                }
            }
            for (int i = 0; i < Beacons.Count; i++)
            {
                List<int> d = [];
                for (int j = 0; j < Beacons.Count; j++)
                {
                    if (i == j) { continue; }
                    d.Add(distances[(i, j)]);
                }
                d.Sort();
                Beacons[i].Hash = Hash(d[0], d[1]);
            }
        }

        public int PossibleOverlap(Scanner other)
        {
            return Beacons.Select(b => b.Hash).Intersect(other.Beacons.Select(b => b.Hash)).Count();
        }

        public List<(Beacon, Beacon)> CommonBeacons(Scanner other)
        {
            List<(Beacon, Beacon)> common = [];
            foreach (Beacon beacon in Beacons)
            {
                Beacon? match = other.Beacons.FirstOrDefault(b => beacon.Hash == b.Hash);
                if (match is not null)
                {
                    common.Add((beacon, match));
                }
            }
            return common;
        }

        public Vector3Int BeaconToGlobalCoords(Beacon beacon)
        {
            return GlobalPosition + beacon.X * LocalX + beacon.Y * LocalY + beacon.Z * LocalZ;
        }
    }

    private class ListMC<T>
    {
        private readonly List<T> _items = [];

        public void Add(T item) => _items.Add(item);

        public (T item, int frequency) GetMostCommon()
        {
            IGrouping<T, T>? mostCommon = _items
                .GroupBy(i => i)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault();
            return mostCommon is null
                ? throw new InvalidOperationException()
                : (mostCommon.Key, mostCommon.Count());
        }
    }
}
